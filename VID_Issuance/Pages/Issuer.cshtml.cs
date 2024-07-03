using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace VID_Issuance.Pages
{
    public class IssuerModel : PageModel
    {
        private readonly ILogger<IssuerModel> _logger;
        public IssuerModel(ILogger<IssuerModel> logger)
        {
            _logger = logger;
        }
        public void OnGet()
        {
        }
    }
}
