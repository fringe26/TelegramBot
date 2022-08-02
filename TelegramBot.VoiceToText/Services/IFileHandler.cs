using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot.VoiceToText.Services
{
    public interface IFileHandler
    {

        Task Download(string fileId, CancellationToken ct);
        string Process(string param);
    }
}
