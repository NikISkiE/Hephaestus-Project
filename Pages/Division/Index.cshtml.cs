using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Hephaestus_Project.Models;
using System.Data;
using Hephaestus_Project.Interface;

namespace Hephaestus_Project.Pages.Division
{
    [Authorize(Policy = "MustBeAtleastCom")]
    public class DivisionModel : PageModel 
    {
        private readonly IConfiguration Configuration;
        public DivisionModel(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public List<DivisionInfo> ListDivision = new List<DivisionInfo>();

        public void OnGet()
        {
            try
            {
                
                var constring = Configuration["ConnectionStrings:DefaultString"];

                using (SqlConnection con = new SqlConnection(constring))
                {
                    con.Open();
                    string sql;
                    if (User.FindFirst("PermLVL")?.Value == "2")
                    {
                         sql = $"SELECT UserData.id, UserData.Name, UserData.Surname, UserData.Division, UserData.Rank FROM UserData WHERE UserData.Division IN (SELECT UserData.Division FROM UserData, AccountData WHERE AccountData.userid = UserData.id AND AccountData.login = '{User.Identity.Name}') ";
                    }
                    else
                    {
                         sql = "SELECT UserData.id, UserData.Name, UserData.Surname, UserData.Division, UserData.Rank FROM UserData ";
                    }
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DivisionInfo claims = new DivisionInfo();
                                claims.Id = reader.GetInt32(0).ToString();
                                claims.Name = reader.GetString(1);
                                claims.Surname = reader.GetString(2);
                                claims.Division = reader.GetString(3);
                                claims.Rank = reader.GetString(4);
                                ListDivision.Add(claims);
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