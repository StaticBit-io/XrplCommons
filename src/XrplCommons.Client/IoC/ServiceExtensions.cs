using Microsoft.Extensions.DependencyInjection;

namespace XrplCommons.Client.IoC;

public static class ServiceExtensions
{
    public static IServiceCollection AddXrplCommonsClient(
        this IServiceCollection services,
        Action<XrplCommonsClientOptions>? configure = null)
    {
        XrplCommonsClientOptions options = new();
        configure?.Invoke(options);

        services.AddHttpClient<IXrplCommonsClient, XrplCommonsClient>(client =>
        {
            client.BaseAddress = new Uri(options.BaseUrl);
        });

        return services;
    }
}
