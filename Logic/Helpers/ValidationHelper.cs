using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Helpers
{
    public static class ValidationHelper
    {
        public class MustHaveOneElementAttribute : ValidationAttribute
        {
            public MustHaveOneElementAttribute(string errorMessage) : base(errorMessage)
            {
            }

            public override bool IsValid(object value)
            {
                var collection = value as IEnumerable;
                if (collection != null && collection.GetEnumerator().MoveNext())
                {
                    return true;
                }
                return false;
            }
        }
    }
}
