using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Hephaestus_Project.Pages.Soldier
{
    public class IndexModel : PageModel
    {
        public List<SoldierLogin> Logins = new List<SoldierLogin>();
        public void OnGet()
        {
            try
            {
                String connectionString = "Data Source=DESKTOP-TGVUSIC\\SQLEXPRESS;Initial Catalog=Smithy;Integrated Security=True";

                using(SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM Soldiers";
                    using(SqlCommand command = new SqlCommand(sql, connection)) 
                    {
                        using(SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read()) 
                            {
                                SoldierLogin soldierLogin = new SoldierLogin();
                                soldierLogin.id = "" + reader.GetInt32(0);
                                soldierLogin.login = reader.GetString(1);
                                soldierLogin.password = reader.GetString(2);
                                soldierLogin.permissionLevel= "" + reader.GetInt32(3);
                                soldierLogin.created_at = reader.GetDateTime(4).ToString();

                                Logins.Add(soldierLogin);
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Exception "+ex.ToString());
            }
        }
    }

    public class SoldierLogin
    {
        public String id;
        public String login;
        public String password;
        public String permissionLevel;
        public String created_at;
    }
}
