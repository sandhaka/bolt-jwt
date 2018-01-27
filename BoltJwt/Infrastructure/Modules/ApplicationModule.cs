using Autofac;
using BoltJwt.Application.Queries.Authorizations;
using BoltJwt.Application.Queries.Groups;
using BoltJwt.Application.Queries.Logs;
using BoltJwt.Application.Queries.Roles;
using BoltJwt.Application.Queries.Users;
using BoltJwt.Application.Services;
using BoltJwt.Domain.Model.Abstractions;
using BoltJwt.Infrastructure.Repositories;
using BoltJwt.Infrastructure.Security;
using Microsoft.AspNetCore.Authorization;

namespace BoltJwt.Infrastructure.Modules
{
    public class ApplicationModule : Module
    {
        private readonly string _connectionString;

        public ApplicationModule(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(compContext => new UserQueries(_connectionString))
                .As<IUserQueries>()
                .InstancePerLifetimeScope();

            builder.Register(compContext => new RoleQueries(_connectionString))
                .As<IRoleQueries>()
                .InstancePerLifetimeScope();

            builder.Register(compContext => new GroupQueries(_connectionString))
                .As<IGroupQueries>()
                .InstancePerLifetimeScope();

            builder.Register(compContext => new AuthorizationQueries(_connectionString))
                .As<IAuthorizationQueries>()
                .InstancePerLifetimeScope();

            builder.Register(compContext => new TokenLogsQueries(_connectionString))
                .As<ITokenLogsQueries>()
                .InstancePerLifetimeScope();

            builder.RegisterType<UserRepository>()
                .As<IUserRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<RoleRepository>()
                .As<IRoleRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<AuthorizationRepository>()
                .As<IAuthorizationRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<ConfigurationRepository>()
                .As<IConfigurationRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<TokenLogsRepository>()
                .As<ITokenLogsRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<AuthorizationsHandler>()
                .As<IAuthorizationHandler>()
                .SingleInstance();

            builder.RegisterType<MailService>()
                .As<IMailService>()
                .SingleInstance();
        }
    }
}