#include "rf_ble_sniffer.h"
#include <set>
#include <string.h>


static BLEScan* pBLEScan = nullptr;

void initRFSniffer(int pin) {
    pinMode(pin, INPUT);
    Serial.printf("[RF] Initialized passive RF peak detector on ADC pin %d\n", pin);
}

float readRFPeakVoltage(int pin) {
    int rawValue = analogRead(pin);
    // Convert 12-bit ADC value (0-4095) to voltage (0.0V - 3.3V)
    float rfVoltage = (rawValue / 4095.0) * 3.3;
    return rfVoltage;
}

void initBLEScanner() {
    BLEDevice::init("");
    pBLEScan = BLEDevice::getScan();
    pBLEScan->setActiveScan(true); // Active scan requests scan response packets
    pBLEScan->setInterval(100);
    pBLEScan->setWindow(99);
    Serial.println("[BLE] Scanner initialized.");
}


int scanBLEDevicesInProximity(int scanDurationSeconds, int rssiThreshold) {
    if (!pBLEScan) return 0;

    BLEScanResults* foundDevices = pBLEScan->start(scanDurationSeconds, false);
    std::set<std::string> uniqueDevices;

    if (foundDevices != nullptr) {
        for (int i = 0; i < foundDevices->getCount(); i++) {
            BLEAdvertisedDevice device = foundDevices->getDevice(i);
            int rssi = device.getRSSI();

            // Check RSSI threshold (ensure threshold is negative, e.g., -80)
            if (rssi > rssiThreshold) {
                std::string macAddress = device.getAddress().toString().c_str();
                uniqueDevices.insert(macAddress); // std::set automatically drops duplicates
            }
        }
    }

    pBLEScan->clearResults(); 
    return uniqueDevices.size(); // Returns exact number of unique physical devices
}