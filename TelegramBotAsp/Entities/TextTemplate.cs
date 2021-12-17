namespace TelegramBotAsp.Entities
{
    public class TextTemplate : BaseEntity
    {
        public string Request { get; set; }
        public string Response { get; set; }
        public string TopicName { get; set; }
    }
}