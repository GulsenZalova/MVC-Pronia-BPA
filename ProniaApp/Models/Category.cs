using System.ComponentModel.DataAnnotations;

namespace ProniaApp.Models
{
    public class Category:BaseEntity
    {
         [MaxLength(30, ErrorMessage = "Input uzunlugu 30dan artiq ola bilmez")]
         public string Name { get; set; }
         public List<Product>? Products { get; set; }
    }
}