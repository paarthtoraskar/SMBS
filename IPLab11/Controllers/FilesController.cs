﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Http;

namespace IPLab11.Controllers
{
    public class CustomMultipartFormDataStreamProvider : MultipartFormDataStreamProvider
    {
        public CustomMultipartFormDataStreamProvider(string path)
            : base(path)
        {
        }

        public override string GetLocalFileName(HttpContentHeaders headers)
        {
            string name = !string.IsNullOrWhiteSpace(headers.ContentDisposition.FileName)
                ? headers.ContentDisposition.FileName
                : "NoName";
            return name.Replace("\"", string.Empty);
            //this is here because Chrome submits files in quotation marks which get treated as part of the filename and get escaped
        }
    }

    public class FilesController : ApiController
    {
        // GET api/files
        [HttpGet]
        public HttpResponseMessage Get()
        {
            try
            {
                IEnumerable<string> files = Directory.GetFiles(GetFileDepotPathInDomain()).Select(Path.GetFileName);
                var responseMessage = new HttpResponseMessage(HttpStatusCode.OK);
                responseMessage.Content = new ObjectContent<IEnumerable<string>>(files, new JsonMediaTypeFormatter());
                return responseMessage;
            }
            catch
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        // GET api/files/5
        [HttpGet]
        public HttpResponseMessage Get(int id)
        {
            try
            {
                string[] files = Directory.GetFiles(GetFileDepotPathInDomain());
                var fileStream = new FileStream(files.ElementAt(id), FileMode.Open, FileAccess.Read);
                var responseMessage = new HttpResponseMessage(HttpStatusCode.OK);
                responseMessage.Content = new StreamContent(fileStream);
                return responseMessage;
            }
            catch (IOException)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        // POST api/files
        [HttpPost]
        public Task Post()
        {
            if (Request.Content.IsMimeMultipartContent())
            {
                var streamProvider = new CustomMultipartFormDataStreamProvider(GetFileDepotPathInDomain());
                Task<CustomMultipartFormDataStreamProvider> task = Request.Content.ReadAsMultipartAsync(streamProvider);
                return task;
            }
            throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotAcceptable,
                "This request is not properly formatted"));
        }

        private string GetFileDepotPathInDomain()
        {
            string appRoot =
                Directory.GetParent(Directory.GetParent(HostingEnvironment.MapPath("~")).ToString()).ToString();
            string fileDepotPathInDomain = appRoot + "\\SBMS" + "\\Products";
            if (!Directory.Exists(fileDepotPathInDomain))
                Directory.CreateDirectory(fileDepotPathInDomain);
            return fileDepotPathInDomain;
        }

        public class FileDesc
        {
            public FileDesc(string name, string path, long size)
            {
                Name = name;
                Path = path;
                Size = size;
            }

            public string Name { get; set; }
            public string Path { get; set; }
            public long Size { get; set; }
        }
    }
}