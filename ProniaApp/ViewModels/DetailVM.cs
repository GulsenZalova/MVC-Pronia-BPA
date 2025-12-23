using ProniaApp.Models;

namespace ProniaApp.ViewModels
{
    public class DetailVM
    {
          public Product Product { get; set; }
          public List<Product> RelatedProducts  { get; set; }
          
    }
}