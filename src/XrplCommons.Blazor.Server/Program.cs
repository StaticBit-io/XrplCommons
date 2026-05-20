WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient("ApiProxy", client =>
{
    client.BaseAddress = new Uri("https://map.xrpl-commons.org");
});

WebApplication app = builder.Build();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.Map("/api/{**path}", async (string path, HttpContext context, IHttpClientFactory httpClientFactory) =>
{
    HttpClient client = httpClientFactory.CreateClient("ApiProxy");

    string requestUri = $"/api/{path}";
    if (context.Request.QueryString.HasValue)
    {
        requestUri += context.Request.QueryString.Value;
    }

    HttpResponseMessage response = await client.GetAsync(requestUri, context.RequestAborted);
    string content = await response.Content.ReadAsStringAsync(context.RequestAborted);

    context.Response.StatusCode = (int)response.StatusCode;
    context.Response.ContentType = response.Content.Headers.ContentType?.ToString() ?? "application/json";
    await context.Response.WriteAsync(content, context.RequestAborted);
});

app.MapFallbackToFile("index.html");

app.Run();
