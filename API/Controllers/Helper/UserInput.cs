using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Controllers.Helper
{
    /// <summary>
    /// Helper for valid user input
    /// </summary>
    public static class UserInput
    {
        /// <summary>
        /// Input string value and convert to valid Float value
        /// </summary>
        /// <param name="input">input string delegate</param>
        /// <returns>Converted Float value</returns>
        public static float ValidFloat(Func<string> input)
        {
            float number;
            string str = input?.Invoke();
            while (!float.TryParse(str, out number))
            {
                Console.Write("Ви ввели не валідне значення. Спробуйте ще раз: ");
                str = input.Invoke();
            }

            return number;
        }

        /// <summary>
        /// Input string value and convert to valid Int value
        /// </summary>
        /// <param name="input">input string delegate</param>
        /// <returns>Converted Int value</returns>
        public static int ValidInt(Func<string> input)
        {
            int number;
            string str = input?.Invoke();
            while (!int.TryParse(str, out number))
            {
                Console.Write("Ви ввели не валідне значення для числа. Спробуйте ще раз: ");
                str = input?.Invoke();
            }

            return number;
        }

        /// <summary>
        /// Input string value and convert to DateTime value
        /// </summary>
        /// <param name="input">input string delegate</param>
        /// <returns>Converted DateTime value</returns>
        public static DateTime ValidDateTime(Func<string> input)
        {
            DateTime date = DateTime.Today;
            string str = input?.Invoke();
            while (!DateTime.TryParse(str, out DateTime newDate) ||
                   (DateTime.TryParse(str, out newDate) && newDate.Year < 1800 || newDate > DateTime.Now))
            {
                Console.Write("Ви ввели не валідне значення для дати. Спробуйте ще раз: ");
                str = input?.Invoke();
                date = newDate;
            }

            return date;
        }

        /// <summary>
        /// Input not empty string value
        /// </summary>
        /// <param name="input">input string delegate</param>
        /// <returns>Not empty string value</returns>
        public static string NotEmptyString(Func<string> input)
        {
            string str = input?.Invoke();
            while (string.IsNullOrWhiteSpace(str))
            {
                if (string.IsNullOrWhiteSpace(str))
                {
                    Console.Write("Ви ввели порожню строку. Спробуйте ще раз: ");
                }

                str = input?.Invoke();
            }

            return str;
        }
    }
}
