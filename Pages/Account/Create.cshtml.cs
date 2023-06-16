using Hephaestus_Project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System.Data.SqlClient;

namespace Hephaestus_Project.Pages.Account
{
    [Authorize(Policy = "MustBeAtleastQuater")]
    public class CreateModel : PageModel
    {
        [BindProperty]
        public AccountInfo input { get; set; } = new AccountInfo();
        public bool success = false;
        public string error = "NULL";
        private readonly IConfiguration Configuration;

        public CreateModel(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public void OnGet()
        {
            String id = Request.Query["id"];
            input.UserID = id;

        }

        public void OnPost()
        {
            //no empty field
            if (input.Login == null || input.Password == null || input.PermLVL == null)
            {
                error = "Fill All Empty Spaces";
                return;
            }
            //Database script
            try
            {
                //guess what db connection
                var constring = Configuration["ConnectionStrings:DefaultString"];
                using (SqlConnection connection = new SqlConnection(constring))
                {
                    //Insert new data
                    connection.Open();
                    String sql = "INSERT INTO AccountData" +
                                "(login,password,permlvl,userid) VALUES" +
                                $"('{input.Login}','{input.Password}','{input.PermLVL}','{input.UserID}');";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                }
            }
            catch
            {
                RedirectToPage("/Error");
            }


            //success
            success = true;

            Response.Redirect("/Users/Index");
        }
    }
}

