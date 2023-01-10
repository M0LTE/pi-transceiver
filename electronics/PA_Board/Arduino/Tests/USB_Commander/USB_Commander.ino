/*Commander example - basic
 * Demonstrating commands to get and set an int and a float
 */
#include <Commander.h>
#include <Adafruit_DotStar.h>
#include <Adafruit_INA219.h>
#include <Adafruit_MCP4725.h>
#include <Temperature_LM75_Derived.h>

#define VGG_EN_PIN  10  // Vgg Enable Pin
#define VGG_MINIMUM 4095
#define VGG_MAXIMUM 0
#define TX_RELAY_PIN 5
#define VGG_MIN_VOLTS 2.0f
#define VGG_MAX_VOLTS 5.85f
#define VGG_SPAN_VOLTS (VGG_MAX_VOLTS - VGG_MIN_VOLTS)

Commander cmd;
Adafruit_MCP4725 dac;
Adafruit_INA219 ina219;
Generic_LM75_12Bit temperature(&Wire2);


//Variables we can set or get
int myInt = 0;
float myFloat = 0.0;

Adafruit_DotStar strip = Adafruit_DotStar(DOTSTAR_NUM, PIN_DOTSTAR_DATA, PIN_DOTSTAR_CLK, DOTSTAR_BGR);

uint8_t Dotstar_Colours[3] = {64,0,0};
bool LED_state = false;
bool TX_state = false;
bool VGG_state = false;
uint16_t DAC_value;


String deviceInfo = "RADARC Pi Transceiver PA BOARD USB Commander";


//SETUP ---------------------------------------------------------------------------
void setup() {
  Serial.begin(115200);
  Wire2.begin(); // Internal I2C Bus
  strip.begin();
  pinMode(LED_BUILTIN, OUTPUT); 
  digitalWrite(LED_BUILTIN, LOW);
  strip.setPixelColor(0, Dotstar_Colours[0],Dotstar_Colours[1],Dotstar_Colours[2]); strip.show();

  pinMode(VGG_EN_PIN, OUTPUT); 
  digitalWrite(VGG_EN_PIN, LOW);

  pinMode(TX_RELAY_PIN, OUTPUT); 
  digitalWrite(TX_RELAY_PIN, LOW);

  dac.begin(0x61, &Wire2);
  dac.setVoltage( VGG_MINIMUM, false);
  DAC_value = VGG_MINIMUM;

  ina219.begin(&Wire2);
  
  analogReadResolution(ADC_RESOLUTION); // set ADC resolution to 12 bits


  initialiseCommander();
  while(!Serial){;}
  cmd.printUserString();
  cmd.println();
  Serial.println("Type 'help' to get help");
  cmd.printCommandPrompt();
}

//MAIN LOOP ---------------------------------------------------------------------------
void loop() {
  cmd.update();
}
