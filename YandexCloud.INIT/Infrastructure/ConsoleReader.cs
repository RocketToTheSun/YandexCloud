using System.Globalization;
using System.Text.RegularExpressions;

namespace YandexCloud.INIT.Infrastructure
{
    public class ConsoleReader : IConsoleReader
    {
        public DateTime ReadDateTime(string message)
        {
            DateTime result;
            bool isMatch;
            string fromRequest;

            do
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(message + ": ");
                fromRequest = Console.ReadLine();
                var regEx = new Regex(@"\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}.000Z");
                isMatch = regEx.IsMatch(fromRequest);

                if (!isMatch)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Попробуйте еще раз");
                }

            } while (!isMatch);

            result = DateTime.Parse(fromRequest, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);
            return result;
        }

        public string ReadString(string message)
        {
            string fromRequest;

            do
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(message + ": ");
                fromRequest = Console.ReadLine();

                if (string.IsNullOrEmpty(fromRequest))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Попробуйте еще раз");
                }

            } while (string.IsNullOrEmpty(fromRequest));

            return fromRequest;
        }
    }
}
