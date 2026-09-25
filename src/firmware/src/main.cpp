#include <Arduino.h>
#include "config.h"
#include "wifi_manager.h"
#include "audio_telemetry.h"
#include "rf_ble_sniffer.h"
#include "telemetry_publisher.h"

void setup() {
    Serial.begin(115200);
    delay(1000);

    Serial.println("\n=================================");
    Serial.println("  Anti-Cheat Supervision Node    ");
    Serial.println("  Status: Initializing...        ");
    Serial.println("=================================");

    // Initialization of modules' functions 
    initWiFi(WIFI_SSID, WIFI_PASS);
    initAudioSensor(AUDIO_PIN);
    initRFSniffer(RF_PIN);
    initBLEScanner();
    initTelemetryPublisher();
}

void loop() {
    // 1. Maintain Network Connection
    handleWiFiReconnect(WIFI_SSID, WIFI_PASS);

    // 2. Sample Sensors
    float audioVoltage = readAudioPeakVoltage(AUDIO_PIN, AUDIO_SAMPLE_WINDOW_MS);
    float rfVoltage = readRFPeakVoltage(RF_PIN);
    int nearbyBLECount = scanBLEDevicesInProximity(BLE_SCAN_DURATION_S, BLE_RSSI_PROXIMITY_THRESHOLD);

    // Telemetry Diagnostics
    Serial.printf("[TELEMETRY] Audio: %.2fV | RF: %.2fV | Nearby BLE: %d\n", 
                  audioVoltage, rfVoltage, nearbyBLEDevices);

    if (rfVoltage > RF_SPIKE_THRESHOLD_V) {
        Serial.printf("[WARNING] RF Transmission Spike Detected! Voltage: %.2f V\n", rfVoltage);
    }
    
    if (isAcousticAnomalyDetected(audioVoltage, AUDIO_ANOMALY_THRESHOLD_V)) {
        Serial.printf("[WARNING] Acoustic Anomaly Detected! Sound Voltage: %.2f V\n", audioVoltage);
    }

    // 3. Assemble Telemetry Struct
    TelemetryData telemetry;
    telemetry.roomId = ROOM_ID;
    telemetry.rfVoltage = rfVoltage;
    telemetry.soundPeakVoltage = audioVoltage;
    telemetry.bleDeviceCount = nearbyBLECount;

    // 4. Publish Payload to ASP.NET Core Gateway
    publishTelemetry(API_INGEST_ENDPOINT, telemetry);

    delay(200);
}
