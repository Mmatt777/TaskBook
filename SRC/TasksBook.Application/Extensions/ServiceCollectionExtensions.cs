using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;



namespace TasksBook.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddApplication(this IServiceCollection services)
    {
        var appAssembly = typeof(ServiceCollectionExtensions).Assembly;

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(appAssembly));
        services.AddAutoMapper(cfg => cfg.AddMaps(appAssembly));

        services.AddValidatorsFromAssembly(appAssembly)
                .AddFluentValidationAutoValidation();
    }

}
