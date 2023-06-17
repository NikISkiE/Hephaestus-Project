using Hephaestus_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Hephaestus_Project.Pages.Stock
{
    public class CreateModel : PageModel
    {
        public string error = "NULL";
        public bool success = false;
        public string EQID { get; set; }
        [BindProperty]
        public StockInfo input { get; set; }
        private readonly IConfiguration Configuration;

        public CreateModel(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public void OnGet()
        {
            EQID = Request.Query["id"];
        }

        public void OnPost()
        {
            if (input.Serial == null)
            {
                error = "Serial can't be null";
                return;
            }
            if (input.InMaintance == null) input.InMaintance = "0";
            if (input.UserIDL == null) input.UserIDL = "NULL";
            //Database script
            try
            {
                //guess what db connection
                var constring = Configuration["ConnectionStrings:DefaultString"];
                using (SqlConnection connection = new SqlConnection(constring))
                {
                    //Insert new data
                    connection.Open();
                    String sql = "INSERT INTO Stock" +
                                "(Serial,UserIDL,Inmaintance,EquipmentID) VALUES" +
                                $"('{input.Serial}',{input.UserIDL},{input.InMaintance},{input.EquipmentID});";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                }
            }
            catch (Exception ex)
            {
                //error
                error = "Something Went Wrong";
                return;
            }


            //success
            success = true;

            Response.Redirect($"/Stock/Index?id={input.EquipmentID}");
        }
    }
}

