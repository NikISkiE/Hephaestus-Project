using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Hephaestus_Project.Models;
using Microsoft.Data.SqlClient;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Drawing;

namespace Hephaestus_Project.Pages.Stock
{
    [Authorize(Policy = "MustBeAtleastQuater")]
    public class EditModel : PageModel
    {
        [BindProperty]
        public StockInfo input { get; set; }
        public List<UserInfo> users = new List<UserInfo>();
        public bool success = false;
        public string error = "NULL";
        private readonly IConfiguration Configuration;
        public string EQID;

        public EditModel(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void OnGet()
        {
            EQID = Request.Query["eqid"];
            input = new StockInfo();
            String id = Request.Query["id"];
            //DB connection
            try
            {
                var constring = Configuration["ConnectionStrings:DefaultString"];
                using (SqlConnection connection = new SqlConnection(constring))
                {
                    connection.Open();
                    String sql = $"SELECT ID, Serial, UserIDL, InMaintance FROM Stock WHERE ID=@id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("id", id);
                        using(SqlDataReader reader = command.ExecuteReader())
                        {
                            if(reader.Read())
                            {
                                input.Id = "" + reader.GetInt32(0);
                                input.Serial = reader.GetString(1);
                                input.UserIDL = (reader.IsDBNull(2)) ? "None" : reader.GetInt32(2).ToString();
                                input.InMaintance = "" + reader.GetBoolean(3);
                            }
                        }
                    }
                    sql = $"SELECT ID,Name FROM UserData";
                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                UserInfo claims = new UserInfo();
                                claims.Id = "" + reader.GetInt32(0);
                                claims.Name = reader.GetString(1);
                            users.Add(claims);
                            }
                        }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                BadRequest(ex);
            }
        }

        public void OnPost()
        {
            //no empty field
            if (input.Serial == null)
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
                    String sql = $"UPDATE Stock SET Serial='{input.Serial}', UserIDL={input.UserIDL}, InMaintance={input.InMaintance} WHERE ID={input.Id}";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                BadRequest(ex);
            }

            Response.Redirect($"/Stock/Index?id={input.EquipmentID}");
        
        }

    }
}
