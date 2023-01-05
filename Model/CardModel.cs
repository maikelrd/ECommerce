using System.ComponentModel.DataAnnotations;

namespace ECommerce.Model
{
    public class CardModel
    {
        [Required]
        public string Email { get; set; }
        public int CardId { get; set; }
        [Required]
        public string CardNumber { get; set; }
        [Required]
        public string NameOnCard { get; set; }
        [Required]
        public int ExpMonth { get; set; }
        [Required]
        public int ExpYear { get; set; }
        public bool DefaultPMethod { get; set; }
    }
}
