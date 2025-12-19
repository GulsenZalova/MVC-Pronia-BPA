namespace ProniaApp.Models
{
    public class Category:BaseEntity
    {
         public string Name { get; set; }
         public Product Products { get; set; }
    }
}