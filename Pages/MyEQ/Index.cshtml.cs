using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Hephaestus_Project.Models;

namespace Hephaestus_Project.Pages.Users
{
    [Authorize(Policy = "MustBeAtleastCom")]
    public class MyEQModel : PageModel
    {
        private readonly IConfiguration Configuration;

        public MyEQModel(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public List<MyEQinfo> ListMyEQ = new List<MyEQinfo>();

        public void OnGet()
        {
            try
            {
                var constring = Configuration["ConnectionStrings:DefaultString"];

                using (SqlConnection con = new SqlConnection(constring))
                {
                    con.Open();
                    string sql = "SELECT stock.ID,Equipment.Name,Stock.Serial,Equipment.Type,Stock,InMaintance FROM Stock,Equipment,UserData,AccountData WHERE stock.EquipmentID=Equipment.ID AND stock.UserIDL=UserData.ID AND UserData.ID=AccountData.UserID AND AccountData.login=";
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                MyEQinfo claims = new MyEQinfo();
                                claims.Id = "" + reader.GetInt32(0);
                                claims.Name = reader.GetString(1);
                                claims.Serial = reader.GetString(2);
                                claims.Type = reader.GetString(3);
                                claims.InMaintance = reader.GetBool(4);

                                ListMyEQ.Add(claims);
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
