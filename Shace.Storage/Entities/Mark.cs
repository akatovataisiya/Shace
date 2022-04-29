namespace Shace.Storage.Entities
{
    public class Mark
    {
        [Key]
        public int Id { get; set; }


        [Required]
        public int AccountId { get; set; }

        [Required]
        public int PostId { get; set; }

        [ForeignKey(nameof(AccountId))]
        public virtual Account Account { get; set; }

        [ForeignKey(nameof(PostId))]
        public virtual Post Post { get; set; }
    }
}
