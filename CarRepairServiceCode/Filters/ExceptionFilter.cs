using CarRepairServiceCode.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRepairServiceCode.Filters
{
    public class ExceptionFilter : Attribute, IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            switch (context.Exception)
            {
                case UnauthorizedException e:
                    string exceptionMessage = context.Exception.Message;
                    context.Result = new ContentResult
                    {
                        Content = exceptionMessage,
                        StatusCode = 401
                    };
                    context.ExceptionHandled = true;
                    break;

                case PermissionException e:
                    exceptionMessage = context.Exception.Message;
                    context.Result = new ContentResult
                    {
                        Content = exceptionMessage,
                        StatusCode = 403
                    };
                    context.ExceptionHandled = true;
                    break;

                case NotFoundException e:
                    exceptionMessage = context.Exception.Message;
                    context.Result = new ContentResult
                    {
                        Content = exceptionMessage,
                        StatusCode = 404
                    };
                    context.ExceptionHandled = true;
                    break;
            }
        }
    }
}
