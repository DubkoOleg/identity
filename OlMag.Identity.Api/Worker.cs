using Microsoft.Extensions.Hosting;
using OlMag.Identity.Api.Models;
using OpenIddict.Abstractions;
using System.Threading.Tasks;
using System.Threading;
using System;
using Microsoft.Extensions.DependencyInjection;
using OlMag.Identity.Api.Models.Scopes;

public class Worker : IHostedService
{
    private readonly IServiceProvider _serviceProvider;

    public Worker(IServiceProvider serviceProvider)
        => _serviceProvider = serviceProvider;
        
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var serviceScope = _serviceProvider.CreateScope();

        var context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await context.Database.EnsureCreatedAsync();


        await CreateManufacturingApplication(serviceScope).ConfigureAwait(false);
        await CreateScopesAsync(serviceScope).ConfigureAwait(false);

        ValueTask CreateManufacturingApplication(IServiceScope scope)
        {
            var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();
            return CreateApplications(cancellationToken, manager, ManufacturingScopes.Applications);
        }

        async ValueTask CreateScopesAsync(IServiceScope scope)
        {
            var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictScopeManager>();
            await CreateScopes(cancellationToken, manager, ManufacturingScopes.ApiScope);

        }
    }

    private static async ValueTask CreateApplications(CancellationToken ct, IOpenIddictApplicationManager manager, params OpenIddictApplicationDescriptor[] descriptors)
    {
        foreach (var descriptor in descriptors)
        {
            if (descriptor.ClientId == null)
                throw new Exception(
                    $"{nameof(OpenIddictApplicationDescriptor.ClientId)} is required but empty for {descriptor.DisplayName}");

            var entity = await manager.FindByClientIdAsync(descriptor.ClientId, ct).ConfigureAwait(false);
            if (entity != null) continue;

            await manager.CreateAsync(descriptor, ct).ConfigureAwait(false);
        }
    }

    private static async ValueTask CreateScopes(CancellationToken ct, IOpenIddictScopeManager manager, params OpenIddictScopeDescriptor[] descriptors)
    {
        foreach (var descriptor in descriptors)
        {
            var entity = await manager.FindByNameAsync(descriptor.Name!, ct).ConfigureAwait(false);
            if (entity != null) continue;

            await manager.CreateAsync(descriptor, ct).ConfigureAwait(false);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}