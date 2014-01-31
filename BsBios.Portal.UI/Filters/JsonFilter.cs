using System;
using System.IO;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace BsBios.Portal.UI.Filters
{
    public class JsonFilter : ActionFilterAttribute
    {
        public string Param { get; set; }
        public Type JsonDataType { get; set; }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Request.ContentType.Contains("application/json"))
            {
                string inputContent;
                filterContext.HttpContext.Request.InputStream.Position = 0;
                using (var sr = new StreamReader(filterContext.HttpContext.Request.InputStream))
                {
                    inputContent = sr.ReadToEnd();
                }
                var serializer = new JavaScriptSerializer();
                var result = serializer.Deserialize(inputContent, JsonDataType);
               //var result = JsonConvert.DeserializeObject(inputContent, JsonDataType);
                filterContext.ActionParameters[Param] = result;
            }
        }

    }



}