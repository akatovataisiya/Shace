namespace Shace.Storage.Entities
{
    public class Dialog
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int AccountId { get; set; }

        [ForeignKey(nameof(AccountId))]
        public virtual Account Account { get; set; }
    }
}
