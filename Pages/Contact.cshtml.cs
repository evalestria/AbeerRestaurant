using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

public class ContactModel : PageModel
{
    [BindProperty]
    public ContactFormModel ContactForm { get; set; } = new ContactFormModel();

    public void OnGet()
    {
    }

    public class ContactFormModel
    {
        [Required]
        public string Name { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Message { get; set; }
    }
}
