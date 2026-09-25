#ifndef RF_BLE_SNIFFER_H
#define RF_BLE_SNIFFER_H

#include <Arduino.h>
#include <BLEDevice.h>
#include <BLEUtils.h>
#include <BLEScan.h>

// Function Declarations
void initRFSniffer(int pin);
float readRFPeakVoltage(int pin);

void initBLEScanner();
int scanBLEDevicesInProximity(int scanDurationSeconds, int rssiThreshold);

#endif // RF_BLE_SNIFFER_H
