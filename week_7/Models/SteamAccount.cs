﻿namespace HTTPServer.Models
{
    public record class SteamAccount
    {
        public string Login { get; init; }
        public string Password { get; init; }

        public override string ToString()
        {
            return $"Аккаунт Steam: (Логин: {Login}, Пароль: {Password})";
        }
    }
}
