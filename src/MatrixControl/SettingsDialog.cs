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
    public partial class SettingsDialog : Form
    {
        Settings settings;
        private TextBox[] presetBoxes;
        private TextBox[] inputBoxes;

        public SettingsDialog(Settings settings)
        {
            this.settings = settings;

            InitializeComponent();

            this.presetBoxes = new TextBox[]{
                this.presetBox1,
                this.presetBox2,
                this.presetBox3,
                this.presetBox4,
                this.presetBox5,
                this.presetBox6,
                this.presetBox7,
                this.presetBox8,
                this.presetBox9,
                this.presetBox10,
                this.presetBox11,
                this.presetBox12,
                this.presetBox13,
                this.presetBox14,
                this.presetBox15,
                this.presetBox16
            };
            this.inputBoxes = new TextBox[]{
                this.inputBox1,
                this.inputBox2,
                this.inputBox3,
                this.inputBox4,
                this.inputBox5,
                this.inputBox6,
                this.inputBox7,
                this.inputBox8
            };
        }

        private void Settings_Shown(object sender, EventArgs e)
        {
            //string[] portNames = SerialPort.GetPortNames();
            string[] portNames = new string[] { "COM1", "COM2", "COM3" };
            this.comPortComboBox.Items.Clear();
            foreach (string portName in portNames)
            {
                this.comPortComboBox.Items.Add(portName);
            }

            if (settings.ComPort != null && this.comPortComboBox.Items.Contains(settings.ComPort))
            {
                this.comPortComboBox.SelectedItem = settings.ComPort;
            }

            int i;
            for (i = 0; i < settings.NumPresets; i++)
            {
                this.presetBoxes[i].Text = (settings.Presets[i] == null ? "" : settings.Presets[i]);
            }
            for (i = 0; i < settings.NumInputs; i++)
            {
                this.inputBoxes[i].Text = (settings.Inputs[i] == null ? "" : settings.Inputs[i]);
            }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            string selectedComPort = this.comPortComboBox.SelectedItem as String;
            if (selectedComPort != null && selectedComPort != settings.ComPort)
            {
                settings.ComPort = selectedComPort;
            }

            int i;

            string[] presets = new string[settings.NumPresets];
            for (i = 0; i < settings.NumPresets; i++)
            {
                string preset = presetBoxes[i].Text;
                preset = preset.Trim();
                if (preset.Length == 0)
                {
                    preset = null;
                }
                presets[i] = preset;
            }
            settings.Presets = presets;

            string[] inputs = new string[settings.NumInputs];
            for (i = 0; i < settings.NumInputs; i++)
            {
                string input = inputBoxes[i].Text;
                input = input.Trim();
                if (input.Length == 0)
                {
                    input = null;
                }
                inputs[i] = input;
            }
            settings.Inputs = inputs;

            this.Close();
        }

    }
}
