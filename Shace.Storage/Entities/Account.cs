namespace Shace.Storage.Entities
{
    public class Account
    {
        [Key]
        public int Id { get; set; }

        public string? ShortName { get; set; }

        public string? Url { get; set; }

        public string? Photo { get; set; }

        public bool? Privacy { get; set; }

        public long? Phone { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public DateTime? BDay { get; set; }
        
        public DateTime RegDay { get; set; }

        public string? Description { get; set; }

        public string? Location { get; set; }

        public int? SubscibersCounter { get; set; }

        public int? SubscriptionsCounter { get; set; }
    }
}
