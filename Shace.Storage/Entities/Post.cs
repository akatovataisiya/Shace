namespace Shace.Storage.Entities
{
    public class Post
    {
        [Key]
        public int Id { get; set; }

        public string Photo { get; set; }

        public DateTime PostDate { get; set; }

        public string? Description { get; set; }

        public string? Location { get; set; }

        public int? LikeCounter { get; set; }

        public int? CommentCounter { get; set; }

        public int? MarkCounter { get; set; }

        public int? PostType { get; set; }

        [Required]
        public int AccountId { get; set; }

        [ForeignKey(nameof(AccountId))]
        public virtual Account Account { get; set; }
    }
}
