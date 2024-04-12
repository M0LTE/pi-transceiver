# Arduino Support for Pi Transciever PA Board
# Arduino Support for Pi Transciever PA Board

The Pi Transceiver PA board is programmed under the Arduino IDE environment with Arduino Libraries










## Test programs

    - Blink - Basic test to verify CPU operation with RGB LED and watchdog LED

    - I2C Scanner - Find I2c devices on Internal and External I2C buses

    - OLED Test - Test of optional I2C OLED display

    - Temperature - Show temperature of LM75B sensor

    - Power_Tests - Test Vdd, Idd, Vgg Monitor
    
    - fan_PWM - test speed control of fan

    - USB_Commander - a command interpreter interface to set and read all parameters of the PA and receiver boards
    



## Libraries Used

    - "FlashStorage_SAMD"  used to support non voltaile storage from application into Flash memory as there is no EEPROM on ATSAMD21E18

    - "Adafruit_INA219" used to support the INA219 voltage and current sensor for Vdd and Idd to the RF Block

    - "Adafruit_MCP4725" used to support the DAC used to set the Vgg voltage

    - "Commander" used to provide command interpreter function on USB or TTL Serial port

    - "Adafruit_Neopixel"  used to support the onboard DotStar RGB LED

    - "Temperature_LM75_Derived" used to support the onboard LM75B temperature sensor

    - "Adafruit_SSD1306" and "Adafruit GFX" are Used to support the optional 128 * 64 pixel monochrome I2C LCD display

    - "PCF8574" is used to support the receiver board I2C interface IC



