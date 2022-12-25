using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Helpers
{
    public static class StringHelper
    {
        public static string ListProperties(this object obj)
        {
            var props = obj.GetType().GetProperties();
            var sb = new StringBuilder();
            foreach (var p in props)
            {
                sb.AppendLine(p.Name + ": " + p.GetValue(obj, null));
            }
            return sb.ToString();
        }

        public static string PropertiesToString<T>(this T obj, int tabs = 0) where T : class
        {
            int initTabs = tabs;
            string result = string.Empty;
            PropertyInfo[] propertyInfo = obj.GetType().GetProperties();
            foreach (PropertyInfo property in propertyInfo)
            {
                string name = property.Name;
                object value = property.GetValue(obj, null);
                Type valueType = value?.GetType();
                if (valueType == null || valueType.IsValueType || valueType.Name.Equals("String") || value is IEnumerable)
                {
                    for (int i = 0; i < tabs; i++)
                    {
                        result += "    ";
                    }
                    if(value is IEnumerable && valueType.Name.Equals("String") == false)
                    {
                        var entries = new List<string>();
                        foreach (var listitem in value as IEnumerable)
                        {
                            entries.Add(listitem.ToString());
                        }
                        value = $"[{String.Join(", ", entries)}]";
                    }
                    result += string.Format("{0}: {1}\n", name, value == null ? "NULL" : value.ToString());
                }
                else
                {
                    result += string.Format("{0}:\n", name);
                    result += value.PropertiesToString(++tabs);
                }
                tabs = initTabs;
            }
            return result;
        }
    }
}
