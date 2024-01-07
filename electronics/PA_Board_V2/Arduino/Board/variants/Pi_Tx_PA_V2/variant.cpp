/*
  Copyright (c) 2014-2015 Arduino LLC.  All right reserved.

  This library is free software; you can redistribute it and/or
  modify it under the terms of the GNU Lesser General Public
  License as published by the Free Software Foundation; either
  version 2.1 of the License, or (at your option) any later version.

  This library is distributed in the hope that it will be useful,
  but WITHOUT ANY WARRANTY; without even the implied warranty of
  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
  See the GNU Lesser General Public License for more details.

  You should have received a copy of the GNU Lesser General Public
  License along with this library; if not, write to the Free Software
  Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA
*/

#include "variant.h"

/*
 * Pins descriptions
 *---------------------------------------
 * Note these are not standard Arduino pin allocations
 *---------------------------------------
 */
const PinDescription g_APinDescription[]=
{
  // 0 - SK6812 LED Data
  // ----------------------
  { PORTA, 0, PIO_DIGITAL, PIN_ATTR_DIGITAL, No_ADC_Channel, NOT_ON_PWM, NOT_ON_TIMER, EXTERNAL_INT_NONE }, // SK6812_DATA

 // 1..3 - Analog pins
  // --------------------
  { PORTA,  3, PIO_ANALOG, 0, ADC_Channel1, NOT_ON_PWM, NOT_ON_TIMER, EXTERNAL_INT_NONE }, // ADC/AIN[1] ADC_FWD
  { PORTA,  4, PIO_ANALOG, 0, ADC_Channel4, NOT_ON_PWM, NOT_ON_TIMER, EXTERNAL_INT_NONE }, // ADC/AIN[4] ADC_Rev
  { PORTA,  5, PIO_ANALOG, 0, ADC_Channel5, NOT_ON_PWM, NOT_ON_TIMER, EXTERNAL_INT_NONE }, // ADC/AIN[5] ADC_VGG
  
 // 4..5 - Digital
 // ----------------------
  { PORTA,  6, PIO_DIGITAL, PIN_ATTR_DIGITAL, No_ADC_Channel, NOT_ON_PWM, NOT_ON_TIMER, EXTERNAL_INT_NONE }, // TX RELAY
  { PORTA,  7, PIO_DIGITAL, PIN_ATTR_DIGITAL, No_ADC_Channel, NOT_ON_PWM, NOT_ON_TIMER, EXTERNAL_INT_NONE }, // DRIVER_EN

 // 6..7 Pi Serial Interface Pins to Pi
  { PORTA, 8, PIO_SERCOM, PIN_ATTR_DIGITAL, No_ADC_Channel, NOT_ON_PWM, NOT_ON_TIMER, EXTERNAL_INT_NONE }, // PI_TXD: SERCOM0/PAD[0]
  { PORTA, 9, PIO_SERCOM, PIN_ATTR_DIGITAL, No_ADC_Channel, NOT_ON_PWM, NOT_ON_TIMER, EXTERNAL_INT_NONE }, // PI_RXD: SERCOM0/PAD[1]

 // 8..11 Digital
  { PORTA, 10, PIO_DIGITAL, PIN_ATTR_DIGITAL, No_ADC_Channel, NOT_ON_PWM, NOT_ON_TIMER, EXTERNAL_INT_NONE }, // WATCHDOG_LED
  { PORTA, 11, PIO_DIGITAL, PIN_ATTR_DIGITAL, No_ADC_Channel, NOT_ON_PWM, NOT_ON_TIMER, EXTERNAL_INT_NONE }, // PI_TXEN
  { PORTA, 14, PIO_DIGITAL, PIN_ATTR_DIGITAL, No_ADC_Channel, NOT_ON_PWM, NOT_ON_TIMER, EXTERNAL_INT_14 }, // FAN_RPM
  { PORTA, 15, PIO_DIGITAL, PIN_ATTR_DIGITAL, No_ADC_Channel, NOT_ON_PWM, NOT_ON_TIMER, EXTERNAL_INT_NONE }, // EN VGG_PA

 // 12..13 External I2C pins (SDA/SCL)  
  { PORTA, 16, PIO_SERCOM, PIN_ATTR_DIGITAL, No_ADC_Channel, NOT_ON_PWM, NOT_ON_TIMER, EXTERNAL_INT_NONE }, // EXT_SDA: SERCOM1/PAD[0]
  { PORTA, 17, PIO_SERCOM, PIN_ATTR_DIGITAL, No_ADC_Channel, NOT_ON_PWM, NOT_ON_TIMER, EXTERNAL_INT_NONE }, // EXT_SCL: SERCOM1/PAD[1]
  
 // 14..15 Digital
  { PORTA, 18, PIO_DIGITAL, PIN_ATTR_DIGITAL, No_ADC_Channel, NOT_ON_PWM, NOT_ON_TIMER, EXTERNAL_INT_NONE }, // FAN_PWM
  { PORTA, 19, PIO_DIGITAL, PIN_ATTR_DIGITAL, No_ADC_Channel, NOT_ON_PWM, NOT_ON_TIMER, EXTERNAL_INT_3 }, // LM75_INT


 // 16..17 Internal I2C pins (SDA/SCL)  
  { PORTA, 22, PIO_SERCOM, PIN_ATTR_DIGITAL, No_ADC_Channel, NOT_ON_PWM, NOT_ON_TIMER, EXTERNAL_INT_NONE }, // INT_SDA: SERCOM3/PAD[0]
  { PORTA, 23, PIO_SERCOM, PIN_ATTR_DIGITAL, No_ADC_Channel, NOT_ON_PWM, NOT_ON_TIMER, EXTERNAL_INT_NONE }, // INT_SCL: SERCOM3/PAD[1]


 // 18..20 - USB
  // --------------------
  { PORTA, 24, PIO_COM, PIN_ATTR_NONE, No_ADC_Channel, NOT_ON_PWM, NOT_ON_TIMER, EXTERNAL_INT_NONE }, // USB/DM
  { PORTA, 25, PIO_COM, PIN_ATTR_NONE, No_ADC_Channel, NOT_ON_PWM, NOT_ON_TIMER, EXTERNAL_INT_NONE }, // USB/DP
  { PORTA, 28, PIO_COM, PIN_ATTR_NONE, No_ADC_Channel, NOT_ON_PWM, NOT_ON_TIMER, EXTERNAL_INT_NONE }, // USB Host enable
 
} ;



// Multi-serial objects instantiation
SERCOM sercom0( SERCOM0 ) ;
SERCOM sercom1( SERCOM1 ) ;
SERCOM sercom3( SERCOM3 ) ;


Uart Serial1( &sercom0, PIN_SERIAL1_RX, PIN_SERIAL1_TX, PAD_SERIAL1_RX, PAD_SERIAL1_TX ) ;

void SERCOM0_Handler()
{
  Serial1.IrqHandler();
}



