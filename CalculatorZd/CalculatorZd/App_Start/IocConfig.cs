using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.WebApi;

namespace JsFrameworkTest
{
    public class IocConfig
    {
        public static void Init()
        {
            // Create the container builder.
            var builder = new ContainerBuilder();

            // Register the Web API controllers.
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            //RegisterImplementations(builder);

            // Build the container.
            var container = builder.Build();

            // Create the depenedency resolver.
            var resolver = new AutofacWebApiDependencyResolver(container);

            // Configure Web API with the dependency resolver.
            GlobalConfiguration.Configuration.DependencyResolver = resolver;
        }

        //private static void RegisterImplementations(ContainerBuilder builder)
        //{
        //    builder.RegisterType<EntityRepository>().As<IEntityRepository>().SingleInstance();
        //}
    }
}