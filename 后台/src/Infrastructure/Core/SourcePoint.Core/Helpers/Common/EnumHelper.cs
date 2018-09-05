using System;
using System.ComponentModel;
using System.Reflection;

namespace SourcePoint.Core.Helpers.Common
{
    public sealed class EnumHelper
    {
        public static string GetDescription(Enum enumObj)
        {
            if (enumObj == null) return null;
            var fieldInfo = enumObj.GetType().GetField(enumObj.ToString(), BindingFlags.Public | BindingFlags.Static);
            var attr = fieldInfo.GetCustomAttribute<DescriptionAttribute>();
            return attr?.Description;
        }

        public static T GetCustomAttribute<T>(Enum enumObj) where T : System.Attribute
        {
            if (enumObj == null) return null;
            var fieldInfo = enumObj.GetType().GetField(enumObj.ToString(), BindingFlags.Public | BindingFlags.Static);
            var attr = fieldInfo.GetCustomAttribute<T>();
            return attr;
        }
    }
}
