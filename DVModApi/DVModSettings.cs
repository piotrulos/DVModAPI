using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Linq;

namespace DVModApi
{
    internal class SettingsList
    {
        public List<Setting> settings = new List<Setting>();
    }
    internal class Setting
    {
        public string ID;
        public object Value;

        public Setting(string id, object value)
        {
            ID = id;
            Value = value;
        }
    }
    internal enum SettingsType
    {
        CheckBox,
        Button,
        Slider,
        SliderInt,
        Text,
        Selector,
    }
    /// <summary>
    /// Class for adding custom Mod Settings
    /// </summary>
    public class DVModSettings
    {
        static DVModEntry mod = null;
        internal static void Settings(DVModEntry dVModEntry)
        {
            mod = dVModEntry;
            mod.ModSettingsSetup();
        }
        internal static void SaveSettings(DVModEntry mod)
        {
            if (!mod.hasModSettings) return;

            SettingsList list = new SettingsList();
            if (!Directory.Exists(Path.Combine(UMMEntryPoint.apimod.Path, "ModSettings")))
                Directory.CreateDirectory(Path.Combine(UMMEntryPoint.apimod.Path, "ModSettings"));
            string path = Path.Combine(UMMEntryPoint.apimod.Path, "ModSettings", $"{mod.modEntry.Info.Id}.json");
            LogHelper.Log($"Saving Mod Settings for <color=#00ffffff>{mod.modEntry.Info.Id}</color> to file...");
            for (int i = 0; i < mod.modSettings.Count; i++)
            {
                switch (mod.modSettings[i].Type)
                {
                    case SettingsType.CheckBox:
                        ModSettingsCheckBox scb = (ModSettingsCheckBox)mod.modSettings[i];
                        list.settings.Add(new Setting(scb.ID, scb.Value));
                        break;
                    case SettingsType.Slider:
                        ModSettingsSlider ss = (ModSettingsSlider)mod.modSettings[i];
                        list.settings.Add(new Setting(ss.ID, ss.Value));
                        break;
                    case SettingsType.SliderInt:
                        ModSettingsSliderInt ssi = (ModSettingsSliderInt)mod.modSettings[i];
                        list.settings.Add(new Setting(ssi.ID, ssi.Value));
                        break;
                    case SettingsType.Text:
                    case SettingsType.Button:
                    case SettingsType.Selector:
                        continue;
                }
            }
            string serializedData = JsonConvert.SerializeObject(list, Formatting.Indented);
            File.WriteAllText(path, serializedData);
        }

        internal static void LoadSettings(DVModEntry mod)
        {
            if (!mod.hasModSettings) return;
            string path = Path.Combine(UMMEntryPoint.apimod.Path, "ModSettings", $"{mod.modEntry.Info.Id}.json");
            if (!File.Exists(path)) return;
            SettingsList s = JsonConvert.DeserializeObject<SettingsList>(File.ReadAllText(path));
            LogHelper.Log($"Loading saved Mod Settings for <color=#00ffffff>{mod.modEntry.Info.Id}</color> from file...");
            for (int i = 0; i < s.settings.Count; i++)
            {
                ModSettings ms = mod.modSettings.Where(x => x.ID == s.settings[i].ID).FirstOrDefault();
                if (ms != null)
                {
                    switch (ms.Type)
                    {
                        case SettingsType.CheckBox:
                            ModSettingsCheckBox scb = (ModSettingsCheckBox)ms;
                            scb.SetValue(bool.Parse(s.settings[i].Value.ToString()));
                            break;
                        case SettingsType.Slider:
                            ModSettingsSlider ss = (ModSettingsSlider)ms;
                            ss.SetValue(float.Parse(s.settings[i].Value.ToString()));
                            break;
                        case SettingsType.SliderInt:
                            ModSettingsSliderInt ssi = (ModSettingsSliderInt)ms;
                            ssi.SetValue(int.Parse(s.settings[i].Value.ToString()));
                            break;
                        case SettingsType.Text:
                        case SettingsType.Button:
                        case SettingsType.Selector:
                            continue;
                    }
                }
            }
        }
        /// <summary>
        /// Add just static text in Settings
        /// </summary>
        /// <param name="text">Your text</param>
        public static void AddText(string text)
        {
            if (mod == null)
            {
                LogHelper.LogError("AddText() Mod Entry is null");
            }
            else
            {
                mod.modSettings.Add(new ModSettings(null, text, null, null, SettingsType.Text));            
            }
        }

