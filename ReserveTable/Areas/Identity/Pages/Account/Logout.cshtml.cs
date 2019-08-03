namespace ReserveTable.App.Areas.Identity.Pages.Account
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Domain;

    [AllowAnonymous]
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<ReserveTableUser> _signInManager;

        public LogoutModel(SignInManager<ReserveTableUser> signInManager)
        {
            _signInManager = signInManager;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();
            return this.Redirect("/");
        }
    }
}