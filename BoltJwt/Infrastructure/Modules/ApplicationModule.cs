using Autofac;
using BoltJwt.Domain.Model.Abstractions;
using BoltJwt.Infrastructure.Repositories;
using BoltJwt.Infrastructure.Security;
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