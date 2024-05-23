using Microsoft.Extensions.FileSystemGlobbing.Internal;
using System.Text.RegularExpressions;
using System.Xml.Linq;
namespace MoviesE_commerce.Models
{
    public class Validation
    {
        public bool isValidName(string name)
        {
            if (name.Length<12 &&(nameExpression(name)))
            {
                return true;
            }
            return false;
        }
        public static bool nameExpression(string name)
        {
            string pattern = @"^[a-zA-Z']+$";
            return ValidateRegex(name, pattern);
        }

        public bool isValidEmail(string email)
        {
            string pattern = @"^\w+@gmail.com$";
            return ValidateRegex(email, pattern);
        }

        public bool isValidPassword(string pass)
        {
            int Capital = 0;
            for (int i = 0; i < pass.Length; i++)
            {
                if (pass[i] == ' ') 
                    return false; 
                if (pass[i] >= 65 && pass[i] <= 91) Capital++;
            }
            return Capital > 3 && pass.Length >= 8;
        }

        public bool isValidPhone(string phoneNumber)
        {
            string pattern = @"^(01[0-2]|015)[0-9]{8}$";
             return ValidateRegex(phoneNumber, pattern);
        }
        static bool ValidateRegex(string input, string pattern)
        {
            Regex regex = new Regex(pattern);
            return regex.IsMatch(input);
        }
    }
}
