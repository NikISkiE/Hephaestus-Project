using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Hephaestus_Project.Models;
using Microsoft.Data.SqlClient;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Hephaestus_Project.Pages.Stock
{
    [Authorize(Policy = "MustBeAtleastCom")]
    public class EditModel : PageModel
    {
        [BindProperty]
        public StockInfo input { get; set; }
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
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                //error
                error = "Something Went Wrong";
                return;
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
                    String sql = $"UPDATE Stock SET Serial='{input.Serial}', UserIDL='{input.UserIDL}', InMaintance='{input.InMaintance}' WHERE ID='{input.Id}'";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                //error
                error = "Something Went Wrong";
                return;
            }

            Response.Redirect($"/Stock/Index?id={input.EquipmentID}");
        
        }

    }
}
