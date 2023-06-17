using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Hephaestus_Project.Models;

namespace Hephaestus_Project.Pages.Arsenal
{
    [Authorize(Policy = "MustBeAtleastQuater")]
    public class ArsenalModel : PageModel
    {
        private readonly IConfiguration Configuration;

        public ArsenalModel(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public List<ArsenalInfo> ListArsenal = new List<ArsenalInfo>();

        public void OnGet()
        {
            try
            {
                var constring = Configuration["ConnectionStrings:DefaultString"];

                using (SqlConnection con = new SqlConnection(constring))
                {
                    con.Open();
                    string sql = "SELECT * FROM Equipment";
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        using(SqlDataReader reader = cmd.ExecuteReader()) 
                        {
                            while (reader.Read())
                            {
                                ArsenalInfo claims = new ArsenalInfo();
                                claims.Id = "" + reader.GetInt32(0);
                                claims.Name = reader.GetString(1);
                                claims.Type = reader.GetString(2);
                                claims.Stocked = "" + reader.GetInt32(3);
                                ListArsenal.Add(claims);
                            }
                        }
                    }
                    con.Close();
                }
            }catch(Exception ex)
            {
                BadRequest(ex);
            }
        }
    }
}
