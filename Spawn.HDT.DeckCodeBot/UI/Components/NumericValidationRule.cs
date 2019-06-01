#region Using
using System;
using System.Globalization;
using System.Windows.Controls;
#endregion

namespace Spawn.HDT.DeckCodeBot.UI.Components
{
    public class NumericValidationRule : ValidationRule
    {
        #region Properties
        #region ErrorMessage
        public string ErrorMessage => $"Invalid value! Enter a value between {MinValue} and {MaxValue}.";
        #endregion

        #region MinValue
        public int MinValue { get; set; }
        #endregion

        #region MaxValue
        public int MaxValue { get; set; }
        #endregion
        #endregion

        #region Validate
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            ValidationResult retVal = ValidationResult.ValidResult;

            try
            {
                int nValue = Convert.ToInt32(value.ToString());

                if (nValue < MinValue || nValue > MaxValue)
                    retVal = new ValidationResult(false, ErrorMessage);
            }
            catch
            {
                retVal = new ValidationResult(false, ErrorMessage);
            }

            return retVal;
        }
        #endregion
    }
}