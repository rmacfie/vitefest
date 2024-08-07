using System;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using ViteFest;
using ViteFest.AspNetCore;

#pragma warning disable IDE0130 // ReSharper disable CheckNamespace

namespace Microsoft.Extensions.DependencyInjection;

public static class ViteServiceExtensions
{
    public static IServiceCollection AddViteFest(
        this IServiceCollection services,
        Action<ViteOptions> configure
    )
    {
        if (services == null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        if (configure == null)
        {
            throw new ArgumentNullException(nameof(configure));
        }

        if (services.Any(x => x.ServiceType == typeof(Vite)))
        {
            throw new InvalidOperationException(
                "ViteFest has already been added to the service collection."
            );
        }

        var options = new ViteOptions();
        configure(options);
        options.Validate();

        services.AddSingleton<IViteEnvironment>(x =>
        {
            var hostEnvironment = x.GetRequiredService<IWebHostEnvironment>();
            return new ViteEnvironment(
                options,
                hostEnvironment.WebRootPath,
                hostEnvironment.IsDevelopment()
            );
        });

        services.AddSingleton<IViteManifestReader>(x => new ViteManifestReader());
        services.AddSingleton<IViteResourceMapper>(x => new ViteResourceMapper(
            x.GetRequiredService<IViteEnvironment>()
        ));
        services.AddSingleton<IViteState>(x => new ViteState(
            x.GetRequiredService<IViteEnvironment>(),
            x.GetRequiredService<IViteManifestReader>(),
            x.GetRequiredService<IViteResourceMapper>()
        ));
        services.AddSingleton<IVite>(x => new Vite(x.GetRequiredService<IViteState>()));

        services.AddHostedService<ViteHostedService>();

        return services;
    }
}
