using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Hephaestus_Project.Models;

namespace Hephaestus_Project.Pages.Users
{
    [Authorize(Policy = "MustBeAtleastQuater")]
    public class UsersModel : PageModel
    {
        private readonly IConfiguration Configuration;

        public UsersModel(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public List<UserInfo> ListUsers = new List<UserInfo>();

        public void OnGet()
        {
            try
            {
                var constring = Configuration["ConnectionStrings:DefaultString"];

                using (SqlConnection con = new SqlConnection(constring))
                {
                    con.Open();
                    string sql = "SELECT * FROM UserData";
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        using(SqlDataReader reader = cmd.ExecuteReader()) 
                        {
                            while (reader.Read())
                            {
                                UserInfo claims = new UserInfo();
                                claims.Id = "" + reader.GetInt32(0);
                                claims.Name = reader.GetString(1);
                                claims.Surname = reader.GetString(2);
                                claims.Division = reader.GetString(3);
                                claims.Rank = reader.GetString(4);
                                claims.AccountID = reader["accountid"] as string;

                                ListUsers.Add(claims);
                            }
                        }
                    }
                    con.Close();
                }
            }catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
