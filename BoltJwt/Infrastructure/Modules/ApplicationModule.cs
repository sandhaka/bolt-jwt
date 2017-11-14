using Autofac;
using BoltJwt.Application.Queries;
using BoltJwt.Infrastructure.Security;
using Microsoft.AspNetCore.Authorization;

namespace BoltJwt.Infrastructure.Modules
{
    public class ApplicationModule : Module
    {
        public string ConnectionString { get; }

        public ApplicationModule(string connectionString)
        {
            ConnectionString = connectionString;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(compContext => new UserQueries(ConnectionString))
                .As<IUserQueries>()
                .InstancePerLifetimeScope();
            
            builder.RegisterType<AuthorizationsHandler>()
                .As<IAuthorizationHandler>()
                .SingleInstance();
        }
    }
}