using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AdoGemeenschap
{
    public class PrijsValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            decimal getal;
            NumberStyles style = NumberStyles.Currency;
            if(value == null || value.ToString() == string.Empty)
                return new ValidationResult(false, "Getal moet ingevuld zijn");
            if (!decimal.TryParse(value.ToString(), style, CultureInfo.CurrentCulture, out getal))
                return new ValidationResult(false, "Moet een getal zijn");
            if (getal <= 0m)
                return new ValidationResult(false, "Getal moet groter dan 0 zijn");
            return ValidationResult.ValidResult;
        }
    }
}
