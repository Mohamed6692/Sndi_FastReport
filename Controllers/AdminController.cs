using Identity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ActeAdministratif.Areas.Identity.Data;
using ActeAdministratif.Controllers;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace Identity.Controllers
{
   
    public class AdminController : Controller
    {   
        private UserManager<SNDIUser> userManager;
        private IPasswordHasher<SNDIUser> passwordHasher;
        private readonly ILogger<AdminController> _logger;
    

        //public HomeController(ILogger<HomeController> logger, UserManager<SNDIUser> userManager)
        //{
        //    _logger = logger;
        //    this._userManager = userManager;
        //}

        public AdminController(ILogger<AdminController> logger,UserManager<SNDIUser> usrMgr, IPasswordHasher<SNDIUser> passwordHash)
        {
            userManager = usrMgr;
            passwordHasher = passwordHash;
            _logger = logger;
        }
 
        public IActionResult Index()
        {
            return View(userManager.Users);
        }

        private string GenerateRandomPassword(int length)
        {
            const string upperChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string lowerChars = "abcdefghijklmnopqrstuvwxyz";
            const string digitChars = "0123456789";
            const string specialChars = "!@#$%^&*()_-+=<>?";

            StringBuilder password = new StringBuilder();
            Random random = new Random();

            // Assurez-vous d'avoir au moins un caractère de chaque catégorie
            password.Append(upperChars[random.Next(upperChars.Length)]);
            password.Append(lowerChars[random.Next(lowerChars.Length)]);
            password.Append(digitChars[random.Next(digitChars.Length)]);
            password.Append(specialChars[random.Next(specialChars.Length)]);

            // Complétez le reste de la longueur du mot de passe
            for (int i = 4; i < length; i++)
            {
                string randomChars = upperChars + lowerChars + digitChars + specialChars;
                char randomChar = randomChars[random.Next(randomChars.Length)];
                password.Append(randomChar);
            }

            // Mélangez les caractères du mot de passe pour plus de sécurité
            for (int i = 0; i < length; i++)
            {
                int swapIndex = random.Next(length);
                char temp = password[i];
                password[i] = password[swapIndex];
                password[swapIndex] = temp;
            }

            return password.ToString();
        }


        public ViewResult Create()
        {
            return View();
           
        }


        //public ViewResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(User user)
        {
            if (ModelState.IsValid)
            {
                string randomPassword = GenerateRandomPassword(8);
                SNDIUser appUser = new SNDIUser
                {
                    UserName = user.Email, 
                    Email = user.Email,
                    PhoneNumber =user.PhoneNumber,
                    PassGenerate = randomPassword
                     
                    //PassGenerate=user.PassGenerate
                };
                
                IdentityResult result = await userManager.CreateAsync(appUser, appUser.PassGenerate);
                //user.password
                if (result.Succeeded)
                    return RedirectToAction("Index");
                else
                {
                    foreach (IdentityError error in result.Errors)
                        ModelState.AddModelError("", error.Description);
                }
            }
            return View(user);
        }

        public async Task<IActionResult> Update(string id)
        {
            SNDIUser user = await userManager.FindByIdAsync(id);
            if (user != null)
                return View(user);
            else
                return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Update(string id, string email, string password)
        {
            SNDIUser user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                if (!string.IsNullOrEmpty(email))
                    user.Email = email;
                else
                    ModelState.AddModelError("", "Email cannot be empty");

                if (!string.IsNullOrEmpty(password))
                    user.PasswordHash = passwordHasher.HashPassword(user, password);
                else
                    ModelState.AddModelError("", "Password cannot be empty");

                if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
                {
                    IdentityResult result = await userManager.UpdateAsync(user);
                    if (result.Succeeded)
                        return RedirectToAction("Index");
                    else
                        Errors(result);
                }
            }
            else
                ModelState.AddModelError("", "User Not Found");
            return View(user);
        }



        //Changer password par le user connecté:
        public async Task<IActionResult> ChangePassword()
        {
            // Récupérez l'utilisateur connecté
            var user = await userManager.GetUserAsync(User);

            if (user != null)
            {
                return View(user);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(SNDIUser model)
        {
            // Récupérez l'utilisateur connecté
            var user = await userManager.GetUserAsync(User);

            if (user != null)
            {
                // Mettez à jour l'e-mail si un nouvel e-mail est fourni
                if (!string.IsNullOrEmpty(model.Email))
                {
                    user.Email = model.Email;
                }

                // Mettez à jour le mot de passe si un nouveau mot de passe est fourni
                if (!string.IsNullOrEmpty(model.PasswordHash))
                {
                    // Utilisez UserManager pour mettre à jour le mot de passe de l'utilisateur
                    var result = await userManager.ChangePasswordAsync(user, null, model.PasswordHash);

                    if (!result.Succeeded)
                    {
                        // Gérez les erreurs de mise à jour du mot de passe
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }

                        return View(user);
                    }
                }

                // Mettez à jour l'utilisateur
                var updateResult = await userManager.UpdateAsync(user);

                if (updateResult.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    // Gérez les erreurs de mise à jour de l'utilisateur
                    foreach (var error in updateResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }

                    return View(user);
                }
            }
            else
            {
                return RedirectToAction("Index");
            }
        }





        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            SNDIUser user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult result = await userManager.DeleteAsync(user);
                if (result.Succeeded)
                    return RedirectToAction("Index");
                else
                    Errors(result);
            }
            else
                ModelState.AddModelError("", "User Not Found");
            return View("Index", userManager.Users);
        }

        ////Logout
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Logout()
        //{
        //    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        //    // Redirigez l'utilisateur vers une page d'accueil ou une autre page après la déconnexion
        //    return RedirectToAction("/Home/");
        //}




        private void Errors(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
                ModelState.AddModelError("", error.Description);
        }
    }
}