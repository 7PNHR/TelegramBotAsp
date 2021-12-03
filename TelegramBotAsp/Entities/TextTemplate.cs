namespace TelegramBotAsp.Entities
{
    public class TextTemplate : BaseEntity
    {
        public Topic Topic { get; set; }
        public string Text { get; set; }
        public string Template { get; set; }
    }
}