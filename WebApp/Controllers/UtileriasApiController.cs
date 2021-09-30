using Business.Utilerias;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace WebApp.Controllers
{
    public class UtileriasApiController : ApiController
    {
        [HttpPost]
        public async Task<IHttpActionResult> Upload()
        {
            
            if (!Request.Content.IsMimeMultipartContent())
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);

            var provider = new MultipartMemoryStreamProvider();
            var result = await Request.Content.ReadAsMultipartAsync(provider);
            var filename = provider.Contents.First().Headers.ContentDisposition.FileName.Trim('\"');
            var buffer = await provider.Contents.First().ReadAsByteArrayAsync();

            string jsonString = provider.Contents[1].ReadAsStringAsync().Result;


            Sftp mySftp = Newtonsoft.Json.JsonConvert.DeserializeObject<Sftp>(jsonString);

            mySftp.subirArchivoArchivoBinarizado(buffer, filename);

            return Ok();
        }
    }
}
