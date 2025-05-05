namespace Aviate.Infrastructure.Auth
{
    public class JwtOptions
    {
        public string Secretkey { get; set; } = string.Empty;
        public int ExpitesHours { get; set; }
    }
}