using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;

namespace timelog.net.Exceptions
{
    public class ControllerException : Exception
    {
        public ControllerException() : base()
        {
        }

        public ControllerException(string? message) : base(message)
        {
        }

        public ControllerException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
        public ControllerException(HttpStatusCode statusCode, string? message = null) : base(message)
        {
            StatusCode = statusCode;
        }

        public ControllerException(ActionResult actionResult) : base()
        {
            Result = actionResult;
        }

        public HttpStatusCode StatusCode { get; } = HttpStatusCode.InternalServerError;

        public ActionResult? Result { get; }
    }
}
