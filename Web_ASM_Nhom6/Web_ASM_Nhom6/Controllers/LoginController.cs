using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Web_ASM_Nhom6.Models;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.Extensions.Logging;

namespace Web_ASM_Nhom6.Controllers
{
    public class LoginController : Controller
    {
        string url = "http://localhost:29015/api/User";
        private readonly ILogger<LoginController> _logger;

        public LoginController(ILogger<LoginController> logger)
        {
            _logger = logger;
        }
        public async Task<IActionResult> Index()
        {
            List<User> users = new List<User>();
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(url);
            string apiResponse = await response.Content.ReadAsStringAsync();
            users = JsonConvert.DeserializeObject<List<User>>(apiResponse);

            return View(users);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            List<User> users = new List<User>();
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(url);
            string apiResponse = await response.Content.ReadAsStringAsync();
            users = JsonConvert.DeserializeObject<List<User>>(apiResponse);
            var isSuccsess = users.SingleOrDefault(a => a.Email.Equals(username) && a.Password.Equals(password));
            if (isSuccsess == null)
            {
                return View(users);
            }
            else if (isSuccsess.role.Equals("admin"))
            {
                return RedirectToAction("Index");
            }
            else if (isSuccsess.role.Equals("restaurant"))
            {
                //return RedirectToAction("RestaurantIndex");
            }
            return Ok();
        }
        
        public async Task SingInWithGoogle()
        {
            await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme, new AuthenticationProperties
            {
                RedirectUri = Url.Action("GoogleResponse")
            });
        }
        //public async Task SignInWithFacebook()
        //{
        //    await HttpContext.ChallengeAsync(FacebookDefaults.AuthenticationScheme, new AuthenticationProperties 
        //    { 
        //        RedirectUri = Url.Action("FacebookResponse")
        //    });
        //}

        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (!result.Succeeded)
            {
                return RedirectToAction("Login");
            }
            var emailClaim = result.Principal.FindFirst(ClaimTypes.Email);
            if (emailClaim == null)
            {
                return RedirectToAction("Login");
            }

            List<User> users = new List<User>();
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(url);
            string apiResponse = await response.Content.ReadAsStringAsync();
            users = JsonConvert.DeserializeObject<List<User>>(apiResponse);
            var isSuccsess = users.FirstOrDefault(u => u.Email == emailClaim.Value);

            if (isSuccsess == null)
            {
                return RedirectToAction("Login");
            }
            else if (isSuccsess.role.Equals("admin"))
            {
                return RedirectToAction("Index");
            }
            else if (isSuccsess.role.Equals("restaurant"))
            {
                //return RedirectToAction("RestaurantIndex");
            }
            return RedirectToAction("Login");
        }

        //public async Task<IActionResult> FacebookResponse()
        //{
        //    var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        //    if (!result.Succeeded)
        //    {
        //        return RedirectToAction("Login");
        //    }

        //    var claims = result.Principal.Identities
        //        .FirstOrDefault()?.Claims
        //        .Select(claim => new
        //        {
        //            claim.Issuer,
        //            claim.OriginalIssuer,
        //            claim.Type,
        //            claim.Value
        //        });


        //    // Handle user login in your system here

        //    return RedirectToAction("Index", "Home");
        //}
        public async Task SignInWithFacebook()
        {
            await HttpContext.ChallengeAsync(FacebookDefaults.AuthenticationScheme, new AuthenticationProperties
            {
                RedirectUri = Url.Action("FacebookResponse")
            });
        }

        public async Task<IActionResult> FacebookResponse()
        {
            var authenticateResult = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (!authenticateResult.Succeeded)
                return BadRequest(); // Xử lý lỗi đăng nhập

            // Xử lý kết quả và đăng nhập người dùng
            var claims = authenticateResult.Principal.Identities.FirstOrDefault()?.Claims.Select(claim => new
            {
                claim.Type,
                claim.Value
            });

            // Giả sử bạn có phương thức để đăng nhập người dùng với claims
            // await SignInUser(claims);

            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index");
        }
    }
}
