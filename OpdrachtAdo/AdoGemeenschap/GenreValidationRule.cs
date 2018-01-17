using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AdoGemeenschap
{
    public class GenreValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value==null||value.ToString()==string.Empty||(int)value==0)
                return new ValidationResult(false, "Genre moet gekozen worden");
            return ValidationResult.ValidResult;
        }
    }
}
