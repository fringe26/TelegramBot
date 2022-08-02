using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using TelegramBot.VoiceToText;
using TelegramBot.VoiceToText.Configuration;
using TelegramBot.VoiceToText.Controllers;
using TelegramBot.VoiceToText.Services;

Console.OutputEncoding = System.Text.Encoding.Unicode;

// Объект, отвечающий за постоянный жизненный цикл приложения
var host = new HostBuilder()
    .ConfigureServices((hostContext, services) => ConfigureServices(services))  // Задаем конфигурацию
    .UseConsoleLifetime()  // Позволяет поддерживать приложение активным в консоли
    .Build();   // Собираем

Console.WriteLine("Сервис запущен");
// Запускаем сервис
await host.RunAsync();
Console.WriteLine("Сервис остановлен");


static void ConfigureServices(IServiceCollection services)
{
    AppSettings appSettings = BuildAppSettings();
    services.AddSingleton(BuildAppSettings());

    // Подключаем контроллеры сообщений и кнопок
    services.AddTransient<DefaultMessageController>();
    services.AddTransient<InlineKeyboardController>();
    services.AddTransient<TextMessageController>();
    services.AddTransient<VoiceMessageController>();

    // Подключаем сервис отвечающий за хранения сессий (вместо БД )
    services.AddSingleton<IStorage, MemoryStorage>();
    //Voice Message Convertor Service Added
    services.AddTransient<IFileHandler, AudioFileHandler>();

    // Регистрируем объект TelegramBotClient c токеном подключения
    services.AddSingleton<ITelegramBotClient>(provider => new TelegramBotClient(appSettings.BotToken));
    // Регистрируем постоянно активный сервис бота
    services.AddHostedService<Bot>();
}


static AppSettings BuildAppSettings()
{
    return new AppSettings()
    {
        DownloadsFolder = @"D:\Downloads(D)\TelegramBotVoices",
        BotToken = "5433500272:AAHIgGvJpvSxQgCp0GxeFEUOBqz-9INCY1o",
        AudioFileName = "audio",
        InputAudioFormat = "ogg",
        OutputAudioFormat = "wav"
    };
}