
using APICore_7.Models;
using APICore_7.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.ComponentModel.DataAnnotations;
namespace APICore_7
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);



            #region DbContext Connection String 
            var ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<ApplicationDbContext>(option =>
            option.UseSqlServer(ConnectionString));

            #endregion

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            #region Dependencs Injection
            builder.Services.AddScoped<IGenresServices, GenresService>();
            builder.Services.AddScoped<IMoviesServices, MoviesServices>();
            #endregion

            #region Swagger Options 
            builder.Services.AddSwaggerGen(option =>
         {
             option.SwaggerDoc("v1", new OpenApiInfo
             {
                 Version = "v1",
                 Title = "MoviesApi",
                 Description = "My Project Api",
                 TermsOfService = new Uri("https://www.google.com"),
                 Contact = new OpenApiContact
                 {
                     Name = "Nader",
                     Email = "Nader.Mamdouh@gmail.com",
                     Url = new Uri("https://www.google.com")
                 },
                 License = new OpenApiLicense
                 {
                     Name = "My License",
                     Url = new Uri("https://www.google.com")
                 }
             });
             option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
             {
                 Name = "Authrization",
                 Type = SecuritySchemeType.ApiKey,
                 Description = "Enter Your JWT Key",
                 Scheme = "Bearer",
                 BearerFormat = "JWT",
                 In = ParameterLocation.Header
             });
             option.AddSecurityRequirement(new OpenApiSecurityRequirement
             {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
             });
         });
            #endregion


            #region CORS Policy
            builder.Services.AddCors();
            #endregion



            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            #region Configuration For Cors Policy 
            
            app.UseCors(option =>
                        option.AllowAnyHeader()
                              .AllowAnyMethod()
                              .AllowAnyOrigin()
                      );

            #endregion

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
