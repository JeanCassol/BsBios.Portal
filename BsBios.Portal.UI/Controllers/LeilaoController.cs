using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BsBios.Portal.UI.Controllers
{
    public class LeilaoController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        [HttpGet]
        public Mensagem HelloWorld()
        {
            return new Mensagem(){Status = "OK", Descricao = "Hello World"};
        }

        [HttpGet]
        public IList<Mensagem> GetMensagens()
        {
            return new List<Mensagem>()
                {
                    new Mensagem(){Status="OK", Descricao = "Status OK"},
                    new Mensagem(){Status = "Erro", Descricao = "Status Erro"}
                };
        }

        // POST api/<controller>
        public HttpResponseMessage Post(Mensagem mensagem)
        {
            return Request.CreateResponse(HttpStatusCode.OK, "Status: " + mensagem.Status + " - Descrição: " + mensagem.Descricao);
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]Mensagem mensagem)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }

    public class Mensagem
    {
        public String Status { get; set; }
        public String Descricao { get; set; }
    }

}