        /// <summary>
        /// Add button to Settings
        /// </summary>
        /// <param name="name">Name on the button</param>
        /// <param name="tooltip">Tooltip displayed when you hover over button</param>
        /// <param name="onClick">Function to execute when button is clicked</param>
        public static void AddButton(string name, string tooltip, Action onClick)
        {
            if (mod == null)
            {
                LogHelper.LogError("AddButton() Mod Entry is null");
            }
            else
            {
                mod.modSettings.Add(new ModSettings(null, name, tooltip, onClick, SettingsType.Button)); 
            }
        }

        /// <summary>
        /// Add checkbox (toggle) to Settings
        /// </summary>
        /// <param name="id">Unique id for this checkbox</param>
        /// <param name="name">Name visible on checkbox</param>
        /// <param name="tooltip">Tooltip displayed when you hover over checkbox</param>
        /// <param name="value">Default value</param>
        /// <param name="onValueChanged">Function to execute when value is changed (optional)</param>
        /// <returns>ModSettingsCheckBox variable</returns>
        public static ModSettingsCheckBox AddCheckBox(string id, string name, string tooltip, bool value = false, Action onValueChanged = null)
        {
            if (mod == null)
            {
                LogHelper.LogError("AddCheckBox() Mod Entry is null");
                return null;
            }
            ModSettingsCheckBox c = new ModSettingsCheckBox(id, name, tooltip, value, onValueChanged, SettingsType.CheckBox);
            mod.modSettings.Add(c);
            return c;
        }
        /// <summary>
        /// Add slider to settings that uses float values
        /// </summary>
        /// <param name="id">Unique id for this slider</param>
        /// <param name="name">Name visible on slider</param>
        /// <param name="tooltip">Tooltip displayed when you hover over slider</param>
        /// <param name="minValue">Min allowed value for slider</param>
        /// <param name="maxValue">Max allowed value for slider</param>
        /// <param name="value">Default value for slider</param>
        /// <param name="decimalPoints">Round to number of decimal points (default 2)</param>
        /// <param name="onValueChanged">Function to execute if value is changed (optional)</param>
        /// <returns>ModSettingsSlider</returns>
        public static ModSettingsSlider AddSlider(string id, string name, string tooltip, float minValue, float maxValue, float value = 0f, int decimalPoints = 2, Action onValueChanged = null)
        {
            if (mod == null)
            {
                LogHelper.LogError("AddSlider() Mod Entry is null");
                return null;
            }
            if(decimalPoints < 0)
            {
                LogHelper.LogError("AddSlider() Decimal points cannot be negative (defaulting to 2)");
                decimalPoints = 2;
            }
            ModSettingsSlider s = new ModSettingsSlider(id, name, tooltip, value, minValue, maxValue, decimalPoints, onValueChanged, SettingsType.Slider);
            mod.modSettings.Add(s);
            return s;
        }
        /// <summary>
        /// Add slider to settings that uses integer values
        /// </summary>
        /// <param name="id">Unique id for this slider</param>
        /// <param name="name">Name visible on slider</param>
        /// <param name="tooltip">Tooltip displayed when you hover over slider</param>
        /// <param name="minValue">Min allowed value for slider</param>
        /// <param name="maxValue">Max allowed value for slider</param>
        /// <param name="value">Default value for slider</param>
        /// <param name="onValueChanged">Function to execute if value is changed (optional)</param>
        /// <returns>ModSettingsSliderInt</returns>
        public static ModSettingsSliderInt AddSlider(string id, string name, string tooltip, int minValue, int maxValue, int value = 0, Action onValueChanged = null)
        {
            if (mod == null)
            {
                LogHelper.LogError("AddSlider() Mod Entry is null");
                return null;
            }
            ModSettingsSliderInt s = new ModSettingsSliderInt(id, name, tooltip, value, minValue, maxValue, null, onValueChanged, SettingsType.SliderInt);
            mod.modSettings.Add(s);
            return s;
        }
        /// <summary>
        /// Add slider to settings that uses integer values (displays text from array based on value)
        /// </summary>
        /// <param name="id">Unique id for this slider</param>
        /// <param name="name">Name visible on slider</param>
        /// <param name="tooltip">Tooltip displayed when you hover over slider</param>
        /// <param name="textValues">Array of text values</param>
        /// <param name="value">Default value for slider (within array bounds)</param>
        /// <param name="onValueChanged">Function to execute if value is changed (optional)</param>
        /// <returns>ModSettingsSliderInt</returns>
        public static ModSettingsSliderInt AddSlider(string id, string name, string tooltip, string[] textValues, int value = 0, Action onValueChanged = null)
        {
            if (mod == null)
            {
                LogHelper.LogError("AddSlider() Mod Entry is null");
                return null;
            }
            if(textValues == null)
            {
                LogHelper.LogError("AddSlider() textValues is null");
                AddText("Error: AddSlider() textValues is null");
                return null;
            }
            if(textValues.Length < 2)
            {
                LogHelper.LogError("AddSlider() textValues array needs at least 2 elements");
                AddText("Error: AddSlider() textValues array needs at least 2 elements");
                return null;
            }
            ModSettingsSliderInt s = new ModSettingsSliderInt(id, name, tooltip, value, 0, textValues.Length-1, textValues, onValueChanged, SettingsType.SliderInt);
            mod.modSettings.Add(s);
            return s;
        }
    }

