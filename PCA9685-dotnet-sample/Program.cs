using System;
using System.Device.I2c;
using System.Threading;

var rand = new Random();
Console.WriteLine("START OF FILE - Program.cs is executing");
Console.WriteLine("Each output will concurrently blink or fade independently. Press Ctrl+C to end.");

int i2cBusId = 1;
int pca9685Address = 0x40;
I2cDevice i2cDevice = I2cDevice.Create(new I2cConnectionSettings(i2cBusId, pca9685Address));

SetPwmFrequency(i2cDevice, 1000);
for (int ch = 0; ch < 3; ch++) SetPwm(i2cDevice, ch, 0, 0); // Ensure all LEDs are OFF at start

for (int ch = 0; ch < 3; ch++)
{
    int channel = ch; // Local copy for closure
    new Thread(() =>
    {
        while (true)
        {
            bool doFade = rand.Next(2) == 0;
            if (doFade)
            {
                int fadeStep = rand.Next(8, 64);
                int fadeDelay = rand.Next(2, 20);
                Console.WriteLine($"FADING {channel}");
                for (int duty = 0; duty <= 4095; duty += fadeStep)
                {
                    SetPwm(i2cDevice, channel, 0, duty);
                    Thread.Sleep(fadeDelay);
                }
                for (int duty = 4095; duty >= 0; duty -= fadeStep)
                {
                    SetPwm(i2cDevice, channel, 0, duty);
                    Thread.Sleep(fadeDelay);
                }
            }
            else
            {
                int blinkCount = rand.Next(2, 6);
                int onDelay = rand.Next(200, 800);
                int offDelay = rand.Next(200, 800);
                Console.WriteLine($"BLINKING {channel} {blinkCount}x");
                for (int i = 0; i < blinkCount; i++)
                {
                    SetPwm(i2cDevice, channel, 0, 4095);
                    Thread.Sleep(onDelay);
                    SetPwm(i2cDevice, channel, 0, 0);
                    Thread.Sleep(offDelay);
                }
            }
        }
    }) { IsBackground = true }.Start();
}

Thread.Sleep(Timeout.Infinite); // Keep main thread alive

void SetPwm(I2cDevice device, int channel, int on, int off)
{
    int reg = 0x06 + 4 * channel;
    byte[] data = new byte[5];
    data[0] = (byte)reg;
    data[1] = (byte)(on & 0xFF);
    data[2] = (byte)((on >> 8) & 0xFF);
    data[3] = (byte)(off & 0xFF);
    data[4] = (byte)((off >> 8) & 0xFF);
    device.Write(data);
}

void SetPwmFrequency(I2cDevice device, int freq)
{
    double prescaleval = 25000000.0 / 4096.0 / freq - 1.0;
    byte prescale = (byte)Math.Floor(prescaleval + 0.5);
    device.Write(new byte[] { 0x00, 0x10 });
    device.Write(new byte[] { 0xFE, prescale });
    device.Write(new byte[] { 0x00, 0x80 });
    Thread.Sleep(5);
    device.Write(new byte[] { 0x00, 0xA1 });
}


