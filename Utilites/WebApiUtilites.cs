using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using Microsoft.Win32;

namespace Common.Utilites
{
    public static class WebApiUtilites
    {
        public static HttpResponseMessage CreateResponseFile(string path, string name, string ext)
        {
            using (var stream = File.OpenRead(path))
            {
                return CreateResponseFile(stream, name, ext);
            }
        }

        public static HttpResponseMessage CreateResponseFile(byte[] content, string name, string ext)
        {
            using (var stream = new MemoryStream(content))
            {
                return CreateResponseFile(stream, name, ext);
            }
        }

        public static HttpResponseMessage CreateResponseFile(Stream content, string name, string ext)
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StreamContent(content)
            };

            var contenttype = "application/" + ext;
            var fileextkey = Registry.ClassesRoot.OpenSubKey("." + ext);
            if (fileextkey != null)
            {
                contenttype = fileextkey.GetValue("Content Type", contenttype).ToString();
            }

            if (HttpContext.Current.Request.Browser.IsBrowser("IE"))
            {
                name = (HttpUtility.UrlEncode(name) ?? "").Replace("+", " ");
            }

            response.Headers.CacheControl = new CacheControlHeaderValue();
            response.Content.Headers.ContentLength = content.Length;
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = name + '.' + ext
            };
            response.Content.Headers.ContentType = new MediaTypeHeaderValue(contenttype)
            {
                CharSet = Encoding.Unicode.ToString()
            };

            return response;
        }
    }
}