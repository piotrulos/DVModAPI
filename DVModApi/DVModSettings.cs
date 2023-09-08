using DV.Booklets;
using System;
using System.Reflection.Emit;

namespace DVModApi
{
    internal enum SettingsType
    {
        CheckBox,
        Button,
        Slider,
        SliderInt,
        Text,
        Selector,
    }
    public class DVModSettings
    {
        static DVModEntry mod = null;
        internal static void Settings(DVModEntry dVModEntry)
        {
            mod = dVModEntry;
            mod.ModSettingsSetup();
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
        /// <param name="onClick">Your action when button is clicked</param>
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
        /// <param name="id">Unique id for this setting</param>
        /// <param name="name">Name visible on checkbox</param>
        /// <param name="tooltip">Tooltip displayed when you hover over checkbox</param>
        /// <param name="value">Default value</param>
        /// <param name="onValueChanged">optional action when value is changed</param>
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
