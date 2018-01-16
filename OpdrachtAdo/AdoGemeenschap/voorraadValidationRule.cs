using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AdoGemeenschap
{
    public class voorraadValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            int getal;
            if (value == null || value.ToString() == string.Empty)
                return new ValidationResult(false, "Getal moet ingevuld zijn");
            if (!int.TryParse(value.ToString(), out getal))
                return new ValidationResult(false, "Moet een getal zijn");
            return ValidationResult.ValidResult;
        }
    }
}
