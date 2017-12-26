﻿using Autofac;
using BoltJwt.Application.Queries.Authorizations;
using BoltJwt.Application.Queries.Roles;
using BoltJwt.Application.Queries.Users;
using BoltJwt.Domain.Model.Abstractions;
using BoltJwt.Infrastructure.AppConfigurations;
using BoltJwt.Infrastructure.Repositories;
using BoltJwt.Infrastructure.Security;
using MailKit;
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

            builder.Register(compContext => new RoleQueries(ConnectionString))
                .As<IRoleQueries>()
                .InstancePerLifetimeScope();

            builder.Register(compContext => new AuthorizationQueries(ConnectionString))
                .As<IAuthorizationQueries>()
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

            builder.RegisterType<AuthorizationsHandler>()
                .As<IAuthorizationHandler>()
                .SingleInstance();

            builder.RegisterType<MailService>()
                .As<IMailService>()
                .SingleInstance();
        }
    }
}