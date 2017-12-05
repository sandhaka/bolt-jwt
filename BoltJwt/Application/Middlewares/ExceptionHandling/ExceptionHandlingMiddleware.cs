using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using BoltJwt.Application.Queries.QueryUtils;
using BoltJwt.Infrastructure.Repositories.Exceptions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace BoltJwt.Application.Middlewares.ExceptionHandling
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var jsonResponse = new ErrorPayload {StackTrace = exception.StackTrace};

            switch (exception)
            {
                case EntityNotFoundException entityNotFoundException:
                {
                    context.Response.StatusCode = (int) HttpStatusCode.NotFound;

                    jsonResponse.Message = entityNotFoundException.Message +
                                           $" - Entity: {entityNotFoundException.EntityTypeName}";
                    jsonResponse.StatusCode = (int) HttpStatusCode.NotFound;
                    break;
                }
                case NotEditableEntityException notEditableEntityException:
                {
                    context.Response.StatusCode = (int) HttpStatusCode.Forbidden;

                    jsonResponse.Message = notEditableEntityException.Message;
                    jsonResponse.StatusCode = (int) HttpStatusCode.Forbidden;
                    break;
                }
                case WrongCredentialsException wrongCredentialsException:
                {
                    context.Response.StatusCode = (int) HttpStatusCode.Forbidden;

                    jsonResponse.Message = wrongCredentialsException.Message;
                    jsonResponse.StatusCode = (int) HttpStatusCode.Forbidden;
                    break;
                }
                case PropertyIndexExistsException propertyIndexExistsException:
                {
                    context.Response.StatusCode = (int) HttpStatusCode.Conflict;

                    jsonResponse.Message = propertyIndexExistsException.Message +
                                           $" - Property: {propertyIndexExistsException.PropertyIndexName}";
                    jsonResponse.StatusCode = (int) HttpStatusCode.Conflict;
                    break;
                }
                case QueryFiltersFormatException queryFiltersFormatException:
                {
                    context.Response.StatusCode = (int) HttpStatusCode.BadRequest;

                    jsonResponse.Message = queryFiltersFormatException.Message +
                                           $" - Filters: {queryFiltersFormatException.Filters}" +
                                           $", Details: {queryFiltersFormatException.InnerException?.Message}";

                    jsonResponse.StatusCode = (int) HttpStatusCode.BadRequest;
                    break;
                }
                default:
                {
                    context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;

                    jsonResponse.Message = exception.Message;
                    jsonResponse.StatusCode = (int) HttpStatusCode.InternalServerError;
                    break;
                }
            }

            await context.Response.WriteAsync(JsonConvert.SerializeObject(jsonResponse), Encoding.UTF8);
        }
    }
}