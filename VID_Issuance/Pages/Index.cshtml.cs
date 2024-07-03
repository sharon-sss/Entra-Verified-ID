using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace VID_Issuance.Pages
{
    public class IndexModel : PageModel
    {
        private IConfiguration _configuration;
        public IndexModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void OnGet()
        {
            ViewData["AzureAd:ClientId"] = _configuration["AzureAd:ClientId"];

            if (User.Identity.IsAuthenticated)
            {
                ViewData["username"] = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "preferred_username")?.Value;
                string email = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                string given_name = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName)?.Value;
                string family_name = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Surname)?.Value;
                string Mobile = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Mobile")?.Value;
                string Country = "United Arab Emirates"; //HttpContext.User.Claims.FirstOrDefault(c => c.Type == "ctry")?.Value; 
                string Department = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Department")?.Value;
                string JobTitle = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "JobTitle")?.Value;
                if (string.IsNullOrWhiteSpace(given_name))
                {
                    ViewData["given_name"] = "(not available in ID token)";
                }
                else
                {
                    ViewData["given_name"] = given_name;
                }
                if (string.IsNullOrWhiteSpace(JobTitle))
                {
                    ViewData["JobTitle"] = "(not available in ID token)";
                }
                else
                {
                    ViewData["JobTitle"] = JobTitle;
                }
                if (string.IsNullOrWhiteSpace(family_name))
                {
                    ViewData["family_name"] = "(not available in ID token)";
                }
                else
                {
                    ViewData["family_name"] = family_name;

                }
                if(string.IsNullOrWhiteSpace(email))
                {
                    ViewData["email"] = "Not Available in token"; 
                }
                else
                {
                    ViewData["email"] = email;
                }
                if (string.IsNullOrWhiteSpace(Mobile))
                {
                    ViewData["Mobile"] = "(not available in ID token)";
                }
                else
                {
                    ViewData["Mobile"] = Mobile;
                }
                if (string.IsNullOrWhiteSpace(Country))
                {
                    ViewData["Country"] = "(not available in ID token)";
                }
                else
                {
                    ViewData["Country"] = Country;
                }
                if (string.IsNullOrWhiteSpace(Department))
                {
                    ViewData["Department"] = "(not available in ID token)";
                }
                else
                {
                    ViewData["Department"] = Department;
                }
            }
        }
    }
}
