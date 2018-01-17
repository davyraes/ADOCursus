using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AdoGemeenschap
{
    public class InVoorraadValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            int getal;
            if (value == null || value.ToString() == string.Empty)
                return new ValidationResult(false, "Getal moet ingevuld zijn");
            if (!int.TryParse(value.ToString(), out getal))
                return new ValidationResult(false, "Moet een getal zijn");
            if (getal <= 0)
                return new ValidationResult(false, "Getal moet groter dan 0 zijn");
            return ValidationResult.ValidResult; ;
        }
    }
}
