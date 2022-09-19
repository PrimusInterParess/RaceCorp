namespace RaceCorp.Services.ValidationAttributes
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Net.NetworkInformation;
    using System.Text;
    using System.Threading.Tasks;

    using static RaceCorp.Services.Constants.NumbersValues;

    public class ValidateDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            DateTime d = Convert.ToDateTime(value);

            return d.Year >= YearMinValue;
        }
    }
}
