/* Command Interpeter and Display for RADARC Pi Transceiver PA board
 * Version 2.0 December 2023
 * Richard Ibbotson
 */


#include <Commander.h>
#include <Adafruit_NeoPixel.h>
#include <Adafruit_INA219.h>
#include <Adafruit_MCP4725.h>
#include <Temperature_LM75_Derived.h>
#include <Adafruit_SSD1306.h>
#include <FlashStorage_SAMD.h>



#define VGG_MINIMUM 4095 // DAC value corresponding to Minimum Vgg (2.0V)
#define VGG_MAXIMUM 0    // DAC value corresponding to Maximum Vgg (5,85V)
#define VGG_MIN_VOLTS (2.0f)
#define VGG_MAX_VOLTS (5.85f)
#define VGG_SPAN_VOLTS (VGG_MAX_VOLTS - VGG_MIN_VOLTS)


#define SCREEN_WIDTH 128 // OLED display width, in pixels
#define SCREEN_HEIGHT 64 // OLED display height, in pixels
#define OLED_RESET     -1 // Reset pin # (or -1 if sharing Arduino reset pin)
#define SCREEN_ADDRESS 0x3C ///< See datasheet for Address; 0x3D for 128x64, 0x3C for 128x32

#define RADARC_LOGO_HEIGHT   44
#define RADARC_LOGO_WIDTH    68

  // 'RADARC68x44', 68x44px
