using Db.DbContext;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace netcorewebapi.Ioc
{
    public static class Ioc
    {
        public static CarDb CarDb => IoCContainer.Context;
    }

    public static class IoCContainer
    {
        public static ServiceProvider Provider { get; set; }

        public static CarDb Context { get; set; }
        public static IConfiguration Configuration { get; set; }
    }
}