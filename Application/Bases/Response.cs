using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Application.Bases
{
    public class Response<T>
    {
        public HttpStatusCode StatusCode { get; set; }
        public object Meta { get; set; }
        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public List<string> Error { get; set; }
        public T Data { get; set; }

        public Response() 
        {
            Succeeded = false;
            Error = new List<string>();
        }

        public Response(T data, string message = "Success")
        {
            Data = data;
            Message = message;
            Succeeded = true;
            StatusCode = HttpStatusCode.OK;
            Error = new List<string>();
        }

        public Response(string message, bool succeeded = false) 
        {
            Succeeded = succeeded;
            Message = message;
            Error = new List<string>();
        }

        public Response(List<string> errors, string message = "Errors occurred")
        {
            Error = errors ?? new List<string>();
            Message = message;
            Succeeded = false;
        }
    }
}
