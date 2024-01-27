using System.ComponentModel.DataAnnotations;

namespace GameStore.Server.Models
{
    public class Game 
    {

        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public required String Name { get; set; }
       
        [Required] 
        [StringLength(20)]
        public String Genre { get; set; } = String.Empty;
       
        [Range(1,100)]
        public Decimal Price { get; set; }
        public DateTime ReleasedDate { get; set; }
    }
}