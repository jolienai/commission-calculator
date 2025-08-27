
using FCamara.Commission.Api.Middlewares;
using FCamara.Commission.Application;

namespace FCamara.Commission.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var allowedOrigins = builder.Configuration.GetSection("AllowedCorsOrigins").Get<string[]>()??[];
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowFrontend", policy =>
            {
                policy.WithOrigins(allowedOrigins) // React dev server
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });
        
        builder.Services.AddControllers();
        builder.Services.AddApiVersioning();
        
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddApplication();

        var app = builder.Build();
        app.UseCors("AllowFrontend");

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        
        app.UseAuthorization();
        
        app.UseMiddleware<ExceptionLoggingMiddleware>();
        
        app.MapControllers();

        app.Run();
    }
}