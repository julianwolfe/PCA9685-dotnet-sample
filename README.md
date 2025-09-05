# PCA9685-dotnet-sample

This is a .NET sample application for controlling LEDs using the PCA9685 PWM controller on a Raspberry Pi.

## Features
- Fade and blink multiple LEDs independently and concurrently
- The first 3 outputs (channels 0, 1, and 2) each randomly and concurrently blink or fade, creating a dynamic light show
- Randomized timing and effects for each output
- Easy to modify for other PWM applications

## Hardware Setup
- Raspberry Pi (tested on Zero W 2)
- PCA9685 PWM controller board
- LEDs with current-limiting resistors

### Wiring
- Pi 3.3V → PCA9685 VCC
- Pi 5V → PCA9685 V+
- Pi GND → PCA9685 GND
- Pi SDA (GPIO2) → PCA9685 SDA
- Pi SCL (GPIO3) → PCA9685 SCL
- LED anode → V+
- LED cathode → PCA9685 output (0, 1, 2, ...)

## Usage
1. Install .NET 8.0 or newer on your Raspberry Pi.
2. Clone this repository and navigate to the project directory.
3. Restore dependencies:
   ```sh
   dotnet restore
   ```
4. Build the project:
   ```sh
   dotnet build
   ```
5. Run the application:
   ```sh
   dotnet run
   ```
6. Observe the first 3 LEDs fading and blinking independently, with each channel randomly choosing to blink or fade and running concurrently.

## License
MIT