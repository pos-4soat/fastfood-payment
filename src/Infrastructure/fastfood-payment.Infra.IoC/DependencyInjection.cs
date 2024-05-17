using fastfood_payment.Application.Shared.BaseResponse;
using fastfood_payment.Domain.Contracts.Http;
using fastfood_payment.Domain.Contracts.Mongo;
using fastfood_payment.Domain.Contracts.Payment;
using fastfood_payment.Infra.Http;
using fastfood_payment.Infra.MercadoPago;
using fastfood_payment.Infra.MongoDb.Context;
using fastfood_payment.Infra.MongoDb.Repository;
using FluentValidation;
using FluentValidation.AspNetCore;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace fastfood_payment.Infra.IoC;

[ExcludeFromCodeCoverage]
public static class DependencyInjection
{
    public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureBehavior();
        services.ConfigureServices();
        services.ConfigureAutomapper();
        services.ConfigureMediatr();
        services.ConfigureFluentValidation();
        services.ConfigureHttpClient(configuration);
        services.ConfigureDatabase(configuration);
    }

    private static void ConfigureBehavior(this IServiceCollection services)
    {
        services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
    }
    private static void ConfigureAutomapper(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(Result).Assembly);
    }

    private static void ConfigureMediatr(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Result).Assembly));
    }

    private static void ConfigureFluentValidation(this IServiceCollection service)
    {
        service.AddValidatorsFromAssemblyContaining<Result>();
        service.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
        service.AddFluentValidationRulesToSwagger();
    }

    private static void ConfigureHttpClient(this IServiceCollection services, IConfiguration configuration)
    {
        IConfiguration externalConfig = configuration.GetSection("Http");
        string baseUrlOrder = externalConfig.GetSection("Order").Value;

        services.AddTransient<IOrderHttpClient>(provider =>
        {
            string? baseAddress = baseUrlOrder;
            return new OrderHttpClient(baseAddress);
        });

        string baseUrlProduction = externalConfig.GetSection("Production").Value;

        services.AddTransient<IProductionHttpClient>(provider =>
        {
            string? baseAddress = baseUrlProduction;
            return new ProductionHttpClient(baseAddress);
        });
    }

    private static void ConfigureServices(this IServiceCollection services)
    {
        services.AddScoped<IPaymentRepository, PaymentRepository>();
        services.AddScoped<IOrderPayment, MercadoPagoPayment>();
    }

    private static void ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<MongoDbContext>();
    }
}
