using Stargate.Services;
using Stargate.Services.Models;
using Stargate.Services.Repos;

namespace Stargate.API
{
    public static class WebAppServiceExtensions
    {
        public static void RegisterRepos(this IServiceCollection services)
        {
            services.AddScoped<IRepository<Person>, PersonRepo>();
            services.AddScoped<IRepository<Astronaut>, AstronautRepo>();
            services.AddScoped<IRepository<Duty>, DutyRepo>();
        }

        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IPersonService, PersonService>();
            services.AddScoped<IAstronautService, AstronautService>();
            services.AddScoped<IDutyService, DutyService>();
        }
    }
}
