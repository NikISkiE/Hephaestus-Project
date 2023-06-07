namespace Hephaestus_Project
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthentication("MyCookieAuth").AddCookie("MyCookieAuth",options =>
            {
                options.Cookie.Name = "MyCookieAuth";
                options.LoginPath= "/Account/Login";
                options.AccessDeniedPath= "/AccessDenied";
            });
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("OnlyAdmin",
                    policy => policy.RequireClaim("PermLVL", "4"));

                options.AddPolicy("MustBeAtleastQuater", 
                    policy => policy.RequireClaim("PermLVL","3","4"));

                options.AddPolicy("MustBeAtleastCom",
                    policy => policy.RequireClaim("PermLVL", "2", "3", "4"));
            });
            builder.Services.AddRazorPages();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapRazorPages();
            
            app.Run();
        }
    }
    Siema
}