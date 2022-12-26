using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
