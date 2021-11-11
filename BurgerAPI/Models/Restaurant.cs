using System.ComponentModel.DataAnnotations;

namespace MoneyTrackDatabaseAPI.Models
{
    public class Restaurant
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(256)]
        public string Name { get; set; }
        [Required]
        public double Latitude { get; set; }
        [Required]
        public double Longitude { get; set; }
        [Required]
        public string OpeningTimes { get; set; }
    }
}