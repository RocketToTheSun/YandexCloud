using YandexCloud.CORE.DTOs;
using YandexCloud.INIT.Infrastructure;

namespace YandexCloud.INIT
{
    public class RequestReader : IRequestReader
    {
        readonly IConsoleReader _consoleReader;

        public RequestReader(IConsoleReader consoleReader)
        {
            _consoleReader = consoleReader;
        }

        public IEnumerable<RequestDataDto> Read()
        {
            string userAnswer;
            var dataList = new List<RequestDataDto>();

            do
            {
                var model = new RequestDataDto();

                var clientId = _consoleReader.ReadString("Введите id клиента");
                model.ClientId = Convert.ToInt32(clientId);

                model.ApiKey = _consoleReader.ReadString("Введите ключ api");
                model.From = _consoleReader.ReadDateTime("Введите дату начала");
                model.To = _consoleReader.ReadDateTime("Введите дату конца");

                dataList.Add(model);

                Console.WriteLine("Хотите добавить еще один магазин?");
                userAnswer = Console.ReadLine();
                if (userAnswer != "yes" && userAnswer != "no")
                {
                    do
                    {
                        Console.WriteLine("Попробуйте еще раз");
                        userAnswer = Console.ReadLine();
                    } while (userAnswer != "yes" && userAnswer != "no");
                }

            } while (userAnswer == "yes");

            return dataList;
        }
    }
}
