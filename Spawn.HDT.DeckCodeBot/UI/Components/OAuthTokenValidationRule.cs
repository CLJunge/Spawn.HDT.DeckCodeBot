#region Using
using System.Globalization;
using System.Windows.Controls;
#endregion

namespace Spawn.HDT.DeckCodeBot.UI.Components
{
    public class OAuthTokenValidationRule : ValidationRule
    {
        #region Properties
        #region ErrorMessage
        public string ErrorMessage => $"Invalid value! Must begin with 'oauth:'.";
        #endregion
        #endregion

        #region Validate
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            ValidationResult retVal = ValidationResult.ValidResult;

            try
            {
                string strValue = value.ToString();

                if (!strValue.StartsWith("oauth:"))
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