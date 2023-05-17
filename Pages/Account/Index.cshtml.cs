using Hephaestus_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Hephaestus_Project.Models;

namespace Hephaestus_Project.Pages.Account
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public AccountInfo input { get; set; }
        private readonly IConfiguration Configuration;

        public IndexModel(IConfiguration configuration)
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
                                input.PermLVL = reader.GetString(3);
                                input.Created_At = reader.GetDateTime(4).ToString();
                                input.UserID = reader["UserID"] as string;
                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
