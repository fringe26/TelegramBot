using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBot.VoiceToText.Configuration;
using TelegramBot.VoiceToText.Services;

namespace TelegramBot.VoiceToText.Controllers
{
    public class VoiceMessageController
    {
        private readonly AppSettings _appSettings;
        private readonly ITelegramBotClient _telegramClient;
        private readonly IFileHandler _audioFileHandler;
        private readonly IStorage _memoryStorage;

        public VoiceMessageController(AppSettings appSettings, ITelegramBotClient telegramBotClient, IFileHandler audioFileHandler, IStorage memoryStorage)
        {
            _appSettings = appSettings;
            _telegramClient = telegramBotClient;
            _audioFileHandler = audioFileHandler;
            _memoryStorage = memoryStorage;
        }

        public async Task Handle(Message message, CancellationToken ct)
        {
            var fileId = message.Voice?.FileId;
            if (fileId == null)
                return;

            await _audioFileHandler.Download(fileId, ct);

            await _telegramClient.SendTextMessageAsync(message.Chat.Id, "Голосовое сообзщение загружено", cancellationToken: ct);

            string userLanguageCode = _memoryStorage.GetSession(message.Chat.Id).LanguageCode; // Здесь получим язык из сессии пользователя
            var result = _audioFileHandler.Process(userLanguageCode); // Запустим обработку

            await _telegramClient.SendTextMessageAsync(message.Chat.Id, result, cancellationToken: ct);
            //await _telegramClient.SendTextMessageAsync(message.Chat.Id, "Голосовое сообщение конвертировано в формат .WAV", cancellationToken: ct);

        }


        //public async Task Handle(Message message, CancellationToken ct)
        //{
        //    Console.WriteLine($"Controller {GetType().Name} get voice message");

        //    await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"Получено голосовое сообщение", cancellationToken: ct);
        //}
    }
}
