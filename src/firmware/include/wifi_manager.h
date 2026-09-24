#ifndef WIFI_MANAGER_H
#define WIFI_MANAGER_H

#include <Arduino.h>
#include <WiFi.h>

// Function Declarations
void initWiFi(const char* ssid, const char* password);
void handleWiFiReconnect(const char* ssid, const char* password);
bool isWiFiConnected();

#endif // WIFI_MANAGER_H
