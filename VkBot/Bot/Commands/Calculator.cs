using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using VkBot.Data.Abstractions;
using VkNet.Abstractions;
using VkNet.Model;

namespace VkBot.Bot.Commands
{
    public class Calculator : IBotCommand
    {
        public string[] Alliases { get; set; } = { "калькулятор", "посчитай" };
        private readonly IVkApi _vkApi;

        public Calculator(IVkApi api)
        {
            _vkApi = api;
        }

        public async Task<string> Execute(Message msg)
        {
            var user = (await _vkApi.Users.GetAsync(new[] { msg.FromId.Value })).FirstOrDefault();

            var split = msg.Text.Split(' ', 2); // [команда, параметры]
            var expression = split[1].Trim();

            var a = new DataTable();
            try
            {
                var answer = Convert.ToString(a.Compute(expression, ""));
                return $"{user.FirstName} {user.LastName}, ответ вашего выражения = {answer}";
            }
            catch(Exception)
            {
                return $"{user.FirstName} {user.LastName}, я не смог посчитать это... =(";
            }
        }
    }
}