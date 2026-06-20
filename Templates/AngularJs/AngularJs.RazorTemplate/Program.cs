namespace AngularJs.RazorTemplate
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddControllers(); // برای API های Angular لازم است

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            // UseStaticFiles روش مطمئن‌تری برای سرو فایل‌های wwwroot هست،
            // مخصوصاً برای فایل‌های HTML/JS که AngularJS بهشون نیاز داره
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();
            app.MapControllers();

            // Fallback: هر مسیری که با Razor Page، API یا فایل استاتیک مچ نشد، بره به Index
            app.MapFallbackToPage("/Index");

            app.Run();
        }
    }
}