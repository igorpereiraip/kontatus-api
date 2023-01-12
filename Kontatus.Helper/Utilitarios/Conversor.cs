using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.Azure.WebJobs;
using Newtonsoft.Json;

namespace ConsigIntegra.Helper.Utilitarios
{
    public static class Conversor
    {
        public static string ConverterObjeto(object entidade)
        {
            return JsonConvert.SerializeObject(entidade, Formatting.None, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        }

        public static byte[] ConverterHtmlPdf(IConverter converter, string htmlDoc)
        {
            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = {
                    ColorMode = ColorMode.Color,
                    Orientation = Orientation.Portrait,
                    PaperSize = PaperKind.A4,
                    Margins = new MarginSettings() { Top = 10, Left = 30, Right = 30, Bottom = 10 }
                },
                Objects = {
                     new ObjectSettings() {
                         PagesCount = true,
                         HtmlContent = htmlDoc.ToString(),
                         WebSettings = { DefaultEncoding = "utf-8" }
                     }
                }
            };

            return converter.Convert(doc);
        }
    }
}
