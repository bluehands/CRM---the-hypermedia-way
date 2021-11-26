using CRM.Domain;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace CRM.Server
{
    public class CustomerImageFormatter : OutputFormatter
    {
        public CustomerImageFormatter()
        {
            SupportedMediaTypes.Add("image/png");
            SupportedMediaTypes.Add("image/jpeg");
        }
        protected override bool CanWriteType(Type? type)
        {
            if (typeof(Customer).IsAssignableFrom(type))
            {
                return base.CanWriteType(type);
            }
            return false;
        }
        public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context)
        {
            var response = context.HttpContext.Response;
            var c = context.Object as Customer;
            var imageUrl = c?.PictureUrl;
            if (imageUrl != null)
            {
                var httpClient = new HttpClient();
                using var responseStream = response.BodyWriter.AsStream();
                using var imageStream = httpClient.GetAsync(imageUrl).Result.Content.ReadAsStream();
                while (true)
                {
                    const int bufSize = 1024 * 1024;
                    var buf = new byte[bufSize];
                    var count = imageStream.ReadAsync(buf, 0, bufSize).Result;
                    responseStream.WriteAsync(buf, 0, count);
                    if (count <= 0) { break; }
                }
                response.BodyWriter.CompleteAsync().GetAwaiter().GetResult();
                //Image image = Image.FromStream();
                //return Task.Run(() => image.Save(response.Body, ImageFormat.Jpeg));
            }
            return Task.CompletedTask;
        }
    }
}