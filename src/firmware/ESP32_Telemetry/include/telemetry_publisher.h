#ifndef TELEMETRY_PUBLISHER_H
#define TELEMETRY_PUBLISHER_H

#include <Arduino.h>
#include <HTTPClient.h>
#include <WiFiClientSecure.h>
#include <ArduinoJson.h>

struct TelemetryData {
    const char* roomId;
    float rfVoltage;
    float soundPeakVoltage;
    int bleDeviceCount;
    bool wifiConnected; // Added
    int wifiRssi;       // Added
    int suspiciousAPCount;  // Added
};

void initTelemetryPublisher();
bool publishTelemetry(const char* apiEndpoint, const TelemetryData& data);

#endif // TELEMETRY_PUBLISHER_H
