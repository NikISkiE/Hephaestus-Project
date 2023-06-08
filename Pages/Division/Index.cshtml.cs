using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Hephaestus_Project.Models;

namespace Hephaestus_Project.Pages.Division
{
    [Authorize(Policy = "MustBeAtleastCom")]
    public class Division : PageModel
    {
        private readonly IConfiguration Configuration;

        public Division(IConfiguration configuration)
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
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                UserInfo claims = new UserInfo();
                                claims.Id = "" + reader.GetInt32(0);
                                claims.Name = reader.GetString(1);
                                claims.Surname = reader.GetString(2);
                                claims.Division = reader.GetString(3);
                                claims.Rank = reader.GetString(4);

                                ListUsers.Add(claims);
                            }
                        }
                    }
                    foreach (UserInfo claims in ListUsers)
                    {
                        sql = $"SELECT id FROM AccountData WHERE userid={claims.Id}";
                        using (SqlCommand cmd = new SqlCommand(sql, con))
                        {
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    claims.IsRegistered = (reader.GetInt32(0) == null) ? 0 : reader.GetInt32(0);
                                }
                            }
                        }
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}