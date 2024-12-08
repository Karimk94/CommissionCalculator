
namespace FCamara.CommissionCalculator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddCors(options =>
            {
                var CORS = builder?.Configuration?.GetSection("Cors")?.Get<List<string>>()?.ToArray();

                options.AddDefaultPolicy(
                   builder =>
                   {
                       if(CORS?.Length > 0)
                       {
                           builder.WithOrigins(CORS)
                                .AllowAnyHeader()
                                .AllowCredentials()
                                .AllowAnyMethod();
                       }
                   });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
