using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Microsoft.Win32;

namespace MatrixControl
{

    public delegate void ComPortChangedHandler(Settings settings);
    public delegate void PresetsChangedHandler(Settings settings);
    public delegate void InputsChangedHandler(Settings settings);
    public delegate void SelectedPresetChangedHandler(Settings settings);
    public delegate void SelectedPreviewChangedHandler(Settings settings);

    /// <summary>
    /// The MatrixControlBar settings object.  This class persists the settings
    /// for the matrix control bar, and it has events so other parts of the system
    /// can be notified when settings change.
    /// </summary>
    public class Settings
    {
        const int NUM_PRESETS = 16;
        const int NUM_INPUTS = 8;
        
        private string comPort = null;
        private string[] presets = null;
        private string[] inputs = null;
        private int selectedPreset;
        private int selectedPreview;

        /// <summary>
        /// Create a new instance of the Settings object.
        /// </summary>
        public Settings()
        {
            this.comPort = null;
            this.presets = null;
            this.inputs = null;
            this.selectedPreset = 0;
            this.selectedPreview = 0;

            this.ComPortChanged = null;
            this.InputsChanged = null;
            this.PresetsChanged = null;
            this.SelectedPresetChanged = null;
            this.SelectedPreviewChanged = null;

            LoadSettings();
        }

        /// <summary>
        /// The com port where the matrix switch is connected
        /// </summary>
        public string ComPort
        {
            get { return this.comPort; }
            set
            {
                this.comPort = value;
                if (this.ComPortChanged != null)
                {
                    this.ComPortChanged(this);
                }
                SaveSettings();
            }
        }

        /// <summary>
        /// Event: triggered whenever the com port is changed
        /// </summary>
        public ComPortChangedHandler ComPortChanged;

        /// <summary>
        /// The number of presets on the switch
        /// </summary>
        public int NumPresets
        {
            get { return NUM_PRESETS; }
        }

        /// <summary>
        /// The number of inputs on the switch
        /// </summary>
        public int NumInputs
        {
            get { return NUM_INPUTS; }
        }


        /// <summary>
        /// The array of presets.  This is a zero-indexed array (0:n-1) and represents
        /// the presets 1-n.  Null values indicate that the preset is not used.  Non-null
        /// values are the name of the corresponding preset.  Presets[n-1] contains the name
        /// of preset n.
        /// </summary>
        public string[] Presets
        {
            get
            {
                if (this.presets == null)
                {
                    this.presets = new string[NUM_PRESETS];
                }
                return this.presets;
            }
            set
            {
                if (value.Length != NUM_PRESETS)
                {
                    throw new ArgumentException("Presets array is the wrong length");
                }
                bool changed = false;
                for (int i = 0; i < value.Length; i++)
                {
                    if (this.presets[i] != value[i])
                    {
                        changed = true;
                    }
                }
                if (changed)
                {
                    this.presets = value;
                    if (this.PresetsChanged != null)
                    {
                        this.PresetsChanged(this);
                    }
                    SaveSettings();
                }
            }

        }

        /// <summary>
        /// Event: triggered when the list of presets is changed
        /// </summary>
        public PresetsChangedHandler PresetsChanged;


        /// <summary>
        /// The array of inputs.  This is a zero-indexed array (0:n-1) and represents
        /// the inputs 1-n.  Null values indicate that the input is not used.  Non-null
        /// values are the name of the corresponding input.  Inputs[n-1] contains the name
        /// of input n.
        /// </summary>
        public string[] Inputs
        {
            get
            {
                if (this.inputs == null)
                {
                    this.inputs = new string[NUM_INPUTS];
                }
                return this.inputs;
            }
            set
            {
                if (value.Length != NUM_INPUTS)
                {
                    throw new ArgumentException("Inputs array is the wrong length");
                }
                bool changed = false;
                for (int i = 0; i < value.Length; i++)
                {
                    if (this.inputs[i] != value[i])
                    {
                        changed = true;
                    }
                }
                if (changed)
                {
                    this.inputs = value;
                    if (this.InputsChanged != null)
                    {
                        this.InputsChanged(this);
                    }
                    SaveSettings();
                }
            }
        }

