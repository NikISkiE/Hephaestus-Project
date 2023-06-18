using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Hephaestus_Project.Models;
using System.Runtime.InteropServices;
using Hephaestus_Project.Pages.Login;

namespace Hephaestus_Project.Pages.Division
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
            String id = Request.Query["id"];
            try
            {

                var constring = Configuration["ConnectionStrings:DefaultString"];

                using (SqlConnection con = new SqlConnection(constring))
                {
                    con.Open();
                    string sql = $"SELECT Equipment.Name, Stock.Serial, Equipment.Type, Stock.InMaintance FROM Equipment,Stock WHERE Equipment.ID=Stock.EquipmentID AND Stock.UserIDL = {id}";
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                MyEQinfo claims = new MyEQinfo();
                                claims.Name = reader.GetString(0);
                                claims.Serial = reader.GetString(1);
                                claims.Type = reader.GetString(2);
                                claims.InMaintance = reader.GetBoolean(3);

                                ListMyEQ.Add(claims);
                            }
                        }
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                BadRequest(ex);
            }
        }
    }
}