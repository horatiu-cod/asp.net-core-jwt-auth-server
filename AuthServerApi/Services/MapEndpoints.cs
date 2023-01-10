namespace AuthServerApi.Services;

public static class MapEndpoints
{
    public static WebApplication MapUserEndpoints(this WebApplication app)
    {
        app.MapGet("/Users", () => "Hello User");

        return app;
    }
    public static WebApplication MapAuthEndpoints(this WebApplication app)
    {
        app.MapControllers();

        return app;
    }
}