const unsigned char RADARC68x44 [] PROGMEM = {
	0xfc, 0x00, 0x03, 0xff, 0xbf, 0xfc, 0x00, 0x03, 0xf0, 0xfc, 0x00, 0x00, 0x7f, 0x6f, 0xe0, 0x00, 
	0x03, 0xf0, 0xff, 0xff, 0xf8, 0x1f, 0x6f, 0x81, 0xff, 0xff, 0xf0, 0xff, 0xff, 0xff, 0x0f, 0x5f, 
	0x0f, 0xff, 0xff, 0xf0, 0xff, 0xff, 0xff, 0x8f, 0x6f, 0x1f, 0xff, 0xff, 0xf0, 0xff, 0xff, 0xff, 
	0xc7, 0xee, 0x3f, 0xff, 0xff, 0xf0, 0xff, 0xff, 0xff, 0xc7, 0xfe, 0x3f, 0xff, 0xff, 0xf0, 0xff, 
	0xff, 0xff, 0xcf, 0xff, 0x3f, 0xff, 0xff, 0xf0, 0xff, 0xff, 0xff, 0x8f, 0xbf, 0x1f, 0xff, 0xff, 
	0xf0, 0xff, 0xff, 0xfe, 0x1f, 0x9f, 0x87, 0xff, 0xff, 0xf0, 0xff, 0xff, 0xe0, 0x3f, 0x5f, 0xc0, 
	0x7f, 0xff, 0xf0, 0xff, 0xe7, 0xe0, 0x7f, 0x0f, 0xe0, 0x7e, 0x7f, 0xf0, 0xff, 0xdb, 0xe0, 0x3f, 
	0x6f, 0xc0, 0x7d, 0xbf, 0xf0, 0xff, 0xbd, 0xfe, 0x1f, 0xff, 0x87, 0xfb, 0xdf, 0xf0, 0xff, 0xbd, 
	0xff, 0x8f, 0xff, 0x1f, 0xfb, 0xdf, 0xf0, 0xff, 0xbd, 0xff, 0xcf, 0xff, 0x3f, 0xfb, 0xdf, 0xf0, 
	0xff, 0x7e, 0xff, 0xc7, 0x4e, 0x3f, 0xf7, 0xef, 0xf0, 0xff, 0x7e, 0xff, 0xc7, 0x6e, 0x3f, 0xf7, 
	0xef, 0xf0, 0xff, 0x7e, 0xff, 0xcf, 0x6f, 0x3f, 0xf7, 0xef, 0xf0, 0xff, 0x7e, 0xff, 0x0f, 0x6f, 
	0x0f, 0xf7, 0xef, 0xf0, 0xff, 0x7e, 0xfc, 0x1f, 0x1f, 0x83, 0xf7, 0xef, 0xf0, 0xff, 0x7e, 0xc0, 
	0x7f, 0xff, 0xe0, 0x37, 0xef, 0xf0, 0xff, 0x7e, 0xe0, 0x7f, 0xff, 0xe0, 0x77, 0xef, 0xf0, 0x7f, 
	0x7f, 0xf8, 0x1f, 0xbf, 0x81, 0xff, 0xef, 0xe0, 0x7f, 0x7f, 0xff, 0x0f, 0x9f, 0x0f, 0xff, 0xef, 
	0xe0, 0x7f, 0x7f, 0xff, 0xcf, 0x5f, 0x3f, 0xff, 0xef, 0xe0, 0x7f, 0xff, 0xff, 0xc7, 0x0e, 0x3f, 
	0xff, 0xff, 0xe0, 0x7e, 0xff, 0xff, 0xc7, 0x6e, 0x3f, 0xff, 0xf7, 0xe0, 0x7e, 0xff, 0xff, 0xcf, 
	0xff, 0x3f, 0xff, 0xf7, 0xe0, 0xfe, 0xff, 0xff, 0x8f, 0xff, 0x1f, 0xff, 0xf7, 0xf0, 0xbe, 0xff, 
	0xfe, 0x1f, 0xff, 0x87, 0xff, 0xf7, 0xd0, 0xbe, 0xff, 0xe0, 0x3f, 0x0f, 0xc0, 0x7f, 0xf7, 0xd0, 
	0xbd, 0xff, 0xe0, 0x7f, 0x6f, 0xe0, 0x7f, 0xfb, 0xd0, 0xdd, 0xff, 0xe0, 0x3f, 0x1f, 0xc0, 0x7f, 
	0xfb, 0xb0, 0xeb, 0xff, 0xfe, 0x1f, 0x4f, 0x87, 0xff, 0xfd, 0x70, 0xff, 0xff, 0xff, 0x8f, 0x6f, 
	0x1f, 0xff, 0xff, 0xf0, 0xff, 0xff, 0xff, 0xcf, 0xff, 0x3f, 0xff, 0xff, 0xf0, 0xff, 0xff, 0xff, 
	0xc7, 0xfe, 0x3f, 0xff, 0xff, 0xf0, 0xff, 0xff, 0xff, 0xc7, 0x9e, 0x3f, 0xff, 0xff, 0xf0, 0xff, 
	0xff, 0xff, 0x8f, 0x6f, 0x1f, 0xff, 0xff, 0xf0, 0xff, 0xff, 0xff, 0x0f, 0x7f, 0x0f, 0xff, 0xff, 
	0xf0, 0xff, 0xff, 0xf8, 0x1f, 0x6f, 0x81, 0xff, 0xff, 0xf0, 0xfc, 0x00, 0x00, 0x7f, 0x2f, 0xe0, 
	0x00, 0x03, 0xf0, 0xfc, 0x00, 0x03, 0xff, 0xff, 0xfc, 0x00, 0x03, 0xf0
};


Commander cmd;
Adafruit_MCP4725 dac;
Adafruit_INA219 ina219;
Generic_LM75_12Bit temperature(&Wire);
Adafruit_NeoPixel RGB(SK6812_NUM, PIN_SK6812_DATA, NEO_RGB + NEO_KHZ800);
Adafruit_SSD1306 display(SCREEN_WIDTH, SCREEN_HEIGHT, &Wire1, OLED_RESET);


//temporary variables used in PA_commands we can set or get
int myInt = 0;
float myFloat = 0.0;




bool LED_state = false;
bool TX_state = false;
bool RX_state = true;
bool LNA_state = false;
bool SINGLE_state = false;

