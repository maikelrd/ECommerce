namespace ECommerce.Data.Entities
{
    public class Card
    {
        public string Email { get; set; }
        public int CardId { get; set; }
        public string CardNumber { get; set; }
        public string NameOnCard { get; set; }
        public int ExpMonth { get; set; }
        public int ExpYear { get; set; }
        public bool DefaultPMethod { get; set; } 
    }
}
