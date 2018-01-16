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
            if (value==null||((Genre)value).GenreNr == 0||value.ToString()==string.Empty)
                return new ValidationResult(false, "Genre moet gekozen worden");
            return ValidationResult.ValidResult;
        }
    }
}
