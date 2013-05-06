﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using System.Threading;
using System.Web.UI;
using System.IO;
using BsBios.Portal.Infra.Services.Contracts;

namespace MvcTesting.Controllers.WebApi
{
    public class UploadController : ApiController
    {
        private readonly IFileService _fileService;

        public UploadController(IFileService fileService)
        {
            _fileService = fileService;
        }

        // Enable both Get and Post so that our jquery call can send data, and get a status
        [HttpGet]
        [HttpPost]
        public HttpResponseMessage Upload()
        {
            try
            {

                // Get a reference to the file that our jQuery sent.  Even with multiple files, they will all be their own request and be the 0 index
                HttpPostedFile file = HttpContext.Current.Request.Files[0];

                // do something with the file in this space 
                // {....}
                // end of file doing
                string caminhoDoArquivo = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Uploads", HttpContext.Current.Request.Form["IdProcessoCotacao"],file.FileName);

                _fileService.Save(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Uploads"),
                    HttpContext.Current.Request.Form["IdProcessoCotacao"],file.FileName,file.InputStream);

                // Now we need to wire up a response so that the calling script understands what happened
                HttpContext.Current.Response.ContentType = "text/plain";
                var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                var result = new { name = file.FileName };

                HttpContext.Current.Response.Write(serializer.Serialize(result));
                HttpContext.Current.Response.StatusCode = 200;

                // For compatibility with IE's "done" event we need to return a result as well as setting the context.response
                return new HttpResponseMessage(HttpStatusCode.OK);
            }

            catch (Exception)
            {

                throw;
            }

        }
    }
}
