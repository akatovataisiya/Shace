namespace Shace.Storage.Entities
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        public string Text { get; set; }

        public DateTime CommentDate { get; set; }

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
