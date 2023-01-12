using Kontatus.Data.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Kontatus.API.Configurations
{
    public static class ContextConfig
    {
        public static void Configure(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>()
           .CreateScope();
            serviceScope.ServiceProvider.GetService<KontatusContext>().Database.Migrate();
        }

    }
}
