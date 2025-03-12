using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Catalog.Application.Abstractions;
using OnlineStore.Catalog.Application.Services;
using OnlineStore.Catalog.Contracts.Common;
using OnlineStore.Catalog.Contracts.Options;
using OnlineStore.Catalog.Domain.Repositories;
using OnlineStore.Catalog.Infrastructure.Caching;
using OnlineStore.Catalog.Infrastructure.Data;
using OnlineStore.Catalog.Infrastructure.Data.Repositories;
using OnlineStore.Catalog.Infrastructure.Mappings;
using StackExchange.Redis.Extensions.Core.Configuration;
using StackExchange.Redis.Extensions.Newtonsoft;

namespace OnlineStore.Catalog.WebApi.Extensions;

public static class AppBuilderExtensions
{
    public static WebApplicationBuilder AddData(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

        builder.Services
            .AddDbContext<MutableOnlineStoreDbContext>(options =>
            {
                options.UseNpgsql(connectionString);
            })
            .AddDbContext<ReadonlyOnlineStoreDbContext>(options =>
            {
                options.UseNpgsql(connectionString);
            });

        builder.Services.AddScoped<IProductRepository, ProductRepository>();
        builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
        builder.Services.AddScoped<IImageRepository, ImageRepository>();

        return builder;
    }

    public static WebApplicationBuilder AddApplicationServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IProductService, ProductService>();
        builder.Services.AddScoped<ICategoryService, CategoryService>();
        builder.Services.AddScoped<IImageService, ImageService>();
        
        builder.Services.AddSingleton<TimeProvider, DateTimeProvider>();
        builder.Services.AddSingleton<ISpecificationFactory, SpecificationFactory>();

        builder.Services.Decorate<IProductService, CachingProductService>();

        return builder;
    }

    public static WebApplicationBuilder AddDistributedCaching(this WebApplicationBuilder builder)
    {
        var redisConfiguration = builder.Configuration
            .GetSection("Redis")
            .Get<RedisConfiguration>()!;

        builder.Services.AddStackExchangeRedisExtensions<NewtonsoftSerializer>(redisConfiguration);

        builder.Services.AddSingleton<ICacheService, RedisCacheService>();

        return builder;
    }

    public static WebApplicationBuilder AddOptions(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<ImageOptions>(builder.Configuration.GetSection("ImagesOptions"));
        builder.Services.Configure<PaginationOptions>(builder.Configuration.GetSection("PaginationOptions"));

        return builder;
    }

    public static WebApplicationBuilder AddMappings(this WebApplicationBuilder builder)
    {
        var mapperConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new ProductMappingProfile());
            mc.AddProfile(new CategoryMappingProfile());
        });

        IMapper mapper = mapperConfig.CreateMapper();
        builder.Services.AddSingleton(mapper);

        return builder;
    }
}
