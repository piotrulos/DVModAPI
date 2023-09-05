using System;

namespace DVModApi
{
    internal enum SettingsType
    {
        CheckBox,
        Button,
        Slider,
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

        public static ModSettingsCheckBox AddCheckBox(string id, string name, string tooltip, bool value =false, Action onValueChanged = null)
        {
            if(mod == null)
            {
                LogHelper.LogError("AddCheckBox() Mod Entry is null");
            }
            ModSettingsCheckBox c = new ModSettingsCheckBox(id, name, tooltip, value, onValueChanged, SettingsType.CheckBox);
            mod.modSettings.Add(c);
            return c;
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
}
