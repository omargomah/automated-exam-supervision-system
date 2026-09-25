#include <Arduino.h>
#include "config.h"
#include "wifi_manager.h"
#include "audio_telemetry.h"
#include "rf_ble_sniffer.h"
#include "telemetry_publisher.h"

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
}

void loop() {
    // 1. Maintain Network Connection
    handleWiFiReconnect(WIFI_SSID, WIFI_PASS);

    // 2. Read Sensors
    float audioVoltage = readAudioPeakVoltage(AUDIO_PIN, AUDIO_SAMPLE_WINDOW_MS);
    float rfVoltage = readRFPeakVoltage(RF_PIN);
    int nearbyBLECount = scanBLEDevicesInProximity(BLE_SCAN_DURATION_S, BLE_RSSI_PROXIMITY_THRESHOLD);

    // 3. Assemble Telemetry Payload
    TelemetryData telemetry;
    telemetry.roomId = ROOM_ID;
    telemetry.rfVoltage = rfVoltage;
    telemetry.soundPeakVoltage = audioVoltage;
    telemetry.bleDeviceCount = nearbyBLECount;

    // 4. Transmit Payload to ASP.NET Core API Gateway
    publishTelemetry(API_INGEST_ENDPOINT, telemetry);

    delay(500);
}
