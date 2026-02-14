using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace CleanArchitecture.Application.Bases
{
    public class Response<T>
    {
        public Response() 
        {

        }

        public Response(T data, string message)
        {
            Data = data;
            Message = message;
        }

        public Response(bool success, string message) 
        {
            Succeeded = success;
            Message = message;
        }

        public Response(string message) 
        {
            Succeeded = false;
            Message = message;
        }

        public HttpStatusCode StatusCode { get; set; }
        public object Meta { get; set; }

        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public List<string> Error { get; set; }
        public T Data { get; set; }

    }
}
