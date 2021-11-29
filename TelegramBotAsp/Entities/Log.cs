namespace TelegramBotAsp.Entities
{
    public class Log : BaseEntity
    {
        public string Message { get; set; }
        public AppUser User { get; set; }
    }
}