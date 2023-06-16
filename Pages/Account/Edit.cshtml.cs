using Hephaestus_Project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Hephaestus_Project.Pages.Account
{
    [Authorize(Policy = "MustBeAtleastQuater")]
    public class EditModel : PageModel
    {
        [BindProperty]
        public AccountInfo input { get; set; }
        public bool success = false;
        public string error = "NULL";
        private readonly IConfiguration Configuration;

        public EditModel(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void OnGet()
        {
            input = new AccountInfo();
            String id = Request.Query["id"];
            //DB connection
            try
            {
                var constring = Configuration["ConnectionStrings:DefaultString"];
                using (SqlConnection connection = new SqlConnection(constring))
                {
                    connection.Open();
                    String sql = "SELECT * FROM AccountData WHERE id=@id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                input.Id = "" + reader.GetInt32(0);
                                input.Login = reader.GetString(1);
                                input.Password = reader.GetString(2);
                                input.PermLVL = reader.GetInt32(3).ToString();
                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch
            {
                RedirectToPage("/Error");
            }
        }

        public void OnPost()
        {
            //no empty field
            if (input.Login == null || input.Password == null || input.PermLVL == null)
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
                    String sql = "UPDATE AccountData " +
                                 $"SET login='{input.Login}', password='{input.Password}', permlvl='{input.PermLVL}'" +
                                 $"WHERE id={input.Id}";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    connection.Close();
                }
            }
            catch
            {
                RedirectToPage("/Error");
            }

            Response.Redirect($"/Account/Index?id={input.Id}");

        }

    }
}

