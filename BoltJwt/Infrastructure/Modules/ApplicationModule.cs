using Autofac;
using BoltJwt.Infrastructure.Repositories;
using BoltJwt.Model.Abstractions;

namespace BoltJwt.Infrastructure.Modules
{
    public class ApplicationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UserRepository>().As<IUserRepository>().InstancePerLifetimeScope();
        }
    }
}