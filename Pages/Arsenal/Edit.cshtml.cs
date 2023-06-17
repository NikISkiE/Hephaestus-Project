using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Hephaestus_Project.Models;
using Microsoft.Data.SqlClient;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Hephaestus_Project.Pages.Arsenal
{
    [Authorize(Policy = "MustBeAtleastCom")]
    public class EditModel : PageModel
    {
        [BindProperty]
        public ArsenalInfo input { get; set; }
        public bool success = false;
        public string error = "NULL";
        private readonly IConfiguration Configuration;

        public EditModel(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void OnGet()
        {
            input = new ArsenalInfo();
            String id = Request.Query["id"];
            //DB connection
            try
            {
                var constring = Configuration["ConnectionStrings:DefaultString"];
                using (SqlConnection connection = new SqlConnection(constring))
                {
                    connection.Open();
                    String sql = "SELECT * FROM Equipment WHERE id=@id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("id", id);
                        using(SqlDataReader reader = command.ExecuteReader())
                        {
                            if(reader.Read())
                            {
                                input.Id = reader.GetInt32(0).ToString();
                                input.Name = reader.GetString(1);
                                input.Type = reader.GetString(2);
                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                //error
                error = "Something Went Wrong";
                return;
            }
        }

        public void OnPost()
        {
            //no empty field
            if (input.Name == null || input.Type == null)
            {
                error = "Fill All Empty Spaces";
                return;
            }
            //Another DB Connection
            try
            {
                var constring = Configuration["ConnectionStrings:DefaultString"];
                using (SqlConnection connection = new SqlConnection(constring))
                {
                    connection.Open();
                    String sql = $"UPDATE Equipment SET Name='{input.Name}', Type='{input.Type}' WHERE ID='{input.Id}'";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                //error
                error = "Something Went Wrong";
                return;
            }

            Response.Redirect("/Arsenal/Index");
        
        }

    }
}
