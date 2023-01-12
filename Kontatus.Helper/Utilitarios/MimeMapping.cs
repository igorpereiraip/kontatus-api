using System;
using System.Collections.Generic;
using System.Text;

namespace ConsigIntegra.Helper.Utilitarios
{
    public static class MimeMapping
    {
        private static readonly Dictionary<string, string> MAPPING = new Dictionary<string, string>
        {
            [".pdf"] = "application/pdf",
            [".png"] = "image/png",
            [".gif"] = "image/gif",
            [".jpg"] = "image/jpeg",
            [".jpeg"] = "image/jpeg",
            [".msg"] = "application/vnd.ms-outlook",
            [".doc"] = "application/msword",
            [".docx"] = "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            [".xls"] = "application/vnd.ms-excel",
            [".xlsx"] = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            [".zip"] = "application/x-zip-compressed",
        };

        public static string GetMapping(string fileName)
        {
            if (fileName != null)
                return MAPPING[System.IO.Path.GetExtension(fileName).ToLower()];
            else
                return null;
        }
    }
}
