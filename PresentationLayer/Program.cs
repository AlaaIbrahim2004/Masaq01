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

            // ����� ����� DbContext
            builder.Services.AddDbContext<MasaqDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // ����� ����� ��� MVC
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
                    // ������ �� �� ����� �������� �� �������
                    context.Database.EnsureCreated();
                    var seedingService = services.GetRequiredService<IDataSeeding>();
                    seedingService.DataSeed(); // ����� ����� ��������
                }
                catch (Exception ex)
                {
                    // ����� ����� (����� ����� ILogger ��� ������ ����)
                    Console.WriteLine($"��� ����� ����� ��������: {ex.Message}");
                }
            }

            #endregion

            // ����� DataSeed ��� ��� �������
            // ����� �� ������ ������� (HTTP Request Pipeline)
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles(); // ������� MapStaticAssets �� UseStaticFiles ���� ������� �������
            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}