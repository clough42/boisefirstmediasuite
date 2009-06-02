using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;

namespace MatrixControl
{
    class SwitchController
    {
        private SerialPort port = null;

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

        /// <summary>
        /// Returns a list of valid com ports on this system.
        /// </summary>
        public static string[] ValidComPorts
        {
            get
            {
                return SerialPort.GetPortNames();
            }
        }

        private void OpenComPort(string comPort)
        {
            try
            {
                if (this.port != null)
                {
                    this.port.Close();
                    this.port = null;
                }

                if (comPort != null && comPort.Length > 0)
                {
                    port = new SerialPort(comPort);
                    port.BaudRate = 9600;
                    port.DataBits = 8;
                    port.StopBits = StopBits.One;
                    port.Parity = Parity.None;
                    port.Handshake = Handshake.None;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("ERROR creating serial port\r\n" + e.ToString());
            }
        }

        private void SwitchPreview(int preview)
        {

            string command = String.Format("{0}*8!\r\n", preview);
            port.Open();
            port.Write(command);
            port.Close();
        }

        private void SwitchPreset(int preset)
        {
            string command = String.Format("{0}.\r\n", preset);
            port.Open();
            port.Write(command);
            port.Close();
        }


    }
}
