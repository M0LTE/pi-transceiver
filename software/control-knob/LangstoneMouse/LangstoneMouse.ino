// Rotary Encoder and Button Mouse Emulation for Langstone Transceiver. By Colin Durbridge G4EML
// for Arduino Pro Micro Board. (5V 16MHz Version) (won't work with all Arduinos. Needs the ATmega32U4 chip for USB Mouse emulation.  
// Needs the Encoder Library by Paul Stoffregen. (Search for Encoder in Library Manager)
// Rotary encoder (used as tuning knob) is read and scaled to suitable output pulses per revolution then sent as mouse scroll wheel movement every 20 ms.
// 3 Buttons (used to select tuning rate and dial lock) are read and sent as mouse clicks on the left, middle and right mouse buttons. 
//
// Mod by M0ZSU, if flex flag is set, do not emulate mouse, send Flex like ASCII on the serial port instead.
// TODO :Implement buttons, add compile flags so it can compile as Flex only with non ATmega32U4 boards as Flex emulation only needs serial port.

#include <Mouse.h>
#include <Encoder.h>
#include <USBAPI.h>

#define encoderStepsPerRev 400                   //number of steps per revolution of the encoder. Change this to match your encoder.  

#define outputStepsPerRev  40                    //nuber of steps per revolution for tuning. 40 is a good number to start with. Change this if you want to adjust the tuning rate. 

#define RPHA 3                        //Rotary encoder A phase connected to Pin 2. Encoder Common connection to ground.  (do not change, needs the interrupt on this pin to work properly)
#define RPHB 2                        //Rotary encoder B phase connected to Pin 3. Swap these two pins to reverse the encoder direction.                                    
#define LEFTBUTTON 4                //Left, right and middle push buttons. Used by Langstone to select the tuning rate and dial lock. 
#define RIGHTBUTTON 5               //Connect buttons between these three pins and ground. 
#define MIDDLEBUTTON 6

#define SERIAL_BAUD     9600 // FlexKnob 9600 by default
#define IS_FLEX 

int encoderDiv;
int leftButton;
int rightButton;
int middleButton;
int leftButtonReleased;
int rightButtonReleased;
int middleButtonReleased;

Encoder Enc(RPHA, RPHB);

bool flex = false;
bool start = true;
bool mousesupport = false;


void setup() 
{

#if defined(__AVR_ATmega32U4__) //Check if we have hardware for mouse emulation
  mousesupport = true;
  #warning "Mouse support detected";
#endif

#ifdef IS_FLEX //At the moment Flex emulation controlled by compile flag, later will change to checking digital input for physical switch
  flex = true;
#endif

  
 encoderDiv=(encoderStepsPerRev*4)/outputStepsPerRev;                     // calculate the requred encoder divisor. (Encoder library outputs 4 steps per input step)
 pinMode(LEFTBUTTON,INPUT_PULLUP);
 pinMode(RIGHTBUTTON,INPUT_PULLUP);
 pinMode(MIDDLEBUTTON,INPUT_PULLUP);
 leftButtonReleased=false;
 rightButtonReleased=false; 
 middleButtonReleased=false; 

 if (mousesupport == false) //if there is no mouse hardware force Flex mode
 {
  flex=true;
 }

 if (flex)
  {
    Serial.begin(SERIAL_BAUD);
  }
 else
 {
    Mouse.begin();
 }

 
}

void loop() 
{
delay(20);     //delay to slow down the rate of USB messages. 50 per second is plenty. 

if (flex) 
{
 
  if (Serial.dtr() == false) // Send ID code once when port becomes active, currently this is done by checking DTR
  {
    start= true;
    while (Serial.dtr() == false)
      {
        ;
      }
      
  }

  if (start)
  {
    for (uint8_t i = 0; i < 1; i++) Serial.write("F0304;");
    start = false;
  }
      
}
   
long counts = Enc.read()/encoderDiv;    //number of encoder counts since last sent to USB.

if(counts!=0)
  {
    if (!flex)
    {
        Mouse.move(0,0,counts);
    }
      else
      {
        if (counts> 0)
        {
          for (uint8_t i = 0; i < counts; i++) Serial.write("U;");
        }
        else if (counts< 0)
         {
          for (long i = counts; i < 0; i++) Serial.write("D;");
        }
    
      }
  Enc.write(0);                         //reset the encoder counts
  }
  
  leftButton=digitalRead(LEFTBUTTON);
  rightButton=digitalRead(RIGHTBUTTON);
  middleButton=digitalRead(MIDDLEBUTTON);

  if(leftButton==LOW)
    {
     if(leftButtonReleased>=3)
      {
       Mouse.click(MOUSE_LEFT); 
       leftButtonReleased=false; 
      }
    }
  else
    {
      if(leftButtonReleased<3) leftButtonReleased=leftButtonReleased+1;
    }

    
  if(rightButton==LOW)
    {
     if(rightButtonReleased>=3)
      {
       Mouse.click(MOUSE_RIGHT);  
       rightButtonReleased=false;
      }
    }
  else
    {
      if(rightButtonReleased<3) rightButtonReleased=rightButtonReleased+1;
    }


  if(middleButton==LOW)
    {
     if(middleButtonReleased>=3)
      {
       Mouse.click(MOUSE_MIDDLE);  
       middleButtonReleased=false;
      }
    }
  else
    {
      if(middleButtonReleased<3) middleButtonReleased=middleButtonReleased+1;
    }

}
