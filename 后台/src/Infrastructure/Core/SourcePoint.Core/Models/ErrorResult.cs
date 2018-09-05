using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SourcePoint.Core.Models
{
    public class ErrorResult<T> : IActionResult
    {
        public T Error { get; set; }
        public int? StatusCode { get; set; }
        public ErrorResult(T error)
        {
            Error = error;
        }
        public Task ExecuteResultAsync(ActionContext context)
        {
            var result = new ObjectResult(this);
            if (StatusCode.HasValue) result.StatusCode = StatusCode.Value;
            return result.ExecuteResultAsync(context);
        }
    }

    public class ErrorResult : ErrorResult<string>
    {
        public ErrorResult(string error) : base(error)
        {

        }

        public ErrorResult(int statusCode, string error) : base(error)
        {
            this.StatusCode = statusCode;
        }

        public ErrorResult(string errorFormat, params string[] args) : base(null)
        {
            base.Error = string.Format(errorFormat, args);
        }
    }
}
