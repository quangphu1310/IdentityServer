﻿using System.Net;

namespace IdentityServer.Models
{
    public class APIResponse
    {
        public APIResponse()
        {
            Errors = new List<string>();
        }
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; } = true;
        public List<string> Errors { get; set; }
        public object Result { get; set; }
    }
}
