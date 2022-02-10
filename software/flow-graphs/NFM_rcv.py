#!/usr/bin/env python3
# -*- coding: utf-8 -*-

#
# SPDX-License-Identifier: GPL-3.0
#
# GNU Radio Python Flow Graph
# Title: NFM_rcv
# Author: Barry Duggan
# Description: NB FM receiver
# GNU Radio version: 3.8.2.0

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

from gnuradio import analog
from gnuradio import audio
from gnuradio import blocks
from gnuradio import filter
from gnuradio.filter import firdes
from gnuradio import gr
import sys
import signal
from PyQt5 import Qt
from argparse import ArgumentParser
from gnuradio.eng_arg import eng_float, intx
from gnuradio import eng_notation
from gnuradio.qtgui import Range, RangeWidget
import limesdr
try:
    from xmlrpc.server import SimpleXMLRPCServer
except ImportError:
    from SimpleXMLRPCServer import SimpleXMLRPCServer
import threading

from gnuradio import qtgui

class NFM_rcv(gr.top_block, Qt.QWidget):

    def __init__(self):
        gr.top_block.__init__(self, "NFM_rcv")
        Qt.QWidget.__init__(self)
        self.setWindowTitle("NFM_rcv")
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

        self.settings = Qt.QSettings("GNU Radio", "NFM_rcv")

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
        self.volume = volume = 0.5
        self.sq_lvl = sq_lvl = -60
        self.samp_rate = samp_rate = 2000000
        self.rf_gain = rf_gain = 50
        self.rf_decim = rf_decim = 1
        self.freq_value = freq_value = 433000000
        self.freq = freq = 433000000

        ##################################################
        # Blocks
        ##################################################
        self._volume_range = Range(0, 1.00, 0.05, 0.5, 200)
        self._volume_win = RangeWidget(self._volume_range, self.set_volume, 'Volume', "slider", float)
        self.top_grid_layout.addWidget(self._volume_win)
        self._sq_lvl_range = Range(-100, 0, 5, -60, 200)
        self._sq_lvl_win = RangeWidget(self._sq_lvl_range, self.set_sq_lvl, 'Squelch', "counter_slider", float)
        self.top_grid_layout.addWidget(self._sq_lvl_win)
        self._rf_gain_range = Range(0, 70, 1, 50, 200)
        self._rf_gain_win = RangeWidget(self._rf_gain_range, self.set_rf_gain, 'RF Gain', "slider", float)
        self.top_grid_layout.addWidget(self._rf_gain_win)
        self.xmlrpc_server_0 = SimpleXMLRPCServer(('localhost', 8080), allow_none=True)
        self.xmlrpc_server_0.register_instance(self)
        self.xmlrpc_server_0_thread = threading.Thread(target=self.xmlrpc_server_0.serve_forever)
        self.xmlrpc_server_0_thread.daemon = True
        self.xmlrpc_server_0_thread.start()
        self.rational_resampler_xxx_1_0_1 = filter.rational_resampler_ccc(
                interpolation=48,
                decimation=200,
                taps=None,
                fractional_bw=0.1)
        self.low_pass_filter_0 = filter.fir_filter_ccf(
            1,
            firdes.low_pass(
                1,
                samp_rate,
                28000,
                21000,
                firdes.WIN_HAMMING,
                6.76))
        self.limesdr_source_0 = limesdr.source('1D588FD736569E', 0, '')


        self.limesdr_source_0.set_sample_rate(samp_rate)


        self.limesdr_source_0.set_center_freq(freq_value, 0)

        self.limesdr_source_0.set_bandwidth(1.5e6, 0)


        self.limesdr_source_0.set_digital_filter(samp_rate, 0)


        self.limesdr_source_0.set_gain(rf_gain, 0)


        self.limesdr_source_0.set_antenna(255, 0)
        self._freq_range = Range(433000000, 433375000, 25000, 433000000, 50)
        self._freq_win = RangeWidget(self._freq_range, self.set_freq, 'Frequency RB0-RB15', "dial", float)
        self.top_grid_layout.addWidget(self._freq_win)
        self.blocks_multiply_const_vxx_0_0 = blocks.multiply_const_ff(volume)
        self.audio_sink_0 = audio.sink(48000, '', False)
        self.analog_simple_squelch_cc_0 = analog.simple_squelch_cc(sq_lvl, 1)
        self.analog_nbfm_rx_0 = analog.nbfm_rx(
        	audio_rate=48000,
        	quad_rate=int(192000),
        	tau=75E-6,
        	max_dev=5000,
          )



        ##################################################
        # Connections
        ##################################################
        self.connect((self.analog_nbfm_rx_0, 0), (self.blocks_multiply_const_vxx_0_0, 0))
        self.connect((self.analog_simple_squelch_cc_0, 0), (self.analog_nbfm_rx_0, 0))
        self.connect((self.blocks_multiply_const_vxx_0_0, 0), (self.audio_sink_0, 0))
        self.connect((self.limesdr_source_0, 0), (self.low_pass_filter_0, 0))
        self.connect((self.low_pass_filter_0, 0), (self.rational_resampler_xxx_1_0_1, 0))
        self.connect((self.rational_resampler_xxx_1_0_1, 0), (self.analog_simple_squelch_cc_0, 0))


    def closeEvent(self, event):
        self.settings = Qt.QSettings("GNU Radio", "NFM_rcv")
        self.settings.setValue("geometry", self.saveGeometry())
        event.accept()

    def get_volume(self):
        return self.volume

    def set_volume(self, volume):
        self.volume = volume
        self.blocks_multiply_const_vxx_0_0.set_k(self.volume)

    def get_sq_lvl(self):
        return self.sq_lvl

    def set_sq_lvl(self, sq_lvl):
        self.sq_lvl = sq_lvl
        self.analog_simple_squelch_cc_0.set_threshold(self.sq_lvl)

    def get_samp_rate(self):
        return self.samp_rate

    def set_samp_rate(self, samp_rate):
        self.samp_rate = samp_rate
        self.limesdr_source_0.set_digital_filter(self.samp_rate, 0)
        self.limesdr_source_0.set_digital_filter(self.samp_rate, 1)
        self.low_pass_filter_0.set_taps(firdes.low_pass(1, self.samp_rate, 28000, 21000, firdes.WIN_HAMMING, 6.76))

    def get_rf_gain(self):
        return self.rf_gain

    def set_rf_gain(self, rf_gain):
        self.rf_gain = rf_gain
        self.limesdr_source_0.set_gain(self.rf_gain, 0)

    def get_rf_decim(self):
        return self.rf_decim

    def set_rf_decim(self, rf_decim):
        self.rf_decim = rf_decim

    def get_freq_value(self):
        return self.freq_value

    def set_freq_value(self, freq_value):
        self.freq_value = freq_value
        self.limesdr_source_0.set_center_freq(self.freq_value, 0)

    def get_freq(self):
        return self.freq

    def set_freq(self, freq):
        self.freq = freq





def main(top_block_cls=NFM_rcv, options=None):

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
