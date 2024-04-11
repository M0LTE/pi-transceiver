# Bootloader for Pi Transciever PA Board

The Bootloader for the Atmel ATSAMD21E18 processor is the same bootloader used on the Adafruit Trinket M0 which uses the same processor

The .bin file bootloader file is programmed into the ATSAMD21E18 using Atmel Studio and an Atmel ICE or similar SWD programmer. The connection to the debug pins on the PA Board is made using a 3d printed programming clip with pogo pins.

The Bootloader area should be locked after programming.
    After flashing, you'll need to set the BOOTPROT fuse back to a 8kB bootloader size.
    From Fuses, set BOOTPROT to 0x2 or 8KB and click Program


