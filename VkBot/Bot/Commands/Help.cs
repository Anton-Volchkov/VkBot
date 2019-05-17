using System.Text;
using System.Threading.Tasks;
using VkBot.Data.Abstractions;
using VkNet.Model;

namespace VkBot.Bot.Commands
{
    public class Help : IBotCommand
    {
        // public string Name { get; set; } = "Помощь";
        public string[] Alliases { get; set; } = { "команды", "помоги", "хелп" };
        public string Description { get; set; } = "Команда !Бот команды возвращает вам список доступных команд." +
                                                  "\nПример: !Бот команды ";

        public Task<string> Execute(Message msg)
        {
            return Task.Run(() =>
            {
                var strBuilder = new StringBuilder();
                strBuilder.AppendLine("***КОМАНДЫ БОТА****");
                strBuilder.AppendLine("Перед каждой командой нужно ставить восклицательный знак.Пример: !Команда");
                strBuilder.AppendLine("_____________").AppendLine();
                strBuilder.AppendLine("!Бот команды");
                strBuilder.AppendLine("!Бот подписка + название города (подписывает вас на рассылку погоды по утрам для данного города)");
                strBuilder.AppendLine("!Бот отписка (отписывает от рассылки погоды по утрам)");
                strBuilder.AppendLine("!Бот вики + заголовок запроса (возвращает ответ по запросу с Wikipedia)");
                strBuilder.AppendLine("!Бот личное [в квадратных скобках указать что запомнить боту])");
                strBuilder.AppendLine("!Бот память (выведет данные которые вы просили запомнить)");
                strBuilder.AppendLine("!Бот забудь (забудет введенные данные)");
                strBuilder.AppendLine("!Бот погода + название города");
                strBuilder.AppendLine("!Бот курс + (USD, EUR, RUR, UAH)");
                strBuilder.AppendLine("!Бот конвертируй + имя валюты (USD, EUR, RUR, UAH) + сколько(100,200,150)");
                strBuilder.AppendLine("!Бот посчитай + выражение которое нужно посчитать");
                strBuilder.AppendLine("!Бот любовь + с кем или с чем проверить процент любви");
                strBuilder.AppendLine("!Бот звонок");
                strBuilder.AppendLine("!Бот бицепсметр");
                strBuilder.AppendLine("!Бот рандом");
                strBuilder.AppendLine("!Бот рулетка");
                strBuilder.AppendLine("!Бот запомни + переслать сообщение которое нужно запомнить (оно будет общее для всех пользователей)");
                strBuilder.AppendLine("!Бот общее (сообщение которое запомнили при помощи !бот запомни)").AppendLine();
                strBuilder.AppendLine("!Бот перевод c языка - на язык Текст");
                strBuilder.AppendLine("Пример: !Бот перевод ru-en Привет\n(Переведёт с Русского на Английский текст 'Привет')");
                strBuilder.AppendLine("_____________");

                return strBuilder.ToString();
            });
        }
    }
}