# pi-transceiver / software / system
Here is a folder for scripts etc to build the system 

## Install notes 2022-02-09

Complete from-the-ground-up build of a working system.

It is highly recommended to copy-paste these commands into a remote connection rather than typing them in by hand on the Pi.

This will result in a system without a graphical desktop interface, which is the same as how the system will run once the radio software is installed.

### Prepare the micro SD card
1. Download latest Raspberry Pi Imager https://www.raspberrypi.com/software/
1. Choose OS -> Raspberry Pi OS (other) -> Raspberry Pi OS Lite (32-bit) - Bullseye 2022-01-28
1. Choose storage -> pick micro SD card, which you have inserted
1. Click the Cog icon
   1. Set hostname: e.g. pi-transceiver
   1. Enable SSH, use password auth
   1. Set username and password - pick whatever you want
   1. Configure wi-fi if desired
   1. Click save
1. Click Write, then click Yes, and wait

### First boot

1. Plug in a network cable if you didn't configure Wi-Fi
1. Plug in a screen - either the official touchscreen or an HDMI monitor
1. Insert the micro SD card
1. Power up the Pi
1. Wait until you see a login prompt
1. Connect to the Pi using your favourite SSH client. PuTTY is fine on Windows. Log in using the username and password you set earlier. The default username is "pi"
1. Issue the command `sudo apt update; sudo apt full-upgrade -y` and wait until completion

### Install GNU Radio and LimeSDR components

1. Install GNU Radio by issuing the command:

```
sudo apt install gnuradio -y
```

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
