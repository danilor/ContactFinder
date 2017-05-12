using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Common.Utilities
{
    public class Validation
    {
        public static bool ValidEmail(String email)
        {
            /**
                * We are using the System Net Mail address to validate the email.
                * It will throw an error if the email is invalid
                * */
            try
            {
                new System.Net.Mail.MailAddress( email );
                return true;
            }
            catch (ArgumentException)
            {
                return false;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
