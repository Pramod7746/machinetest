using System.Data.Entity.Core.Metadata.Edm;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using machinetest.Services; // Import your service namespace
using Unity; // Unity container for Dependency Injection
using Unity.Mvc5; // Unity MVC integration

namespace machinetest
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            // Register areas, filters, routes, and bundles
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Configure Dependency Injection
            RegisterDependencyResolver();
        }

        private void RegisterDependencyResolver()
        {
            var container = new UnityContainer();

            container.RegisterType<ICategoryService, CategoryService>();
            container.RegisterType<IProductService, ProductService>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }

    }
}
