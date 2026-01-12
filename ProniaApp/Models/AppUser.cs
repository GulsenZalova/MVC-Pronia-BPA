using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace ProniaApp.Models
{
    public class AppUser:IdentityUser 
    {
         public string Name {get;set;}
         public string Surname {get;set;}
    }
}