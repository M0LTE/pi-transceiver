#!/usr/bin/env python3
# -*- coding: utf-8 -*-

#
# SPDX-License-Identifier: GPL-3.0
#
# GNU Radio Python Flow Graph
# Title: Not titled yet
# GNU Radio version: 3.8.1.0

from gnuradio import audio
from gnuradio import filter
from gnuradio.filter import firdes
from gnuradio import gr
import sys
import signal
from argparse import ArgumentParser
from gnuradio.eng_arg import eng_float, intx
from gnuradio import eng_notation
import limesdr

class SSB_TX(gr.top_block):

    def __init__(self):
        gr.top_block.__init__(self, "Not titled yet")

        ##################################################
        # Variables
        ##################################################
        self.samp_rate = samp_rate = 32000
        self.lime_serial = lime_serial = "1D5882F9FB64B6"

        ##################################################
        # Blocks
        ##################################################
        self.limesdr_sink_0 = limesdr.sink(lime_serial, 0, '', '')


        self.limesdr_sink_0.set_sample_rate(samp_rate)


        self.limesdr_sink_0.set_center_freq(144200000, 0)

        self.limesdr_sink_0.set_bandwidth(5e6, 0)


        self.limesdr_sink_0.set_digital_filter(samp_rate, 0)


        self.limesdr_sink_0.set_gain(63, 0)


        self.limesdr_sink_0.set_antenna(255, 0)


        self.limesdr_sink_0.calibrate(2.5e6, 0)
        self.hilbert_fc_0 = filter.hilbert_fc(65, firdes.WIN_HAMMING, 6.76)
        self.dc_blocker_xx_1 = filter.dc_blocker_ff(32, True)
        self.band_pass_filter_0 = filter.fir_filter_fff(
            1,
            firdes.band_pass(
                1,
                samp_rate,
                200,
                2800,
                10,
                firdes.WIN_HAMMING,
                6.76))
        self.audio_source_0 = audio.source(samp_rate, '', True)



        ##################################################
        # Connections
        ##################################################
        self.connect((self.audio_source_0, 0), (self.band_pass_filter_0, 0))
        self.connect((self.band_pass_filter_0, 0), (self.dc_blocker_xx_1, 0))
        self.connect((self.dc_blocker_xx_1, 0), (self.hilbert_fc_0, 0))
        self.connect((self.hilbert_fc_0, 0), (self.limesdr_sink_0, 0))

    def get_samp_rate(self):
        return self.samp_rate

    def set_samp_rate(self, samp_rate):
        self.samp_rate = samp_rate
        self.band_pass_filter_0.set_taps(firdes.band_pass(1, self.samp_rate, 200, 2800, 10, firdes.WIN_HAMMING, 6.76))
        self.limesdr_sink_0.set_digital_filter(self.samp_rate, 0)
        self.limesdr_sink_0.set_digital_filter(self.samp_rate, 1)

    def get_lime_serial(self):
        return self.lime_serial

    def set_lime_serial(self, lime_serial):
        self.lime_serial = lime_serial



def main(top_block_cls=SSB_TX, options=None):
    tb = top_block_cls()

    def sig_handler(sig=None, frame=None):
        tb.stop()
        tb.wait()
        sys.exit(0)

    signal.signal(signal.SIGINT, sig_handler)
    signal.signal(signal.SIGTERM, sig_handler)

    tb.start()
    tb.wait()


if __name__ == '__main__':
    main()
