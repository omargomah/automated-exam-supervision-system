# Passive RF Peak Detector & Signal Monitoring Front-End

![License](https://img.shields.io/badge/license-MIT-blue.svg)
![Proteus](https://img.shields.io/badge/Simulation-Proteus%20ISIS-orange)
![Target MCU](https://img.shields.io/badge/MCU-ESP32-green)
![Status](https://img.shields.io/badge/Status-Verified-success)

An ultra-low-cost, passive Radio Frequency (RF) peak detector and voltage doubler designed to capture ambient high-frequency electromagnetic power bursts (such as 2.4 GHz Bluetooth earbuds, Wi-Fi activity, or cellular uplink signals) and convert them into a readable DC voltage for microcontrollers like the **ESP32**.

---

## Features

* **Passive RF Energy Harvesting:** Operates on the incoming radio wave energy without needing an active power supply for the front-end sensor.
* **Greinacher Voltage Doubler:** Utilizes dual Schottky diode stages to double the rectified output DC voltage, boosting sensitivity to weak signals.
* **Zero False-Positive Hold:** Built-in bleeder resistor network ($R_1 = 10\text{ k}\Omega$) ensures immediate capacitor discharge when the RF burst stops.
* **Microcontroller Ready:** Provides a clean $0\text{V} - 3.3\text{V}$ DC output range suitable for ESP32/Arduino Analog-to-Digital Converter (ADC) pins.

---

## Circuit Schematic & Theory

### Schematic Diagram
![Proteus Schematic](docs/hardware/schematics/rf-peak-detector.png)

### Theoretical Model
The circuit converts incoming high-frequency AC signals caught by a $1/4\lambda$ wire antenna into a smooth DC output using a two-stage voltage multiplication topology:

1. **Clamping Phase (Negative Half-Cycle):** Diode $D_1$ conducts to Ground, charging input capacitor $C_1$ to $V_{peak}$.
2. **Rectification Phase (Positive Half-Cycle):** Diode $D_2$ conducts, adding the incoming signal peak to $C_1$'s stored charge into output filter $C_2$.

$$\text{Ideal Output: } V_{out} = 2 \times V_{peak}$$
$$\text{Real-World Output: } V_{out} \approx (2 \times V_{peak}) - (2 \times V_f)$$

*Where $V_f \approx 0.35\text{V}$ is the forward voltage drop of the `BAT54C` Schottky diodes.*

---

## Hardware Bill of Materials (BOM)

| Component | Designator | Value / Part | Description |
| :--- | :--- | :--- | :--- |
| **Schottky Diodes** | $D_1, D_2$ | `BAT54C` (2x) | Dual Small-Signal Schottky Diodes (Common Cathode) |
| **Input Capacitor** | $C_1$ | $10\text{ nF}$ Ceramic | AC Coupling / DC Blocking Capacitor |
| **Filter Capacitor** | $C_2$ | $10\text{ nF}$ Ceramic | DC Smoothing Capacitor |
| **Bleeder Resistor** | $R_1$ | $10\text{ k}\Omega$ | Discharge Load Resistor |
| **Antenna** | ANT1 | $31\text{ mm}$ Copper Wire | Cut to $1/4$ wavelength for 2.4 GHz band |

---

## Simulation Setup (Proteus)

1. Open `simulations/rf_detector.pdsprj` in **Proteus ISIS**.
2. **Signal Generator Configuration:**
   * **Type:** `VSINE`
   * **Amplitude:** `1.0V`
   * **Frequency:** `1 MHz` *(Optimized for SPICE engine real-time execution)*
3. **Running the Test:**
   * Press **Play** in Proteus.
   * Observe **Channel A** (AC Input Waveform) vs. **Channel B** (DC Output across $R_1$).
   * Verified output level: **$\approx 0.7\text{ V} - 0.8\text{ V}$ DC**.

---

## Microcontroller Integration (ESP32)

Connect the `OUTPUT` pin of the detector circuit to **GPIO 34 (ADC1_CH6)** on your ESP32.

### Sample Code (`src/main.cpp`)

```cpp
#include <Arduino.h>

const int RF_ADC_PIN = 34;
const float VOLTAGE_THRESHOLD = 0.25; // Voltage threshold for RF detection

void setup() {
  Serial.begin(115200);
  analogReadResolution(12); // 0 - 4095
}

void loop() {
  int rawADC = analogRead(RF_ADC_PIN);
  float voltage = (rawADC / 4095.0) * 3.3;

  if (voltage > VOLTAGE_THRESHOLD) {
    Serial.printf("[ALERT] RF Transmission Detected! Level: %.2f V\n", voltage);
  }

  delay(100);
}
