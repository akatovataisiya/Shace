namespace Shace.Storage.Entities
{
    public class Advertisment
    {
        [Key]
        public int Id { get; set; }
        
        public string? Description { get; set; }

        public string Url { get; set; }

        public string UrlTo { get; set; }

        [Required]
        public int AccountId { get; set; }

        [ForeignKey(nameof(AccountId))]
        public virtual Account Account { get; set; }
    }
}
