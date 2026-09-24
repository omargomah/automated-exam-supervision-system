#include <Arduino.h>
#include "config.h"

void setup() {
    Serial.begin(115200);
    delay(1000);

    Serial.println("\n=================================");
    Serial.println("  Anti-Cheat Supervision Node    ");
    Serial.println("  Status: Initializing...        ");
    Serial.println("=================================");

    #if defined(BOARD_HAS_PSRAM)
    if (psramInit()) {
        Serial.printf("PSRAM initialized successfully. Free PSRAM: %d bytes\n", ESP.getFreePsram());
    } else {
        Serial.println("PSRAM initialization failed!");
    }
    #endif

    Serial.printf("Free Heap: %d bytes\n", ESP.getFreeHeap());
    Serial.println("Hardware Node Ready.");
}

void loop() {
    delay(1000);
}
