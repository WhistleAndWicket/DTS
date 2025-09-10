using Application;
using Infrastructure;
using Infrastructure.Middleware;
using Microsoft.OpenApi.Models;
using NSwag.Generation;
using System.Reflection;
using System.Text.Json.Serialization;

namespace Api
{
    /// <summary>
    /// Entry point of the application.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Application startup method. Initializes and starts the program.
        /// </summary>
        /// <param name="args">Command-line arguments passed to the application.</param>
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services
                .AddApplication()
                .AddInfrastructure(builder.Configuration);

            // Add services to the container.

            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(s =>
            {
                s.CustomSchemaIds(type => type.FullName);
                s.SwaggerDoc("v1", new OpenApiInfo { Title = "Data Tracking System", Version = "v1" });
                var allMyXmlCommentFileNames =
                    Directory.GetFiles(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!), "*.xml");
                foreach (var filePath in allMyXmlCommentFileNames)
                {
                    s.IncludeXmlComments(filePath);
                }

                // fix the ID generated when generics are used, prevents an invalid OpenAPI doc
                // https://github.com/domaindrivendev/Swashbuckle.AspNetCore/issues/2661#issuecomment-2429971459
                s.CustomSchemaIds(x =>
                {
                    var arguments = x.GetGenericArguments();

                    if (arguments.Length == 0)
                    {
                        return x.FullName;
                    }

                    var result = $"{x.FullName?.Split('`').First()}{arguments.Select(genericArg => genericArg.Name).Aggregate((previous, current) => previous + current)}";
                    return result;
                });
            });

            // This is to make sure the NSwag command executes and generates the output file.
            // This happens on .NET core 8 and had to include this wrapper class.
            // github.com/RicoSuter/NSwag/issues/4669#issuecomment-1898591950
            builder.Services.AddSingleton<IOpenApiDocumentGenerator, GeneratorWrapper>();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAngularDevClient", policy =>
                {
                    policy.WithOrigins("http://localhost:4200") // Angular dev server
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
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

            app.UseCors("AllowAngularDevClient");

            app.MapControllers();

            app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

            app.Run();
        }
    }
}
