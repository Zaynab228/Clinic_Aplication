using clinic_api_project.models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

namespace clinic_api_project
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.;

            builder.Services.AddIdentity<UserApplication, IdentityRole>().AddEntityFrameworkStores<Context>();
            builder.Services.AddDbContext<Context>(optionsBuilder =>
            {
                optionsBuilder.UseSqlServer(@"Data Source=ZAYNAB228\SQL2022;Initial Catalog=Clinic_API;Integrated Security=True;Encrypt=False;");


            });
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
                });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "MoviesApi",
                    Description = "My first api",
                    TermsOfService = new Uri("https://www.google.com"),
                    Contact = new OpenApiContact
                    {
                        Name = "DevCreed",
                        Email = "test@domain.com",
                        Url = new Uri("https://www.google.com")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "My license",
                        Url = new Uri("https://www.google.com")
                    }
                });

    //            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    //            {
    //                Name = "Authorization",
    //                Type = SecuritySchemeType.ApiKey,
    //                Scheme = "Bearer",
    //                BearerFormat = "JWT",
    //                In = ParameterLocation.Header,
    //                Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\""
    //            });
    //            options.AddSecurityRequirement(new OpenApiSecurityRequirement
    //{
    //              {
    //                    new OpenApiSecurityScheme
    //                    {
    //                         Reference = new OpenApiReference
    //                            {
    //                Type = ReferenceType.SecurityScheme,
    //                Id = "Bearer"
    //                         },
    //            Name = "Bearer",
    //            In = ParameterLocation.Header
    //                    },
    //        new List<string>()
    //              }
    //             });
            });
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