    /// <summary>
    /// Mod Settings base class
    /// </summary>
    public class ModSettings
    {
        internal string ID;
        internal string Name;
        internal string Tooltip;
        internal Action DoAction;
        internal SettingsType Type;       

        internal ModSettings(string id, string name, string tooltip, Action doAction, SettingsType type)
        {
            ID = id;
            Name = name;
            Tooltip = tooltip;
            DoAction = doAction;
            Type = type;
        }
    }
    /// <summary>
    /// Variable to get/set values from checkbox (toggle button)
    /// </summary>
    public class ModSettingsCheckBox : ModSettings
    {
        internal bool Value = false;

        internal ModSettingsCheckBox(string id, string name, string tooltip, bool value, Action doAction, SettingsType type) : base(id, name, tooltip, doAction, type)
        {
            Value = value;
        }

        /// <summary>
        /// Get checkbox value
        /// </summary>
        /// <returns>true/false</returns>
        public bool GetValue()
        {
            return Value;
        }

        /// <summary>
        /// Set checkbox settings value
        /// </summary>
        /// <param name="value">true/false</param>
        public void SetValue(bool value)
        {
            Value = value;
        }
    }
    /// <summary>
    /// Variable to get/set values from slider
    /// </summary>
    public class ModSettingsSlider : ModSettings
    {
        internal float Value = 0f;
        internal float MinValue = 0f;
        internal float MaxValue = 10f;
        internal int DecimalPoints = 2;

        internal ModSettingsSlider(string id, string name, string tooltip, float value, float minValue, float maxValue, int decimalPoints, Action doAction, SettingsType type) : base(id, name, tooltip, doAction, type)
        {
            Value = value;
            MinValue = minValue;
            MaxValue = maxValue;
            DecimalPoints = decimalPoints;
        }



        /// <summary>
        /// Get Slider value
        /// </summary>
        /// <returns>float value</returns>
        public float GetValue()
        {
            return Value;
        }

        /// <summary>
        /// Set Slider settings value
        /// </summary>
        /// <param name="value">float value</param>
        public void SetValue(float value)
        {
            if (value > MaxValue)
                Value = MaxValue;
            else if (value < MinValue)
                Value = MinValue;
            else
                Value = value;
        }
    }
    /// <summary>
    /// Variable to get/set values from slider (integer/text version)
    /// </summary>
    public class ModSettingsSliderInt : ModSettings
    {
        internal int Value = 0;
        internal int MinValue = 0;
        internal int MaxValue = 10;
        internal string[] valuesArray = null;

        internal ModSettingsSliderInt(string id, string name, string tooltip, int value, int minValue, int maxValue, string[] values, Action doAction, SettingsType type) : base(id, name, tooltip, doAction, type)
        {
            Value = value;
            MinValue = minValue;
            MaxValue = maxValue;
            valuesArray = values;
        }

        /// <summary>
        /// Get Slider value
        /// </summary>
        /// <returns>float value</returns>
        public int GetValue()
        {
            return Value;
        }

        /// <summary>
        /// Set Slider settings value
        /// </summary>
        /// <param name="value">float value</param>
        public void SetValue(int value)
        {
            if (value > MaxValue)
                Value = MaxValue;
            else if (value < MinValue)
                Value = MinValue;
            else
                Value = value;
        }
    }
}
