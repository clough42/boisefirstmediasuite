using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;

namespace MatrixControl
{
    public partial class Settings : Form
    {
        public Settings()
        {
            InitializeComponent();
        }

        private void Settings_Shown(object sender, EventArgs e)
        {
            string[] portNames = SerialPort.GetPortNames();
            this.comPortComboBox.Items.Clear();
            foreach (string portName in portNames)
            {
                this.comPortComboBox.Items.Add(portName);
            }
        }
    }
}
