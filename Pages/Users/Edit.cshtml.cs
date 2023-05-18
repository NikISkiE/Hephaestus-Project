using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Hephaestus_Project.Models;
using Microsoft.Data.SqlClient;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Hephaestus_Project.Pages.Users
{
    [Authorize(Policy = "MustBeAtleastCom")]
    public class EditModel : PageModel
    {
        [BindProperty]
        public UserInfo input { get; set; }
        public bool success = false;
        public string error = "NULL";
        private readonly IConfiguration Configuration;

        public EditModel(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void OnGet()
        {
            input = new UserInfo();
            String id = Request.Query["id"];
            //DB connection
            try
            {
                var constring = Configuration["ConnectionStrings:DefaultString"];
                using (SqlConnection connection = new SqlConnection(constring))
                {
                    connection.Open();
                    String sql = "SELECT * FROM UserData WHERE id=@id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("id", id);
                        using(SqlDataReader reader = command.ExecuteReader())
                        {
                            if(reader.Read())
                            {
                                input.Id = "" + reader.GetInt32(0);
                                input.Name = reader.GetString(1);
                                input.Surname = reader.GetString(2);
                                input.Division = reader.GetString(3);
                                input.Rank = reader.GetString(4);
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
            if (input.Name == null || input.Surname == null || input.Division == null || input.Rank == null)
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
                    String sql = "UPDATE UserData "+
                                 $"SET name='{input.Name}', surname='{input.Surname}', division='{input.Division}', rank='{input.Rank}' "+
                                 $"WHERE id={input.Id}";
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

            Response.Redirect("/Users/Index");
        
        }

    }
}
