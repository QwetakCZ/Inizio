using InzioTest.Components;
using InzioTest.Services;

namespace InzioTest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddHttpClient(); //P�idani HttpClient
            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

           builder.Services.AddScoped<GoogleSearchService>(); //P�idani DI pro GoogleSearchService
            builder.Services.AddScoped<ConvertDataToJSON>(); //P�idani DI pro ConvertDataToJSON

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseAntiforgery();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Run();
        }
    }
}
