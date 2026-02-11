using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace Project2EmailNight.Dtos
{
    public class UserEditDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Password { get; set; }

        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }

        [BindNever]
        public string ImageUrl { get; set; }
        public IFormFile Image { get; set; }

        public int DraftCount { get; set; }
        public int StarredCount { get; set; }
    }
}