///////////////////////////////////////////////////////////////////////////
bool VGG_state = false;
bool DRIVER_state = false;
uint16_t DAC_value;
float FAN_speed;
uint16_t FAN_rpm;
float TEMP_limit;
float currentTemperature;
unsigned long startMillisec;
uint8_t tickCount;
bool TEMP_alarm = false;
String deviceInfo = "RADARC Pi Transceiver PA BOARD USB Commander";
const int WRITTEN_SIGNATURE = 0xBEEFDEED;
bool flashWritten = false;
int flashSignature;
volatile uint16_t RPM_PulseCount;

typedef struct {
  boolean valid;
  int Signature;
  float MaxTemp;
  float VddMin;
  float VddMax;
  float IddMax;
  float powerFwdMax;
  float powerRevMax;
} Limits;

typedef struct {
  boolean valid;
  int signature;
  boolean TX_USBControl;
  boolean TX_SerialControl;
  boolean TX_PinControl;
  float Vgg; // default Vgg on level
  uint16_t relayDelayTx; // from TX command delay to turn on TX relay in ms
  uint16_t driverDelayTx; // from TX command delay to turn on PA driver stage in ms
  uint16_t VggDelayTx; // from TX command delay to turn on Vgg in ms
  uint16_t RXDelay; // from TX command delay to switch RX path in ms
  uint16_t LNADelay; // from TX command delay to switch in LNA in ms
  uint16_t PORTDelay; // from TX commandelay to switch Single/Dual port in ms
  uint16_t relayDelayRx;
  uint16_t driverDelayRx;
  uint16_t VggDelayRx;
  boolean LNAState;
  boolean SINGLEPort;
  boolean AutoFAN;
  boolean FANTempOn;
  boolean FANTempOff;
 } Settings;

/*
 * Flash storage variables
 */
FlashStorage(mySignature, int); // flash storage of written signature
FlashStorage(myLimits, Limits); // flash storage of protection limits
FlashStorage(mySettings, Settings); // flash storage of operational settings

void Fan_Init(void)
{
   REG_GCLK_GENDIV = GCLK_GENDIV_DIV(1) |          // Divide the 48MHz clock source by divisor N=1: 48MHz/1=48MHz
                    GCLK_GENDIV_ID(4);            // Select Generic Clock (GCLK) 4
  while (GCLK->STATUS.bit.SYNCBUSY);              // Wait for synchronization

  REG_GCLK_GENCTRL = GCLK_GENCTRL_IDC |           // Set the duty cycle to 50/50 HIGH/LOW
                     GCLK_GENCTRL_GENEN |         // Enable GCLK4
                     GCLK_GENCTRL_SRC_DFLL48M |   // Set the 48MHz clock source
                     GCLK_GENCTRL_ID(4);          // Select GCLK4
  while (GCLK->STATUS.bit.SYNCBUSY);              // Wait for synchronization

  // Enable the port multiplexer for the digital pin D14 **** g_APinDescription() converts Arduino Pin to SAMD21 pin
  PORT->Group[g_APinDescription[PIN_FAN_PWM].ulPort].PINCFG[g_APinDescription[PIN_FAN_PWM].ulPin].bit.PMUXEN = 1;
 
  
  // Connect the TCC0 timer to digital output D14 - port pins are paired odd PMUO and even PMUXE
 

  PORT->Group[g_APinDescription[PIN_FAN_PWM].ulPort].PMUX[g_APinDescription[PIN_FAN_PWM].ulPin >> 1].reg = PORT_PMUX_PMUXE_F; // D1 is on PA18 = even, use device F on TCC0/WO[2]

  // Feed GCLK4 to TCC0 and TCC1
  REG_GCLK_CLKCTRL = GCLK_CLKCTRL_CLKEN |         // Enable GCLK4 to TCC0 and TCC1
                     GCLK_CLKCTRL_GEN_GCLK4 |     // Select GCLK4
                     GCLK_CLKCTRL_ID_TCC0_TCC1;   // Feed GCLK4 to TCC0 and TCC1
  while (GCLK->STATUS.bit.SYNCBUSY);              // Wait for synchronization

  // Dual slope PWM mode
  REG_TCC0_WAVE |= TCC_WAVE_POL(0xF) |           // Reverse the output polarity on all TCC0 outputs
                    TCC_WAVE_WAVEGEN_DSBOTTOM;   // Setup dual slope PWM on TCC0
  while (TCC0->SYNCBUSY.bit.WAVE);               // Wait for synchronization

  // Timer counts up to a maximum or TOP value set by the PER register,
  // this determines the frequency of the PWM operation: Freq = 48Mhz/(PER * 2)
    REG_TCC0_PER = 960;                           // Set the FreqTcc of the PWM on TCC1 to 24Khz
  while (TCC0->SYNCBUSY.bit.PER);                 // Wait for synchronization
 
  // Set the PWM signal to output , PWM ds = 2*N(TOP-CCx)/Freqtcc => PWM=0 => CCx=PER, PWM=50% => CCx = PER/2
  REG_TCC0_CCB2 = 0;                             // TCC0 CCB0 - on D14  50%
  while (TCC0->SYNCBUSY.bit.CCB2);                // Wait for synchronization
  
 
  // Divide the GCLOCK signal by 1 giving  in this case 48MHz (20.83ns) TCC1 timer tick and enable the outputs
  REG_TCC0_CTRLA |= TCC_CTRLA_PRESCALER_DIV1 |    // Divide GCLK4 by 1
                    TCC_CTRLA_ENABLE;             // Enable the TCC0 output
  while (TCC0->SYNCBUSY.bit.ENABLE);              // Wait for synchronization

}

