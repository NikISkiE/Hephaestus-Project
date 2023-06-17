using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Hephaestus_Project.Models;
using Microsoft.Build.Framework;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;

namespace Hephaestus_Project.Pages.Arsenal
{
    [Authorize(Policy = "MustBeAtleastCom")]
    public class CreateModel : PageModel
    {
        [BindProperty]
        public ArsenalInfo input { get; set; }
        public bool success = false;
        public string error = "NULL";
        private readonly IConfiguration Configuration;

        public CreateModel(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public void OnGet()
        {

        }

        public void OnPost() 
        {
            //no empty field
            if (input.Name == null|| input.Type == null)
            {
                error = "Fill All Empty Spaces";
                return;
            }
            //Database script
            try
            {
                var constring = Configuration["ConnectionStrings:DefaultString"];
                using(SqlConnection connection = new SqlConnection(constring))
                {
                    connection.Open();
                    String sql = $"INSERT INTO Equipment(Name,Type) VALUES ('{input.Name}','{input.Type}')";
                    using(SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    connection.Close();
                }
            }catch(Exception ex)
            {
                //error
                error = "Something Went Wrong";
                return;
            }
           //success
           success = true;

            Response.Redirect("/Arsenal/Index");
        }
    }
    
}
