using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Mooc.Data.Context;
using Mooc.Data.Service;
using System.Linq;
using System.Reflection;
using System.Web.Compilation;
using System.Web.Http;
using System.Web.Mvc;

namespace Mooc.Web.App_Start
{
    public class AutofacConfig
    {

        /// <summary>
        /// 负责调用autofac框架实现业务逻辑层和数据仓储层程序集中的类型对象的创建
        /// 负责创建MVC控制器类的对象(调用控制器中的有参构造函数),接管DefaultControllerFactory的工作
        /// </summary>
        public static void Register()
        {
            var builder = new ContainerBuilder();
            var config = GlobalConfiguration.Configuration;
            var baseType = typeof(IDependency);
            var assemblys = BuildManager.GetReferencedAssemblies().Cast<Assembly>();
            builder.RegisterAssemblyTypes(assemblys.ToArray()).Where(t => baseType.IsAssignableFrom(t) && t != baseType).AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterControllers(Assembly.GetExecutingAssembly());//if autofac can be used in mvc
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());//if autofac can be used in webapi
            builder.RegisterType<DataContext>();//注入方法类
            var container = builder.Build();

            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

        }
    }
}