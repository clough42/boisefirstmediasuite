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
            settings.SelectedPresetChanged += new SelectedPresetChangedHandler(this.settings_SelectedPresetChanged);
            settings.SelectedPreviewChanged += new SelectedPreviewChangedHandler(this.settings_SelectedPreviewChanged);
            OpenComPort(settings.ComPort);
        }

        private void settings_ComPortChanged(Settings settings)
        {
            OpenComPort(settings.ComPort);
        }

        private void settings_SelectedPresetChanged(Settings settings)
        {
            SwitchPreset(settings.SelectedPreset);
            SwitchPreview(settings.SelectedPreview);
        }

        private void settings_SelectedPreviewChanged(Settings settings)
        {
            SwitchPreview(settings.SelectedPreview);
        }

        private void OpenComPort(string comPort)
        {
            MessageBox.Show("Opening com port: " + comPort);
        }

        private void SwitchPreview(int preview)
        {
            MessageBox.Show("Switching to preview " + preview);
        }

        private void SwitchPreset(int preset)
        {
            MessageBox.Show("Switching to preset " + preset);
        }


    }
}
