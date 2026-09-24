#include <Arduino.h>
#include "config.h"
#include "wifi_manager.h"
#include "audio_telemetry.h"

void setup() {
    Serial.begin(115200);
    delay(1000);

    Serial.println("\n=================================");
    Serial.println("  Anti-Cheat Supervision Node    ");
    Serial.println("  Status: Initializing...        ");
    Serial.println("=================================");

    initWiFi(WIFI_SSID, WIFI_PASS);
    initAudioSensor(AUDIO_PIN);
}

void loop() {
    handleWiFiReconnect(WIFI_SSID, WIFI_PASS);
    
    // Read audio peak voltage
    float audioVoltage = readAudioPeakVoltage(AUDIO_PIN, AUDIO_SAMPLE_WINDOW_MS);
    
    if (isAcousticAnomalyDetected(audioVoltage, AUDIO_ANOMALY_THRESHOLD_V)) {
        Serial.printf("[WARNING] Acoustic Anomaly Detected! Sound Voltage: %.2f V\n", audioVoltage);
    }

    delay(200);
}
