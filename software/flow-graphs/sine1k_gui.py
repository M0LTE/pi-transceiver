#!/usr/bin/env python3
# -*- coding: utf-8 -*-

#
# SPDX-License-Identifier: GPL-3.0
#
# GNU Radio Python Flow Graph
# Title: Not titled yet
# GNU Radio version: 3.8.1.0

from distutils.version import StrictVersion

if __name__ == '__main__':
    import ctypes
    import sys
    if sys.platform.startswith('linux'):
        try:
            x11 = ctypes.cdll.LoadLibrary('libX11.so')
            x11.XInitThreads()
        except:
            print("Warning: failed to XInitThreads()")

from PyQt5 import Qt
from gnuradio import analog
from gnuradio import blocks
from gnuradio import filter
from gnuradio import gr
from gnuradio.filter import firdes
import sys
import signal
from argparse import ArgumentParser
from gnuradio.eng_arg import eng_float, intx
from gnuradio import eng_notation
from gnuradio.qtgui import Range, RangeWidget
import limesdr
from gnuradio import qtgui

class sine1k_gui(gr.top_block, Qt.QWidget):

    def __init__(self):
        gr.top_block.__init__(self, "Not titled yet")
        Qt.QWidget.__init__(self)
        self.setWindowTitle("Not titled yet")
        qtgui.util.check_set_qss()
        try:
            self.setWindowIcon(Qt.QIcon.fromTheme('gnuradio-grc'))
        except:
            pass
        self.top_scroll_layout = Qt.QVBoxLayout()
        self.setLayout(self.top_scroll_layout)
        self.top_scroll = Qt.QScrollArea()
        self.top_scroll.setFrameStyle(Qt.QFrame.NoFrame)
        self.top_scroll_layout.addWidget(self.top_scroll)
        self.top_scroll.setWidgetResizable(True)
        self.top_widget = Qt.QWidget()
        self.top_scroll.setWidget(self.top_widget)
        self.top_layout = Qt.QVBoxLayout(self.top_widget)
        self.top_grid_layout = Qt.QGridLayout()
        self.top_layout.addLayout(self.top_grid_layout)

        self.settings = Qt.QSettings("GNU Radio", "sine1k_gui")

        try:
            if StrictVersion(Qt.qVersion()) < StrictVersion("5.0.0"):
                self.restoreGeometry(self.settings.value("geometry").toByteArray())
            else:
                self.restoreGeometry(self.settings.value("geometry"))
        except:
            pass

        ##################################################
        # Variables
        ##################################################
        self.samp_rate = samp_rate = 32000
        self.nco = nco = 0
        self.mute = mute = False
        self.gain = gain = 14
        self.freq = freq = 144100000

        ##################################################
        # Blocks
        ##################################################
        _mute_check_box = Qt.QCheckBox('mute')
        self._mute_choices = {True: True, False: False}
        self._mute_choices_inv = dict((v,k) for k,v in self._mute_choices.items())
        self._mute_callback = lambda i: Qt.QMetaObject.invokeMethod(_mute_check_box, "setChecked", Qt.Q_ARG("bool", self._mute_choices_inv[i]))
        self._mute_callback(self.mute)
        _mute_check_box.stateChanged.connect(lambda i: self.set_mute(self._mute_choices[bool(i)]))
        self.top_grid_layout.addWidget(_mute_check_box)
        self._gain_range = Range(0, 73, 1, 14, 200)
        self._gain_win = RangeWidget(self._gain_range, self.set_gain, 'gain', "counter_slider", float)
        self.top_grid_layout.addWidget(self._gain_win)
        self._freq_range = Range(144000000, 144500000, 1000, 144100000, 200)
        self._freq_win = RangeWidget(self._freq_range, self.set_freq, 'freq', "counter_slider", float)
        self.top_grid_layout.addWidget(self._freq_win)
        self._nco_range = Range(-1000, 1000, 1, 0, 200)
        self._nco_win = RangeWidget(self._nco_range, self.set_nco, 'nco', "counter_slider", float)
        self.top_grid_layout.addWidget(self._nco_win)
        self.limesdr_sink_0 = limesdr.sink('1D5882F9FB64B6', 0, '', '')


        self.limesdr_sink_0.set_sample_rate(samp_rate)


        self.limesdr_sink_0.set_center_freq(freq, 0)

        self.limesdr_sink_0.set_bandwidth(5e6, 0)


        self.limesdr_sink_0.set_digital_filter(samp_rate, 0)


        self.limesdr_sink_0.set_gain(gain, 0)


        self.limesdr_sink_0.set_antenna(255, 0)


        self.limesdr_sink_0.calibrate(2.5e6, 0)
        self.dc_blocker_xx_0 = filter.dc_blocker_cc(32, True)
        self.blocks_mute_xx_0 = blocks.mute_cc(bool(mute))
        self.analog_sig_source_x_0 = analog.sig_source_c(samp_rate, analog.GR_COS_WAVE, 1500, 1, 0, 0)



        ##################################################
        # Connections
        ##################################################
        self.connect((self.analog_sig_source_x_0, 0), (self.dc_blocker_xx_0, 0))
        self.connect((self.blocks_mute_xx_0, 0), (self.limesdr_sink_0, 0))
        self.connect((self.dc_blocker_xx_0, 0), (self.blocks_mute_xx_0, 0))

    def closeEvent(self, event):
        self.settings = Qt.QSettings("GNU Radio", "sine1k_gui")
        self.settings.setValue("geometry", self.saveGeometry())
        event.accept()

    def get_samp_rate(self):
        return self.samp_rate

    def set_samp_rate(self, samp_rate):
        self.samp_rate = samp_rate
        self.analog_sig_source_x_0.set_sampling_freq(self.samp_rate)
        self.limesdr_sink_0.set_digital_filter(self.samp_rate, 0)
        self.limesdr_sink_0.set_digital_filter(self.samp_rate, 1)

    def get_nco(self):
        return self.nco

    def set_nco(self, nco):
        self.nco = nco
        self.limesdr_sink_0.set_nco(self.nco, 0)

    def get_mute(self):
        return self.mute

    def set_mute(self, mute):
        self.mute = mute
        self._mute_callback(self.mute)
        self.blocks_mute_xx_0.set_mute(bool(self.mute))

    def get_gain(self):
        return self.gain

    def set_gain(self, gain):
        self.gain = gain
        self.limesdr_sink_0.set_gain(self.gain, 0)

    def get_freq(self):
        return self.freq

    def set_freq(self, freq):
        self.freq = freq
        self.limesdr_sink_0.set_center_freq(self.freq, 0)



def main(top_block_cls=sine1k_gui, options=None):

    if StrictVersion("4.5.0") <= StrictVersion(Qt.qVersion()) < StrictVersion("5.0.0"):
        style = gr.prefs().get_string('qtgui', 'style', 'raster')
        Qt.QApplication.setGraphicsSystem(style)
    qapp = Qt.QApplication(sys.argv)

    tb = top_block_cls()
    tb.start()
    tb.show()

    def sig_handler(sig=None, frame=None):
        Qt.QApplication.quit()

    signal.signal(signal.SIGINT, sig_handler)
    signal.signal(signal.SIGTERM, sig_handler)

    timer = Qt.QTimer()
    timer.start(500)
    timer.timeout.connect(lambda: None)

    def quitting():
        tb.stop()
        tb.wait()
    qapp.aboutToQuit.connect(quitting)
    qapp.exec_()


if __name__ == '__main__':
    main()
