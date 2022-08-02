using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBot.VoiceToText.Controllers
{
    internal class DefaultMessageController
    {
        private readonly ITelegramBotClient _telegramClient;

        public DefaultMessageController(ITelegramBotClient telegramClient)
        {
            _telegramClient = telegramClient;
        }

        public async Task Handle(Message message, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Controller {GetType().Name} get message");

            await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"Получено сообщение не поддерживаемого формата", cancellationToken: cancellationToken);
        }
    }
}
