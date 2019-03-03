using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

namespace DatingApp.API.Common
{
    public class ApiResult<T> : ActionResult
    {
        public int StatusCode { get; set; }
        public bool Status { get { return StatusCode == (int)HttpStatusCode.OK || StatusCode == (int)HttpStatusCode.Created; } }
        public string Message { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public T Data { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<ModelError> ModelErrors { get; set; }

        //2XX range status codes
        public static ApiResult<T> Ok(T data = default(T))
        {
            return new ApiResult<T>
            {
                StatusCode = (int)HttpStatusCode.OK, // 200
                Data = data
            };
        }
        public static ApiResult<T> Created(T data = default(T))
        {
            return new ApiResult<T>
            {
                StatusCode = (int)HttpStatusCode.Created, // 201
                Data = data
            };
        }
        //4XX range status codes
        public static ApiResult<T> BadRequest(string message)
        {
            return new ApiResult<T>
            {
                StatusCode = (int)HttpStatusCode.BadRequest, // 400
                Message = message
            };
        }
        public static ApiResult<T> BadRequest(ModelStateDictionary modelState)
        {
            return new ApiResult<T>
            {
                StatusCode = (int)HttpStatusCode.BadRequest, // 400
                ModelErrors = modelState.SelectMany(m => m.Value.Errors.Select(me => new ModelError
                {
                    FieldName = m.Key,
                    ErrorMessage = !string.IsNullOrEmpty(me.ErrorMessage) ? me.ErrorMessage : (me.Exception != null ? me.Exception.Message : $"{m.Key} is invalid")
                }))
            };
        }
        public static ApiResult<T> Forbidden(string message)
        {
            return new ApiResult<T>
            {
                StatusCode = (int)HttpStatusCode.Forbidden, // 403
                Message = message
            };
        }
        public static ApiResult<T> NotFound(string message)
        {
            return new ApiResult<T>
            {
                StatusCode = (int)HttpStatusCode.NotFound, // 404
                Message = message
            };
        }
        public static ApiResult<T> Conflict(string message)
        {
            return new ApiResult<T>
            {
                StatusCode = (int)HttpStatusCode.Conflict, // 409
                Message = message
            };
        }
        //5XX range status codes
        public static ApiResult<T> InternalServerError(string message)
        {
            return new ApiResult<T>
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
                Message = message
            };
        }

        public override Task ExecuteResultAsync(ActionContext context)
        {
            context.HttpContext.Response.StatusCode = StatusCode;
            var json = new JsonResult(this);
            return json.ExecuteResultAsync(context);
        }

        public class ModelError
        {
            public string FieldName { get; set; }
            public string ErrorMessage { get; set; }
        }
    }
}