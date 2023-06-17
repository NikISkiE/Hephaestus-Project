using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Build.Framework;
using Microsoft.Data.SqlClient;
using NuGet.Protocol.Plugins;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Security.Claims;
using Hephaestus_Project.Models;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace Hephaestus_Project.Pages.Login
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public Credential credential { get; set; }
        public AccountInfo CredentialSQL = new AccountInfo();
        private readonly IConfiguration Configuration;

        public IndexModel(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync() 
        {
            if (!ModelState.IsValid) return Page();
            //SQL Connection
            try
            {
                var constring = Configuration["ConnectionStrings:DefaultString"];

                using (SqlConnection con = new SqlConnection(constring))
                {
                    con.Open();
                    String sql = "SELECT login,password,permlvl FROM AccountData where login = '"+credential.Login+"'";
                    using(SqlCommand command= new SqlCommand(sql, con))
                    {
                        using(SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                CredentialSQL.Login = reader.GetString(0);
                                CredentialSQL.Password = reader.GetString(1);
                                CredentialSQL.PermLVL= ""+ reader.GetInt32(2);

                            }
                        }
                    }
                    con.Close();
                }
            }catch
            {
                return RedirectToPage("/Error");
            }
            
            // Verify Credential
            if (credential.Login == CredentialSQL.Login && credential.Password == CredentialSQL.Password) 
            {
                //Security Context
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name,credential.Login),
                    new Claim("PermLVL", CredentialSQL.PermLVL)
                };
                var identity = new ClaimsIdentity(claims,"MyCookieAuth");
                ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync("MyCookieAuth", claimsPrincipal);
                return RedirectToPage("/Index");
            }

            return Page();
        }
    }

    public class Credential
    {
        [Required]
        [Display(Name =" ")]
        public string Login { get; set; }
        [Required]
        [Display(Name = " ")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

}
