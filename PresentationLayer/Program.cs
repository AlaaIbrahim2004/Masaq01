using DataAccessLayer.Contracts;
using DataAccessLayer.Data;
using DataAccessLayer.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace PresentationLayer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ≈÷«›… Œœ„«  DbContext
            builder.Services.AddDbContext<MasaqDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // ≈÷«›… Œœ„«  «·‹ MVC
            builder.Services.AddControllersWithViews();

            builder.Services.AddScoped<IDataSeeding, DataSeeding>();

            var app = builder.Build();

            #region Data Seeding

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<MasaqDbContext>();
                    // «· √ﬂœ „‰ √‰ ﬁ«⁄œ… «·»Ì«‰«   „ ≈‰‘«ƒÂ«
                    context.Database.EnsureCreated();
                    var seedingService = services.GetRequiredService<IDataSeeding>();
                    seedingService.DataSeed(); //  ‰›Ì–  ÂÌ∆… «·»Ì«‰« 
                }
                catch (Exception ex)
                {
                    //  ”ÃÌ· «·Œÿ√ (Ì„ﬂ‰ﬂ ≈÷«›… ILogger Â‰« · ”ÃÌ· √›÷·)
                    Console.WriteLine($"Œÿ√ √À‰«¡  ÂÌ∆… «·»Ì«‰« : {ex.Message}");
                }
            }

            #endregion

            //  ‰›Ì– DataSeed ⁄‰œ »œ¡ «· ÿ»Ìﬁ
            //  ﬂÊÌ‰ Œÿ √‰«»Ì» «·ÿ·»«  (HTTP Request Pipeline)
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles(); // «” »œ«· MapStaticAssets »‹ UseStaticFiles ·œ⁄„ «·„·›«  «·À«» …
            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}