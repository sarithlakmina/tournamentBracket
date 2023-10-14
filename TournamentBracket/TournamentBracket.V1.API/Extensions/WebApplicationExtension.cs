namespace Microsoft.AspNetCore.Builder;

public static class WebApplicationExtension
{
    public static void Initialize(this WebApplication webApplication)
    {
        webApplication.UseRouting();

        webApplication.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();

            endpoints.MapGet("/", async context =>
            {
                await context.Response.WriteAsync("Hello from Back End API");
            });
        });

        webApplication.Run();
    }
}
