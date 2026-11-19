#include <Arduino.h>
#include "config.h"
#include "wifi_manager.h"
#include "audio_telemetry.h"
#include "rf_ble_sniffer.h"
#include "telemetry_publisher.h"

// Timing intervals (in milliseconds)
unsigned long lastTelemetryRun = 0;
const unsigned long TELEMETRY_INTERVAL_MS = 10000; // Run detection and publish every 10 seconds

void setup() {
    Serial.begin(SERIAL_BAUD_RATE);
    delay(1000);

    Serial.println("\n=================================");
    Serial.println("  Anti-Cheat Supervision Node    ");
    Serial.println("  Status: Initializing...        ");
    Serial.println("=================================");

    initWiFi(WIFI_SSID, WIFI_PASS);
    initAudioSensor(AUDIO_PIN);
    initRFSniffer(RF_PIN);
    initBLEScanner();
    initTelemetryPublisher();

    Serial.println();
    Serial.print("ESP32 Station MAC Address: ");
    Serial.println(WiFi.macAddress());
}

void loop() {
    // 1. Maintain Network Connection (must run frequently)
    handleWiFiReconnect(WIFI_SSID, WIFI_PASS);

    unsigned long currentMillis = millis();

    // 2. Run sensor scans and telemetry dispatch on a controlled interval
    if (currentMillis - lastTelemetryRun >= TELEMETRY_INTERVAL_MS) {
        lastTelemetryRun = currentMillis;

        Serial.println("[STATUS] Executing scheduled sensor scan cycle...");

        // Read Sensors
        float audioVoltage = readAudioPeakVoltage(AUDIO_PIN, AUDIO_SAMPLE_WINDOW_MS);
        float rfVoltage = readRFPeakVoltage(RF_PIN);
        int nearbyBLECount = scanBLEDevicesInProximity(BLE_SCAN_DURATION_S, BLE_RSSI_PROXIMITY_THRESHOLD);
        int suspiciousAPs = scanAndGetSuspiciousAPCount();

        // Assemble Telemetry Payload
        TelemetryData telemetry;
        telemetry.roomId = ROOM_ID;
        telemetry.rfVoltage = rfVoltage;
        telemetry.soundPeakVoltage = audioVoltage;
        telemetry.bleDeviceCount = nearbyBLECount;
        telemetry.wifiConnected = (WiFi.status() == WL_CONNECTED);
        telemetry.wifiRssi = telemetry.wifiConnected ? WiFi.RSSI() : 0;
        telemetry.suspiciousAPCount = suspiciousAPs;

        // Transmit Payload to ASP.NET Core API Gateway
        publishTelemetry(API_INGEST_ENDPOINT, telemetry);
    }

    // Small yield to feed the FreeRTOS task watchdog
    delay(50);
}