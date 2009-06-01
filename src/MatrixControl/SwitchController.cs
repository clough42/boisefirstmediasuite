using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MatrixControl
{
    class SwitchController
    {
        public SwitchController(Settings settings)
        {
            settings.ComPortChanged += new ComPortChangedHandler(this.settings_ComPortChanged);
            OpenComPort(settings);
        }

        private void settings_ComPortChanged(Settings settings)
        {
            OpenComPort(settings);
        }

        private void OpenComPort(Settings settings)
        {
            MessageBox.Show("Com port: " + settings.ComPort);
        }

    }
}
