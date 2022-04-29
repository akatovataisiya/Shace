namespace Shace.Storage.Entities
{
    public class Message
    {
        [Key]
        public int Id { get; set; }

        public string Text { get; set; }

        public DateTime MessageDate { get; set; }

        [Required]
        public int AccountId { get; set; }

        [Required]
        public int DialogId { get; set; }

        [ForeignKey(nameof(AccountId))]
        public virtual Account Account { get; set; }

        [ForeignKey(nameof(DialogId))]
        public virtual Dialog Dialog { get; set; }
    }
}
