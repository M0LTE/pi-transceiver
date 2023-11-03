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

#ifndef _VARIANT_Pi_Tx_PA_
#define _VARIANT_Pi_Tx_PA_

// The definitions here needs a SAMD core >=1.6.10
#define ARDUINO_SAMD_VARIANT_COMPLIANCE 10610

/*----------------------------------------------------------------------------
 *        Definitions
 *----------------------------------------------------------------------------*/

/** Frequency of the board main oscillator */
#define VARIANT_MAINOSC		(32768ul)

/** Master clock frequency */
#define VARIANT_MCK	(F_CPU)

/*----------------------------------------------------------------------------
 *        Headers
 *----------------------------------------------------------------------------*/

#include "WVariant.h"
#ifdef __cplusplus
#include "SERCOM.h"
#include "Uart.h"
#endif // __cplusplus

#ifdef __cplusplus
extern "C"
{
#endif // __cplusplus

/*----------------------------------------------------------------------------
 *        Pins
 *----------------------------------------------------------------------------*/

// Number of pins defined in PinDescription array
#define PINS_COUNT           (19u)
#define NUM_DIGITAL_PINS     (13u)
#define NUM_ANALOG_INPUTS    (3u)
#define NUM_ANALOG_OUTPUTS   (0u)
#define analogInputToDigitalPin(p)  ((p < 3u) ? (p) + PIN_A0 : -1)

#define digitalPinToPort(P)        ( &(PORT->Group[g_APinDescription[P].ulPort]) )
#define digitalPinToBitMask(P)     ( 1 << g_APinDescription[P].ulPin )
#define portOutputRegister(port)   ( &(port->OUT.reg) )
#define portInputRegister(port)    ( &(port->IN.reg) )
#define portModeRegister(port)     ( &(port->DIR.reg) )
#define digitalPinHasPWM(P)        ( g_APinDescription[P].ulPWMChannel != NOT_ON_PWM || g_APinDescription[P].ulTCChannel != NOT_ON_TIMER )



// LEDs
#define PIN_LED              (8u)
#define LED_BUILTIN          PIN_LED
#define LED_WATCHDOG         PIN_LED

// DotStar LED
#define PIN_DOTSTAR_DATA     (1u)
#define PIN_DOTSTAR_CLK      (0u)
#define DOTSTAR_NUM          (1u)

/*
 * Analog pins
 */
#define PIN_A0               (2ul)
#define PIN_A1               (PIN_A0 + 1)
#define PIN_A2               (PIN_A0 + 2)
#define PIN_DAC0             -1 // DAC not used

static const uint8_t A0  = PIN_A0;
static const uint8_t A1  = PIN_A1;
static const uint8_t A2  = PIN_A2;


#define ADC_RESOLUTION		12

/*
 * SPI Interfaces
 */
#define SPI_INTERFACES_COUNT 1 // We don't use an SPI interface, but some libraries (Dotstar) appear to need one defined

#define PIN_SPI_MISO         (-1)  // setup with dummy pins
#define PIN_SPI_MOSI         (-1)
#define PIN_SPI_SCK          (-1)
#define PERIPH_SPI           sercom0
#define PAD_SPI_TX           SPI_PAD_2_SCK_3
#define PAD_SPI_RX           SERCOM_RX_PAD_1

/*
 * Wire Interfaces
 */
#define WIRE_INTERFACES_COUNT 3

#define PIN_WIRE_SDA         (6u)
#define PIN_WIRE_SCL         (7u)
#define PERIPH_WIRE          sercom0
#define WIRE_IT_HANDLER      SERCOM0_Handler

static const uint8_t SDA = PIN_WIRE_SDA;
static const uint8_t SCL = PIN_WIRE_SCL;

#define PIN_WIRE1_SDA         (11u)
#define PIN_WIRE1_SCL         (12u)
#define PERIPH_WIRE1          sercom1
#define WIRE1_IT_HANDLER      SERCOM1_Handler

static const uint8_t SDA1 = PIN_WIRE1_SDA;
static const uint8_t SCL1 = PIN_WIRE1_SCL;

#define PIN_WIRE2_SDA         (14u)
#define PIN_WIRE2_SCL         (15u)
#define PERIPH_WIRE2          sercom3
#define WIRE2_IT_HANDLER      SERCOM3_Handler

static const uint8_t SDA2 = PIN_WIRE2_SDA;
static const uint8_t SCL2 = PIN_WIRE2_SCL;

/*
 * USB
 */
#define PIN_USB_HOST_ENABLE (33ul)
#define PIN_USB_DM          (34ul)
#define PIN_USB_DP          (35ul)

#ifdef __cplusplus
}
#endif

/*----------------------------------------------------------------------------
 *        Arduino objects - C++ only
 *----------------------------------------------------------------------------*/

#ifdef __cplusplus

/*	=========================
 *	===== SERCOM DEFINITION
 *	=========================
 * The ATSAMD21E18 only has four SERCOM Ports and we ony use 3 due to Pin limitations
 * SERCOM0 is Wire
 * SERCOM1 is Wire1
 * SERCOM3 is Wire2
 * 
*/

extern SERCOM sercom0;
extern SERCOM sercom1;
extern SERCOM sercom3;




#endif


//
// SERIAL_PORT_USBVIRTUAL     Port which is USB virtual serial
//

#define SERIAL_PORT_USBVIRTUAL      Serial
#define SERIAL_PORT_MONITOR         Serial

#endif /* _VARIANT_Pi_Tx_PA_ */