void Fan_PWM(float percent)
{
  REG_TCC0_CCB2 =  (uint32_t) (percent * 9.6);                   // TCC0 CCB2 - on PIN_FAN_PWM- PWM signalling
  while (TCC0->SYNCBUSY.bit.CCB2);                // Wait for synchronization
}

void rpm_update(void){
  FAN_rpm = RPM_PulseCount * 30; // 2 pulses per rev
  RPM_PulseCount = 0;
}



void tempAlarm_update(void){

  currentTemperature = temperature.readTemperatureC();
  if(currentTemperature > TEMP_limit) TEMP_alarm = true;
  else TEMP_alarm = false;

}

void oled_update(void){
 
// Clear the buffer
  display.clearDisplay();
  display.setTextSize(1); 
  display.setTextColor(SSD1306_WHITE);
  display.setCursor(0, 0);
  display.println("RADARC Pi Transceiver");
  display.setCursor(42, 9);
  display.println("PA Board");
  display.setCursor(0,18);
  display.print("Status: ");
  if(TX_state == true) display.println("Transmit");
  else display.println("Receive");
  display.setCursor(0,27);
  display.print("Temp:");
  display.print(temperature.readTemperatureC(),1);
  display.println(" deg C");
  display.setCursor(0,36);
  display.print("Vdd:");
  display.print(ina219.getBusVoltage_V(),2 );
  display.print("V ");
  display.print("Vgg:");
  display.print( analogRead(2) * ( (5.7 * 3.3) / 4095.0) , 2 );
  display.println("V");
  display.setCursor(0,45);
  display.print("Idd:");
  float Idd_value =  ina219.getShuntVoltage_mV() /1;
  if (Idd_value < 0) Idd_value = -Idd_value;
  display.print(Idd_value,2 );
  display.print("A  RPM: ");
  display.println(FAN_rpm);
  display.setCursor(0,54);
  display.print("Fwd:");
  display.print(analogRead(0) * 3.3 / 4095.0,1 );
  display.print("W ");
  display.print("Rev: ");
  display.print( analogRead(1) * 3.3 / 4095.0 , 1 );
  display.println("W");
  display.display();


}

void RPM_Pulse(void){
  RPM_PulseCount++; 
}



