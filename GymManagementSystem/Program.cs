using GymManagementSystem.BLL.AttachmentServices;
using GymManagementSystem.BLL.Mapping;
using GymManagementSystem.BLL.Services.Classes;
using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.DAL.DataSeading;
using GymManagementSystem.DAL.Repository.Classes;
using GymManagementSystem.DAL.Repository.Interfaces;
using GymManagementSystem.DAL.UnitOFWork.Classes;
using GymManagementSystem.DAL.UnitOFWork.Interfaces;
using GymManagementSystem.DbContexts;
using GymManagementSystem.PL;
using Microsoft.EntityFrameworkCore;

namespace GymManagementSystem
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            // DbContext
            builder.Services.AddDbContext<GymDbContext>(options =>
            {
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection")
                );
            });

            #region Repository
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped<IMemberService, MemberServices>();
            builder.Services.AddScoped<IPlanServices, PlanServices>();
            builder.Services.AddScoped<ITrainerServices, TrainerServices>();
            builder.Services.AddScoped<ISessionServices, SessionsServices>();
            builder.Services.AddScoped<ISessionRepository, SessionRepository>();
            builder.Services.AddScoped<IAttachmentServices, AttachmentServices>();
            builder.Services.AddScoped<IUnitOFWork, UnitOfWork>();

            #endregion

            #region auto Mapper
            builder.Services.AddAutoMapper(m => m.AddProfile(new MemberMappingProfile()));
            builder.Services.AddAutoMapper(m => m.AddProfile(new TrainerMappingProfile()));
            builder.Services.AddAutoMapper(m => m.AddProfile(new PlanMappingProfile()));
            builder.Services.AddAutoMapper(m => m.AddProfile(new SessionMappingProfile()));
            #endregion
            var app = builder.Build();
            await app.MigrateAndSeedDatabaseAsync();
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
