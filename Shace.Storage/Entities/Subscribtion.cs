namespace Shace.Storage.Entities
{
    public class Subscribtion
    {
        [Key]
        [ForeignKey(nameof(Account))]
        public int AccountId { get; set; }

        /*[ForeignKey(nameof(AccountId))]
        public virtual Account Account { get; set; }*/


        [Required]
        public int Account2Id { get; set; }

        [ForeignKey(nameof(Account2Id))]
        public virtual Account Account2 { get; set; }

        public virtual Account Account { get; set; }
    }
}
