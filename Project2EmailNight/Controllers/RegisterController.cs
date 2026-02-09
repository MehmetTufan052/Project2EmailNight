using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using Project2EmailNight.Dtos;
using Project2EmailNight.Entities;

namespace Project2EmailNight.Controllers
{
    public class RegisterController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public RegisterController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult CreateUser()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateUser(UserRegisterDto userRegisterDto)
        {
            if (!ModelState.IsValid)
            {
                return View(userRegisterDto);
            }

            Random random = new Random();
            int confirmCode = random.Next(100000, 999999);

            AppUser appUser = new AppUser()
            {
                Name = userRegisterDto.Name,
                Surname = userRegisterDto.Surname,
                UserName = userRegisterDto.Username,
                Email = userRegisterDto.Email,
                ConfirmCode = confirmCode,
                EmailConfirmed = false

            };

            var result = await _userManager.CreateAsync(appUser, userRegisterDto.Password);

            if (result.Succeeded)
            {
                MimeMessage mimeMessage = new MimeMessage();
                mimeMessage.From.Add(new MailboxAddress("Syndash", "tufaneser8@gmail.com"));
                mimeMessage.To.Add(new MailboxAddress("User", userRegisterDto.Email));
                mimeMessage.Subject = "Syndash Email Doğrulama Kodu";

                BodyBuilder bodyBuilder = new BodyBuilder();
                bodyBuilder.HtmlBody =
                $@"
                <h3>Syndash Email Doğrulama</h3>
                <p>Doğrulama kodunuz:</p>
                <h2>{confirmCode}</h2>
                <p><b>Bu kodu kimseyle paylaşmayın.</b></p>";

                mimeMessage.Body = bodyBuilder.ToMessageBody();

                using (SmtpClient smtpClient = new SmtpClient())
                {
                    smtpClient.Connect("smtp.gmail.com", 587, false);
                    smtpClient.Authenticate("tufaneser8@gmail.com", "rghp xhld nktr pxmb");
                    smtpClient.Send(mimeMessage);
                    smtpClient.Disconnect(true);
                }

                TempData["MailInfo"] = "Doğrulama kodunuz email adresinize gönderildi.";
                return RedirectToAction("EmailConfirm");
            }

            foreach (var item in result.Errors)
            {
                ModelState.AddModelError("", item.Description);
            }

            return View(userRegisterDto);
        }

        [HttpGet]
        public IActionResult EmailConfirm()
        {
            return View();
        }
        
        
        [HttpPost]
        public async Task<IActionResult> EmailConfirm(string Email, int ConfirmCode)
        {
            Console.WriteLine("POST EmailConfirm çalıştı");

            var user = await _userManager.FindByEmailAsync(Email);

            if (user == null)
            {
                ModelState.AddModelError("", "Kullanıcı bulunamadı.");
                return View();
            }

            if (user.ConfirmCode != ConfirmCode)
            {
                ModelState.AddModelError("", "Doğrulama kodu hatalı.");
                return View();
            }

            user.EmailConfirmed = true;
            user.ConfirmCode = null;

            await _userManager.UpdateAsync(user);

            return RedirectToAction("Index", "Login");
        }


    }
}
