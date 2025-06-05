

using System.Text.Json.Serialization;
using Camping_BookingSystem.Repositories;
using Camping_BookingSystem.Services;
using Camping_BookingSystem.Services.BookingServices;
using Microsoft.EntityFrameworkCore;

namespace Camping_BookingSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddScoped<IBookingService, BookingService>();
            builder.Services.AddScoped<IBookingRepository, BookingRepository>();
            builder.Services.AddScoped<ICustomerService, CustomerService>();
            builder.Services.AddScoped<ICustomerRepository, CustomerRepositoy>();
            builder.Services.AddScoped<ICampSpotService, CampSpotService>();
            builder.Services.AddScoped<ICampSpotRepository, CampSpotRepository>();
            builder.Services.AddScoped<ISpotTypeService, SpotTypeService>();
            builder.Services.AddScoped<ISpotTypeRepository, SpotTypeRepository>();
            
            builder.Services.AddScoped<IBookingValidator, BookingValidator>();

            builder.Services.AddControllers()
                .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = 
                    ReferenceHandler.IgnoreCycles);
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<BookingSystem_ClassLibrary.Data.CampingDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            
            builder.Services.AddScoped<ICampSiteService, CampSiteService>();
            builder.Services.AddScoped<BookingSystem_ClassLibrary.Data.ICampSiteRepository, BookingSystem_ClassLibrary.Data.CampSiteRepository>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
