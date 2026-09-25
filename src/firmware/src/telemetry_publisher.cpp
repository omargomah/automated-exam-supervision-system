#include "telemetry_publisher.h"

void initTelemetryPublisher() {
    Serial.println("[Telemetry] Publisher initialized.");
}

bool publishTelemetry(const char* apiEndpoint, const TelemetryData& data) {
    if (WiFi.status() != WL_CONNECTED) {
        Serial.println("[Telemetry] Error: Cannot publish payload, Wi-Fi not connected.");
        return false;
    }

    // Allocate JSON document buffer
    StaticJsonDocument<256> doc;
    doc["roomId"] = data.roomId;
    doc["rfVoltage"] = data.rfVoltage;
    doc["soundPeakVoltage"] = data.soundPeakVoltage;
    doc["bleDeviceCount"] = data.bleDeviceCount;

    String jsonPayload;
    serializeJson(doc, jsonPayload);

    WiFiClientSecure secureClient;
    secureClient.setInsecure(); // Bypass TLS cert verification for local dev environment

    HTTPClient http;
    if (!http.begin(secureClient, apiEndpoint)) {
        Serial.println("[Telemetry] Failed to initialize HTTP client.");
        return false;
    }

    http.addHeader("Content-Type", "application/json");

    Serial.println("[Telemetry] Transmitting payload: " + jsonPayload);
    int httpResponseCode = http.POST(jsonPayload);

    bool success = false;
    if (httpResponseCode > 0) {
        Serial.printf("[Telemetry] Server Response Code: %d\n", httpResponseCode);
        if (httpResponseCode == 200 || httpResponseCode == 201) {
            success = true;
        }
    } else {
        Serial.printf("[Telemetry] POST Request Failed! Error: %s\n", http.errorToString(httpResponseCode).c_str());
    }

    http.end();
    return success;
}
