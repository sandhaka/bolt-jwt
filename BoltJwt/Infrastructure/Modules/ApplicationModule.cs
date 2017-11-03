using Autofac;
using BoltJwt.Infrastructure.Repositories;
using BoltJwt.Infrastructure.Security;
using BoltJwt.Model.Abstractions;
using Microsoft.AspNetCore.Authorization;

namespace BoltJwt.Infrastructure.Modules
{
    public class ApplicationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UserRepository>()
                .As<IUserRepository>()
                .InstancePerLifetimeScope();
            
            builder.RegisterType<AuthorizationsHandler>()
                .As<IAuthorizationHandler>()
                .SingleInstance();
        }
    }
}