using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Shared.DataLayer.Exceptions;
using System;
using System.Net;

namespace Shared.DataLayer.Attributes
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
	public class ValidationFilterAttribute : ExceptionFilterAttribute
	{
		public override void OnException(ExceptionContext context)
		{
			if (context.Exception is ValidationException exception)
			{
				context.HttpContext.Response.ContentType = "application/json";
				context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
				context.Result = new JsonResult(
					exception.Errors);

				return;
			}

			var code = HttpStatusCode.InternalServerError;

			if (context.Exception is NotFoundException)
			{
				code = HttpStatusCode.NotFound;
			}

			context.HttpContext.Response.ContentType = "application/json";
			context.HttpContext.Response.StatusCode = (int)code;
			context.Result = new JsonResult(new
			{
				error = new[] { context.Exception.Message }
			});
		}
	}
}
