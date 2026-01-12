using System.ComponentModel.DataAnnotations;

namespace ProniaApp.ViewModels

{
    public class LoginVM
    {
        [MinLength(3, ErrorMessage = "qisadir")]
        [MaxLength(20, ErrorMessage = "uzundur")]
        public string NameOrEmail { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool IsPersisted { get; set;}
    }
}