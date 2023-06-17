using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Hephaestus_Project.Models;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Hephaestus_Project.Pages.Stock
{
    [Authorize(Policy = "MustBeAtleastQuater")]
    public class StockModel : PageModel
    {
        private readonly IConfiguration Configuration;
        internal String ID { get; set; }

        public StockModel(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public List<StockInfo> ListStock = new List<StockInfo>();



        public void OnGet()
        {
            ID = Request.Query["id"];
            try
            {
                var constring = Configuration["ConnectionStrings:DefaultString"];

                using (SqlConnection con = new SqlConnection(constring))
                {
                    con.Open();
                    string sql = $"SELECT ID, Serial, UserIDL, InMaintance FROM Stock WHERE stock.EquipmentID = {ID}";
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        using(SqlDataReader reader = cmd.ExecuteReader()) 
                        {
                            while (reader.Read())
                            {
                                StockInfo claims = new StockInfo();
                                
                                claims.ID = reader.GetInt32(0).ToString();
                                claims.Serial = reader.GetString(1);
                                claims.UserIDL = (reader.IsDBNull(2)) ? "None" : reader.GetInt32(2).ToString();
                                claims.InMaintance = "" + reader.GetBoolean(3);
                                claims.Id = claims.UserIDL;
                                ListStock.Add(claims);
                            }
                        }                       
                    }
                    sql = $"SELECT Name FROM UserData WHERE id=@id";
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                        foreach (var item in ListStock)
                        {
                            if (item.Id != null) item.Name = " ";
                            cmd.Parameters.AddWithValue("id", item.Id);
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    item.Name= reader.GetString(0);
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