//SETUP ---------------------------------------------------------------------------
void setup() {


 

/*
 * Check if flash EEPROM is initialised
 */
  mySignature.read(flashSignature);
  if(flashSignature == WRITTEN_SIGNATURE) flashWritten = true;
  

  Serial.begin(115200);
/*
 * Watchdog LED is set up and off
 */
  pinMode(LED_BUILTIN, OUTPUT);
  digitalWrite(LED_BUILTIN, LOW);
/*
 * RGB LED are setup and blue and green
 */ 
  RGB.begin();
  RGB.setBrightness(10);
  RGB.show();

  RGB.setPixelColor (0, RGB.Color(0,0,255));
  RGB.setPixelColor (1, RGB.Color(255,0,0));
  RGB.show();

/*
 * VGG is setup and disabled
 */
  pinMode(PIN_EN_VGG, OUTPUT);
  digitalWrite(PIN_EN_VGG, LOW);
/*
 * PA Drivers are setup and disabled
 */
  pinMode(PIN_DRIVER_EN, OUTPUT);
  digitalWrite(PIN_DRIVER_EN, LOW);
/*
 * Tx Relay drive is setup and disabled 
 */
  pinMode(PIN_TX_RELAY, OUTPUT);
  digitalWrite(PIN_TX_RELAY, LOW);

/*
 * Fan PWM is setup and fan set off
 */
  Fan_Init(); //initialise PWM for Fan to 25kHz
/*
 * FAN RPM interrupt Pin is setup and attached
 */
  pinMode(PIN_FAN_RPM, INPUT);
  attachInterrupt(digitalPinToInterrupt(PIN_FAN_RPM), RPM_Pulse, FALLING);


/*
 * Temperature sensor interrupt Pin is setup
 */
  pinMode(PIN_LM75_INT, INPUT);
/*
 * Vgg DAC is setup and set to Maximum value
 * Maximum is 0 volts out from DAC which prevents the DAC driving the voltage on the Vgg output
 * The voltage must be reset before Vgg is enable, elso Vgg will be too high. Actual Vgg voltage in inverse of DAC output
 */
  dac.begin(0x60);
  dac.setVoltage(VGG_MINIMUM, false);
  DAC_value = VGG_MINIMUM;
/*
 *INA219 Vdd Voltage and Idd Current sensor is setup
 */
  ina219.begin();
 /*
 * ADC resolution for Vgg and power measurement is setup
 */ 
  analogReadResolution(ADC_RESOLUTION);  // set ADC resolution to 12 bits

/*
 * Initialise OLED and display OLED startup screen
 */
if(!display.begin(SSD1306_SWITCHCAPVCC, SCREEN_ADDRESS)) {
    Serial.println(F("SSD1306 allocation failed"));
    for(;;); // Don't proceed, loop forever
  }

  display.clearDisplay();

  display.setTextSize(1); 
  display.setTextColor(SSD1306_WHITE);
  display.setCursor(0, 0);
  display.println("RADARC Pi Transceiver");
  display.setCursor(42, 9);
  display.println("PA Board");
  display.drawBitmap(
    (display.width()  - RADARC_LOGO_WIDTH ) / 2, 19,
    RADARC68x44, RADARC_LOGO_WIDTH, RADARC_LOGO_HEIGHT, 1);    
  display.display();
  delay(4000); // Pause for 4 seconds

/*
 * Initialise the Command interpreter
 */
  initialiseCommander();

/*
 *
 */
  while (!Serial); // for the USB serial to start so we see the startup prompt 
/*
 * Print startup message and off we go
 */
  cmd.printUserString();
  cmd.println();
  Serial.println("Type 'help' to get help");
  cmd.printCommandPrompt();

 /*
  * Start the millisecond timer
  */

  startMillisec = millis();
}

//MAIN LOOP ---------------------------------------------------------------------------
void loop() {
  cmd.update();
  if(millis() - startMillisec > 100 ){  // update temperature and OLED display every 100ms
    tempAlarm_update(); 
    oled_update();
    tickCount++;
    if(tickCount >= 10){
      rpm_update(); // update the RPM every second
      tickCount = 0;
    }
  }
  
  
}
