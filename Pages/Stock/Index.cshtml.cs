using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Hephaestus_Project.Models;

namespace Hephaestus_Project.Pages.Stock
{
    [Authorize(Policy = "MustBeAtleastCom")]
    public class StockModel : PageModel
    {
        private readonly IConfiguration Configuration;

        public StockModel(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public List<StockInfo> ListStock = new List<StockInfo>();

        public void OnGet()
        {
            String ID = Request.Query["id"];
            try
            {
                var constring = Configuration["ConnectionStrings:DefaultString"];

                using (SqlConnection con = new SqlConnection(constring))
                {
                    con.Open();
                    string sql = $"SELECT Stock.ID, Stock.Serial, UserData.Name, Stock.InMaintance FROM Stock, UserData WHERE Stock.UserIDL = UserData.id AND stock.EquipmentID = {ID}";
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        using(SqlDataReader reader = cmd.ExecuteReader()) 
                        {
                            while (reader.Read())
                            {
                                StockInfo claims = new StockInfo();
                                claims.ID = reader.GetInt32(0).ToString();
                                claims.Serial = reader.GetString(1);
                                claims.UserName = reader.GetString(2);
                                claims.InMaintance = "" + reader.GetBoolean(3);
                                ListStock.Add(claims);
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
