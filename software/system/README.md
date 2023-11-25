GRM1555C1H3R3CZ01D
# pi-transceiver / software / system
Here is a folder for scripts etc to build the system 
The original description here was created by M0LTE for Raspberry Pi OS (Buster/Bullseye) with additional input from G4CDF. This updated version is due to the release of new versions of Raspberry Pi OS (Bookworm), GNU_Radio 3.10, and gr-limesdr.
Despite the new versions it is not possible to update to the latest Raspberry Pi OS (Bookworm). This is due to the limitation that the GNURadio plugin for GNURadio gr-limesdr is not supported in the latest version of GNURadio 3.10 and we must use GNURadio 3.8. GNURadio 3.8 is not supported under Bookworm so we must also use previous Raspberry Pi OS (Bullseye).

The install notes have two versions, one for the headless build for the pi-transceiver, and the second for a developer build with GUI

## Install notes Headless Version 2023-11-24

Complete from-the-ground-up build of a working headless system. Based on Raspberry Pi 4 Model B. 

It is highly recommended to copy-paste these commands into a remote connection rather than typing them in by hand on the Pi.

This will result in a system without a graphical desktop interface, which is the same as how the system will run once the radio software is installed.

### Prepare the micro SD card
1. Download latest Raspberry Pi Imager https://www.raspberrypi.com/software/
1. Choose Device -> Raspberry Pi 4
1. Choose OS -> Raspberry Pi OS (other) -> Raspberry Pi OS (other) -> Raspberry Pi OS (Legacy) Lite

   Insert SD Card in PC. 32MB recommended

1. Choose storage -> pick micro SD card, which you have inserted
1. Click NEXT
1. Click EDIT SETTINGS
1. Under GENERAL tab
1. Set hostname: e.g. pi-transceiver
1. Set username and password - pick whatever you want
1. Set locale
1. Configure wi-fi if desired
1. Under SERVICES tab
1. Enable SSH
1. Click SAVE
1. Click YES to apply customisation settings
1. Click YES to continue at warning and wait.....

   SD card will be written and verified. remove when "Write Successful" message appears.


### First boot

1. Plug in a network cable if you didn't configure Wi-Fi
1. Plug in a screen - either the official touchscreen or an HDMI monitor
1. Insert the micro SD card
1. Power up the Pi
1. The Raspberry Pi will reboot several times including a login a couple of login prompts if WLAN enabled.
1. Wait until you see a stable login prompt
1. Connect to the Pi using your favourite SSH client. PuTTY is fine on Windows. Log in using the username and password you set earlier. The default username is "pi"
1. Issue the command
```
sudo apt update; sudo apt full-upgrade -y
``` 
and wait until completion



### Install GNU Radio and LimeSDR components

There is no GNU Radio PPA for Raspberry Pi OS either for Buster or Bullseye as it stands.

1.Install GNU Radio by issuing the command:
```
sudo apt install gnuradio -y
```
When installation is complete, issue the command 
```
gnuradio-config-info -v
```
   Which should show version 3.8 of GNURadio has been successfully installed.

2. Install LimeSuite by issuing the commands:
```
sudo apt install -y git g++ cmake libsqlite3-dev libsoapysdr-dev libi2c-dev libusb-1.0-0-dev
cd ~
git clone https://github.com/myriadrf/LimeSuite.git
cd LimeSuite
git checkout stable
mkdir builddir && cd builddir
cmake ../
make -j4
sudo make install
sudo ldconfig
cd ~/LimeSuite/udev-rules
sudo ./install.sh
```
If the Lime SDR Mini is plugged into the  raspberry Pi then issue the command:
```
LimeUtil -find
```
This should detect the Lime SDR and give the serial of the device.

3. Install the LimeSDR plugin for GNU Radio by issuing the commands:

```
cd ~
git clone https://github.com/myriadrf/gr-limesdr.git
cd gr-limesdr
git checkout gr-3.8
mkdir build
cd build
cmake ..
make
sudo make install
sudo ldconfig
```

### Install .NET 6

1. Issue the command

```
wget -O - https://raw.githubusercontent.com/pjgpetecodes/dotnet6pi/master/install.sh | sudo bash
```
2. Reboot by issuing the command `sudo reboot`
3. Connect back up to the Pi using SSH once it has rebooted

*** THIS GOES WRONG HERE FOR ME ***
The end of the install does a test to "Run dotnet -- info" and it returns "No such file or directory"

### Enable I2C

I2C is not enabled by default. Enable it by running `sudo raspi-config` on the Pi, then selecting Interface Options, I2C, Enable. No need to reboot.

### Run the rig controller software

To fetch a working copy of the software:

```
cd ~
git clone https://github.com/M0LTE/pi-transceiver.git
```

To update the software:

```
cd ~/pi-transceiver
git pull
```

To run the software in the foreground (NB there will be no touchscreen UI since the screen will be used for console output - useful for debugging):

```
cd ~/pi-transceiver/software/rig-controller/rig-controller/
dotnet run
```

Then on a second computer on the same network, open http://pi-transceiver:5155 in a web browser. You'll get the radio UI.

G4CDF>  Windows 10 machine cannt see the Raspberry PI for some reason.

NOTE: as above - in the final product, this radio UI will be on the touchscreen.

Instructions on setting up the software to run in the background and display the UI on the touchscreen (or HDMI monitor) will follow.

## Hardware

- When connecting the official touchscreen, the only connections should be VCC, ground, and the ribbon cable. Nothing more. Else you are bridging two I2C buses which will have unpredictable effects.

## Install notes GUI Version 2023-11-25

Complete from-the-ground-up build of a working GUI development system. Based on Raspberry Pi 4 Model B. 


This will result in a system with a graphical desktop interface. 

### Prepare the micro SD card
1. Download latest Raspberry Pi Imager https://www.raspberrypi.com/software/
1. Choose Device -> Raspberry Pi 4
1. Choose OS -> Raspberry Pi OS (other) -> Raspberry Pi OS (other) -> Raspberry Pi OS (Legacy)

   Insert SD Card in PC. 32MB recommended

1. Choose storage -> pick micro SD card, which you have inserted
1. Click NEXT
1. Click EDIT SETTINGS
1. Under GENERAL tab
1. Set hostname: e.g. pi-transceiver
1. Set username and password - pick whatever you want
1. Set locale
1. Configure wi-fi if desired
1. Under SERVICES tab
1. Enable SSH
1. Click SAVE
1. Click YES to apply customisation settings
1. Click YES to continue at warning and wait.....

   SD card will be written and verified. remove when "Write Successful" message appears.


### First boot

1. Plug in a network cable if you didn't configure Wi-Fi
1. Plug in a screen - either the official touchscreen or an HDMI monitor
1. Plug in a USB keyboard and USB mouse
1. Insert the micro SD card
1. Power up the Pi
1. The Raspberry Pi will boot into the OS user interface
1. After a few minutes the user interface will indicate there are updates available. Accept and install these updates



### Install GNU Radio and LimeSDR components

1. Go to Preferences-> Add / Remove Software
1. Search gnuradio.
1. Install "GNU Radio Software Radio Toolkit".
1. Install "LimeSDR Blocks for GnuRADIO"
1. Search limesuite
1. Install "tools to test, control and update LMS7 transceiver based boards"
1. Reboot

The GNU Radio Companion and the LimeSuite Gui should now be visible under the programming tab. Limesuite should detect a connected Lime SDR. GNU Radio Companion should include the LimeSDR blocks at the bottom of the block selector.


Not sure where to go from here with the Dotnet and pi-transceiver code! Over to M0LTE
