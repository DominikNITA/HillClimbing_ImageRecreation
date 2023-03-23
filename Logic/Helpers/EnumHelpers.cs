using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Helpers
{
    public class DropDownItem
    {
        public string Text { get; set; }
        public string Value { get; set; }
    }

    public static class EnumHelpers
    {
        public static IEnumerable<DropDownItem> ConvertEnumToDropDownSource<T>()
        {
            var type = typeof(T);
            return Enum.GetValues(type)
                       .Cast<T>()
                       .Select((x, i) => new DropDownItem
                       {
                           Text = Enum.GetNames(typeof(T))[i],
                           Value = x?.ToString() ?? string.Empty
                       }).ToList();
        }
        public static string GetDescription(this Enum GenericEnum)
        {
            Type genericEnumType = GenericEnum.GetType();
            MemberInfo[] memberInfo = genericEnumType.GetMember(GenericEnum.ToString());
            if ((memberInfo != null && memberInfo.Length > 0))
            {
                var _Attribs = memberInfo[0].GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
                if ((_Attribs != null && _Attribs.Count() > 0))
                {
                    return ((System.ComponentModel.DescriptionAttribute)_Attribs.ElementAt(0)).Description;
                }
            }
            return GenericEnum.ToString();
        }

    }
}
