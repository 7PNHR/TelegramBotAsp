using System;
using System.Text.Json.Serialization;

namespace TelegramBotAsp.Entities
{
    public class BaseEntity
    {
        [JsonIgnore] public long Id { get; set; }

        [JsonIgnore] public DateTime Date { get; set; } = DateTime.UtcNow;
    }
}