using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

namespace TelegramBot
{
    class Program
    {   
        static ITelegramBotClient _botClient;
       
        static void Main(string[] args)
        {
            _botClient = new TelegramBotClient("5689617707:AAEI-F3GE7m5U28PfXJIaPp0bADafZAVflM");
            var me = _botClient.GetMeAsync().Result;
            Console.WriteLine($"Hi, I am {me.Id} and my name is:  {me.FirstName}");

            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = new UpdateType[]
                {
                    UpdateType.Message,
                    UpdateType.EditedMessage,
                }
            };

            _botClient.StartReceiving(UpdateHandler, ErrorHandler, receiverOptions);

            Console.ReadLine();
        }

        private static Task ErrorHandler(ITelegramBotClient arg1, Exception arg2, CancellationToken arg3)
        {
            throw new NotImplementedException();
        }

        private static async Task UpdateHandler(ITelegramBotClient bot, Telegram.Bot.Types.Update update, CancellationToken arg3)
        {
           if(update.Type == UpdateType.Message)
            {
                if(update.Message.Type == MessageType.Text)
                {
                    var text = update.Message.Text;
                    var id = update.Message.Chat.Id;
                    string? username = update.Message.Chat.Username;

                    string publicIP = GetPublicIP();

                    Console.WriteLine($"{username} | {id} | {text} ip: {publicIP}");
                    await _botClient.SendTextMessageAsync(id, $"Tu ip publica es: {publicIP}", parseMode: ParseMode.Html);
                }
            }
       }

        public static string GetPublicIP()
        {
            string url = "http://checkip.dyndns.org";
            System.Net.WebRequest req = System.Net.WebRequest.Create(url);
            System.Net.WebResponse resp = req.GetResponse();
            System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
            string response = sr.ReadToEnd().Trim();
            string[] a = response.Split(':');
            string a2 = a[1].Substring(1);
            string[] a3 = a2.Split('<');
            string a4 = a3[0];
            return a4;
        }

    }
}
