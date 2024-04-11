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
#define PINS_COUNT           (21u)
#define NUM_DIGITAL_PINS     (15u)
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
#define WATCHDOG_LED         PIN_LED

//  RGBW addressable LEDs
#define PIN_SK6812_DATA      (0u)
#define SK6812_NUM           (02u)

/*
 * Analog Pins
 */
#define PIN_A0               (1ul)
#define PIN_A1               (PIN_A0 + 1)
#define PIN_A2               (PIN_A0 + 2)
#define PIN_DAC0             (-1ul) // DAC not used
#define PIN_ADC_FWD          PIN_A0
#define PIN_ADC_REV          PIN_A1
#define PIN_ADC_VGG          PIN_A2

static const uint8_t A0  = PIN_A0;
static const uint8_t A1  = PIN_A1;
static const uint8_t A2  = PIN_A2;


#define ADC_RESOLUTION		12

/*
 * Digital Pins
 */
#define PIN_TX_RELAY         (4u)
#define PIN_DRIVER_EN        (5u)
#define PIN_PI_TX_EN         (9u)
#define PIN_FAN_RPM          (10u)
#define PIN_EN_VGG           (11u)
#define PIN_FAN_PWM          (14u)
#define PIN_LM75_INT         (15u)

/*
 * Serial interfaces
 */

// Serial1 (sercom 0)
#define PIN_SERIAL1_TX       (6ul) // PA08
#define PAD_SERIAL1_TX       (UART_TX_PAD_0)
#define PIN_SERIAL1_RX       (7ul) // PA09
#define PAD_SERIAL1_RX       (SERCOM_RX_PAD_1)

/*
 * SPI Interfaces
 */
#define SPI_INTERFACES_COUNT 1 // We don't use an SPI interface, but some libraries Adafruit OLED appear to need one defined

#define PIN_SPI_MISO         (-1)  // setup with dummy pins
#define PIN_SPI_MOSI         (-1)
#define PIN_SPI_SCK          (-1)
#define PERIPH_SPI           sercom0
#define PAD_SPI_TX           SPI_PAD_2_SCK_3
#define PAD_SPI_RX           SERCOM_RX_PAD_1


/*
 * Wire Interfaces
 */
#define WIRE_INTERFACES_COUNT 2

/*
 * The internal I2C Bus
 */
#define PIN_WIRE_SDA         (16u)
#define PIN_WIRE_SCL         (17u)
#define PERIPH_WIRE          sercom3
#define WIRE_IT_HANDLER      SERCOM3_Handler
static const uint8_t SDA = PIN_WIRE_SDA;
static const uint8_t SCL = PIN_WIRE_SCL;


/*
 * The external I2C Bus
 */
#define PIN_WIRE1_SDA         (12u)
#define PIN_WIRE1_SCL         (13u)
#define PERIPH_WIRE1          sercom1
#define WIRE1_IT_HANDLER      SERCOM1_Handler
static const uint8_t SDA1 = PIN_WIRE1_SDA;
static const uint8_t SCL1 = PIN_WIRE1_SCL;

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
 * SERCOM0 is Serial
 * SERCOM1 is Wire
 * SERCOM3 is Wire1
 * 
*/

extern SERCOM sercom0;
extern SERCOM sercom1;
extern SERCOM sercom3;

extern Uart Serial1;


#endif


//
// SERIAL_PORT_USBVIRTUAL     Port which is USB virtual serial
//

#define SERIAL_PORT_USBVIRTUAL      Serial
#define SERIAL_PORT_MONITOR         Serial

// Serial has no physical pins broken out, so it's not listed as HARDWARE port
#define SERIAL_PORT_HARDWARE        Serial1
#define SERIAL_PORT_HARDWARE_OPEN   Serial1

#endif /* _VARIANT_Pi_Tx_PA_ */

