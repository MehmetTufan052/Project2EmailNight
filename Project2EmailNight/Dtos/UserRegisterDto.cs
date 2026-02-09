using System.ComponentModel.DataAnnotations;

namespace Project2EmailNight.Dtos
{
    public class UserRegisterDto
    {
        [Required(ErrorMessage = "Ad zorunludur")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Soyad zorunludur")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Email zorunludur")]
        [EmailAddress(ErrorMessage = "Geçerli bir email giriniz")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifre zorunludur")]
        [MinLength(6, ErrorMessage = "Şifre en az 6 karakter olmalı")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Şifre onayı zorunludur")]
        [Compare("Password", ErrorMessage = "Şifreler uyuşmuyor")]
        public string PasswordConfirm { get; set; }

        [Required(ErrorMessage = "Kullanıcı adı zorunludur")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Şartları kabul etmelisiniz")]
        public bool? AcceptTerms { get; set; }
    }
}
