using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HTTPServer
{
    public record class ServerSettings
    {
        public int Port { get; init; } = 8080;

        public string Path { get; init; } = @"./static/";

        public static ServerSettings GetServerSettingsFromFile() 
        {
            var i = JsonSerializer.Deserialize<ServerSettings>(File.ReadAllBytes("./settings.json"));
            return i == null ? throw new JsonException("Ошибка в settings.json!") : i;
        }
    }
}
