using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace OTB_SEGURA.Models
{
    public class ResponseHTTP<T>
    {
        public List<T> Data { get; set; }
        public string Msj { get; set; }
        public HttpStatusCode Code { get; set; }
    }
}
