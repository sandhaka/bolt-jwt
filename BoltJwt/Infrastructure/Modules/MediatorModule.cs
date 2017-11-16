using System.Collections.Generic;
using Autofac;
using BoltJwt.Application.Commands.Users;
using System.Reflection;
using BoltJwt.Application.Commands.Authorizations;
using BoltJwt.Application.Commands.Roles;
using BoltJwt.Application.Validations;
using FluentValidation;
using MediatR;

namespace BoltJwt.Infrastructure.Modules
{
    public class MediatorModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly)
                .AsImplementedInterfaces();

            /**
             * Register all the Command classes (they implement IAsyncRequestHandler) in assembly holding the Commands
             */

            #region [Users]

            builder.RegisterAssemblyTypes(typeof(UserInsertCommand).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(IAsyncRequestHandler<,>));

            builder.RegisterAssemblyTypes(typeof(UserEditCommand).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(IAsyncRequestHandler<,>));

            builder.RegisterAssemblyTypes(typeof(UserDeleteCommand).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(IAsyncRequestHandler<,>));

            builder.RegisterAssemblyTypes(typeof(AddAuthorizationUserCommand).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(IAsyncRequestHandler<,>));

            builder
                .RegisterAssemblyTypes(typeof(UserInsertCommandValidator).GetTypeInfo().Assembly)
                .Where(t => t.IsClosedTypeOf(typeof(IValidator<>)))
                .AsImplementedInterfaces();

            builder
                .RegisterAssemblyTypes(typeof(AddAuthorizationUserCommandValidator).GetTypeInfo().Assembly)
                .Where(t => t.IsClosedTypeOf(typeof(IValidator<>)))
                .AsImplementedInterfaces();

            #endregion

            #region [Roles]

            builder.RegisterAssemblyTypes(typeof(RoleInsertCommand).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(IAsyncRequestHandler<,>));

            builder.RegisterAssemblyTypes(typeof(RoleEditCommand).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(IAsyncRequestHandler<,>));

            builder.RegisterAssemblyTypes(typeof(RoleDeleteCommand).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(IAsyncRequestHandler<,>));

            builder
                .RegisterAssemblyTypes(typeof(RoleInsertCommandValidator).GetTypeInfo().Assembly)
                .Where(t => t.IsClosedTypeOf(typeof(IValidator<>)))
                .AsImplementedInterfaces();

            #endregion

            #region [Authorizations]

            builder.RegisterAssemblyTypes(typeof(AuthorizationInsertCommand).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(IAsyncRequestHandler<,>));

            builder
                .RegisterAssemblyTypes(typeof(AuthorizationInsertCommandValidator).GetTypeInfo().Assembly)
                .Where(t => t.IsClosedTypeOf(typeof(IValidator<>)))
                .AsImplementedInterfaces();

            #endregion

            builder.Register<SingleInstanceFactory>(context =>
            {
                var componentContext = context.Resolve<IComponentContext>();
                return t => { object o; return componentContext.TryResolve(t, out o) ? o : null; };
            });

            builder.Register<MultiInstanceFactory>(context =>
            {
                var componentContext = context.Resolve<IComponentContext>();

                return t =>
                {
                    var resolved = (IEnumerable<object>)componentContext.Resolve(typeof(IEnumerable<>).MakeGenericType(t));
                    return resolved;
                };
            });

            builder.RegisterGeneric(typeof(ValidatorPipeline<,>)).As(typeof(IPipelineBehavior<,>));
        }
    }
}