using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyTrackDatabaseAPI.Models
{
    public class Post
    {
                [Key]
                public int Id { get; set; }
                [Required, MaxLength(256)]
                public string Title { get; set; }
                [Required,ForeignKey("Restaurant")]
                public int RestaurantId { get; set; }
                [Required]
                public string Content { get; set; }
                [Required]
                public string ImageUrl { get; set; }
                [Required]
                public int AuthorId { get; set; }
                [Required]
                public int TasteScore { get; set; }
                [Required]
                public int TextureScore { get; set; }
                [Required]
                public int VisualScore { get; set; }
    }
}