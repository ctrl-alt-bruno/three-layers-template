using ThreeLayers.Business.Interfaces;
using ThreeLayers.Business.Notifications;
using ThreeLayers.Business.Services;
using ThreeLayers.Data.Context;
using ThreeLayers.Data.Repository;

namespace ThreeLayers.WebAPI.Configuration;

public static class DependencyInjectionConfiguration
{
     public static IServiceCollection ConfigureDependencies(this IServiceCollection services)
     {
          // ThreeLayers.Data
          services.AddScoped<MyDbContext>();
          services.AddScoped<IProductRepository, ProductRepository>();
          services.AddScoped<ISupplierRepository, SupplierRepository>();
          
          // ThreeLayers.Business
          services.AddScoped<ISupplierService, SupplierService>();
          services.AddScoped<IProductService, ProductService>();
          services.AddScoped<INotifier, Notifier>();
          
          return services;
     }
}