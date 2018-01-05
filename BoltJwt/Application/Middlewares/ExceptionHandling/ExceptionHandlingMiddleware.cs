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

                    jsonResponse.Message = $"The object {entityNotFoundException.EntityTypeName} has not been found";
                    jsonResponse.Details = entityNotFoundException.Message +
                                           $" - Entity: {entityNotFoundException.EntityTypeName}";
                    jsonResponse.StatusCode = (int) HttpStatusCode.NotFound;
                    break;
                }
                case NotEditableEntityException notEditableEntityException:
                {
                    context.Response.StatusCode = (int) HttpStatusCode.Forbidden;

                    jsonResponse.Message = "The builtin object is not editable";
                    jsonResponse.Details = notEditableEntityException.Message;
                    jsonResponse.StatusCode = (int) HttpStatusCode.Forbidden;
                    break;
                }
                case WrongCredentialsException wrongCredentialsException:
                {
                    context.Response.StatusCode = (int) HttpStatusCode.Forbidden;

                    jsonResponse.Message = "Insufficient permissions";
                    jsonResponse.Details = wrongCredentialsException.Message;
                    jsonResponse.StatusCode = (int) HttpStatusCode.Forbidden;
                    break;
                }
                case DuplicatedIndexException duplicatedIndexException:
                {
                    context.Response.StatusCode = (int) HttpStatusCode.Conflict;

                    jsonResponse.Message = "There is another object with the same index. " +
                                           "The element can not be duplicated. " +
                                           $"Duplicated value: {duplicatedIndexException.DuplicatedValue}";
                    jsonResponse.Details = duplicatedIndexException.Message +
                                           $" - Property: {duplicatedIndexException.DuplicatedValue}";
                    jsonResponse.StatusCode = (int) HttpStatusCode.Conflict;
                    break;
                }
                case QueryFiltersFormatException queryFiltersFormatException:
                {
                    context.Response.StatusCode = (int) HttpStatusCode.BadRequest;

                    jsonResponse.Message = "Bad filters format";
                    jsonResponse.Details = queryFiltersFormatException.Message +
                                           $" - Filters: {queryFiltersFormatException.Filters}" +
                                           $", Details: {queryFiltersFormatException.InnerException?.Message}";

                    jsonResponse.StatusCode = (int) HttpStatusCode.BadRequest;
                    break;
                }
                case EntityInUseException entityInUseException:
                {
                    context.Response.StatusCode = (int) HttpStatusCode.BadRequest;

                    jsonResponse.Message = $"Entity '{entityInUseException.Value}' is in use";
                    jsonResponse.Details = entityInUseException.Message;

                    jsonResponse.StatusCode = (int) HttpStatusCode.BadRequest;
                    break;
                }
                default:
                {
                    context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;

                    jsonResponse.Message = "Unrecognized error. See the error details for more info";
                    jsonResponse.Details = ParseExceptionMessages(exception);
                    jsonResponse.StatusCode = (int) HttpStatusCode.InternalServerError;
                    break;
                }
            }

            await context.Response.WriteAsync(JsonConvert.SerializeObject(jsonResponse), Encoding.UTF8);
        }

        private static string ParseExceptionMessages(Exception e)
        {
            var ret = e.Message + Environment.NewLine;

            if(e.InnerException != null)
            {
                ret += ParseExceptionMessages(e.InnerException);
            }

            return ret;
        }
    }
}