        public InputsChangedHandler InputsChanged;

        /// <summary>
        /// The selected (1-based) preset.  This is a number.  If the SelectedPreset is
        /// n, then Presets[n-1] contains the name of the preset.
        /// </summary>
        public int SelectedPreset
        {
            get
            {
                return selectedPreset;
            }
            set
            {
                if (this.Presets[value-1] == null)
                {
                    throw new ArgumentException("Preset " + value + " is not in the list of valid presets");
                }
                this.selectedPreset = value;
                if (this.SelectedPresetChanged != null)
                {
                    this.SelectedPresetChanged(this);
                }
            }
        }

        public SelectedPresetChangedHandler SelectedPresetChanged;


        public int SelectedPreview
        {
            get
            {
                return this.selectedPreview;
            }
            set
            {
                if (this.Inputs[value-1] == null)
                {
                    throw new ArgumentException("Input " + value + " is not in the list of inputs");
                }
                this.selectedPreview = value;
                if (this.SelectedPreviewChanged != null)
                {
                    this.SelectedPreviewChanged(this);
                }
            }
        }

        public SelectedPreviewChangedHandler SelectedPreviewChanged;

        const string BOISEFIRST = "Boise First";
        const string MEDIASUITE = "Media Suite";
        const string MATRIXCONTROL = "Matrix Control Bar";
        const string COMPORT = "ComPort";
        const string PRESET = "Preset";
        const string INPUT = "Input";

        private void SaveSettings()
        {
            using (RegistryKey key = Registry.CurrentUser)
            {
                using (RegistryKey swKey = OpenOrCreateSubKey(key, "Software"))
                {
                    using (RegistryKey bfKey = OpenOrCreateSubKey(swKey, BOISEFIRST))
                    {
                        using (RegistryKey msKey = OpenOrCreateSubKey(bfKey, MEDIASUITE))
                        {
                            using (RegistryKey mcKey = OpenOrCreateSubKey(msKey, MATRIXCONTROL))
                            {
                                SetKeyValue(mcKey, COMPORT, this.comPort, RegistryValueKind.String);

                                for (int i = 0; i < this.Presets.Length; i++)
                                {
                                    SetKeyValue(mcKey, PRESET + (i + 1), this.Presets[i], RegistryValueKind.String);
                                }
                                for (int i = 0; i < this.Inputs.Length; i++)
                                {
                                    SetKeyValue(mcKey, INPUT + (i + 1), this.Inputs[i], RegistryValueKind.String);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void LoadSettings()
        {
            using (RegistryKey key = Registry.CurrentUser)
            {
                using (RegistryKey swKey = OpenOrCreateSubKey(key, "Software"))
                {
                    using (RegistryKey bfKey = OpenOrCreateSubKey(swKey, BOISEFIRST))
                    {
                        using (RegistryKey msKey = OpenOrCreateSubKey(bfKey, MEDIASUITE))
                        {
                            using (RegistryKey mcKey = OpenOrCreateSubKey(msKey, MATRIXCONTROL))
                            {
                                this.comPort = mcKey.GetValue(COMPORT, null) as string;

                                for (int i = 0; i < this.Presets.Length; i++)
                                {
                                    this.Presets[i] = mcKey.GetValue(PRESET + (i + 1), null) as string;
                                }
                                for (int i = 0; i < this.Inputs.Length; i++)
                                {
                                    this.Inputs[i] = mcKey.GetValue(INPUT + (i + 1), null) as string;
                                }
                            }
                        }
                    }
                }
            }
        }

        RegistryKey OpenOrCreateSubKey(RegistryKey parent, string subKeyName)
        {
            RegistryKey child = parent.OpenSubKey(subKeyName, true);
            if (child == null)
            {
                child = parent.CreateSubKey(subKeyName, RegistryKeyPermissionCheck.ReadWriteSubTree);
            }
            return child;
        }

        void SetKeyValue(RegistryKey key, string valueName, object value, RegistryValueKind valueKind)
        {
            if (value == null)
            {
                key.DeleteValue(valueName, false);
            }
            else
            {
                key.SetValue(valueName, value, valueKind);
            }
        }

    }
}
