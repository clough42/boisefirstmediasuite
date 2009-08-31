using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MatrixControl
{
    public partial class OverridesDialog : Form
    {
        Settings settings;
        Label[] outputLabels = null;
        ComboBox[] overrideBoxes = null;

        public OverridesDialog(Settings settings)
        {
            this.settings = settings;

            InitializeComponent();

            this.outputLabels = new Label[]{
                this.outputLabel1,
                this.outputLabel2,
                this.outputLabel3,
                this.outputLabel4,
                this.outputLabel5,
                this.outputLabel6,
                this.outputLabel7
            };
            
            this.overrideBoxes = new ComboBox[]{
                this.overrideBox1,
                this.overrideBox2,
                this.overrideBox3,
                this.overrideBox4,
                this.overrideBox5,
                this.overrideBox6,
                this.overrideBox7
            };
        }

        private void OverridesDialog_Shown(object sender, EventArgs e)
        {
            for (int i = 0; i < settings.NumOutputs; i++)
            {
                // set the label
                string outputLabel = settings.Outputs[i];
                if (outputLabel == null) outputLabel = "";
                this.outputLabels[i].Text = outputLabel;

                // fill out the overrides items
                this.overrideBoxes[i].Items.Clear();
                this.overrideBoxes[i].Items.Add(new OverrideItem(0, "(None)"));
                this.overrideBoxes[i].SelectedIndex = 0; // select this one for now
                for (int j = 0; j < this.settings.Inputs.Length; j++)
                {
                    // if this input is defined
                    if (this.settings.Inputs[j] != null)
                    {
                        // add an item
                        OverrideItem item = new OverrideItem(j + 1, this.settings.Inputs[j]);
                        this.overrideBoxes[i].Items.Add(item);

                        // if this one is selected
                        if (this.settings.Overrides[i] == (j + 1))
                            this.overrideBoxes[i].SelectedItem = item;
                    }
                }
            }
        }

        private class OverrideItem
        {
            public int Index;
            public string Label;

            public OverrideItem(int index, string label)
            {
                this.Index = index;
                this.Label = label;
            }

            public override string ToString()
            {
                return this.Label;
            }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            int[] overrides = new int[this.overrideBoxes.Length];

            for (int i = 0; i < this.overrideBoxes.Length; i++)
            {
                OverrideItem item = this.overrideBoxes[i].SelectedItem as OverrideItem;
                if (item == null)
                {
                    overrides[i] = 0;
                }
                else
                {
                    overrides[i] = item.Index;
                }
            }
            settings.Overrides = overrides;

            this.Close();
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            foreach (ComboBox box in this.overrideBoxes)
            {
                box.SelectedIndex = 0;
            }
        }
    }
}
