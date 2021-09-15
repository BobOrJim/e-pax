using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MVC.ViewModels
{
    public class API1SecretViewModel
    {
        public string SecretMessage { get; set; }
        public HttpResponseMessage httpResponseMessage { get; set; }
    }
}
