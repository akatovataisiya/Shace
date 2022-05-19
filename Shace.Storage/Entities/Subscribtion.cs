namespace Shace.Storage.Entities
{
    public class Subscribtion
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey(nameof(Account))]
        public int AccountId { get; set; }

        [Required]

        [ForeignKey(nameof(Account2))]
        public int Account2Id { get; set; }

        public virtual Account Account2 { get; set; }

        public virtual Account Account { get; set; }
    }
}
