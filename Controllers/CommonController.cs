using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Baganova.Controllers
{
    public class CommonController : Controller
    {
        public string DecryptPassword(string password)
        {
            byte[] b;
            string decrypted;
            try
            {
                b = Convert.FromBase64String(password);
                decrypted = System.Text.ASCIIEncoding.ASCII.GetString(b);
            }
            catch (FormatException fe)
            {
                decrypted = "";
            }
            return decrypted;
        }

        public string EnryptPassword(string password)
        {
            byte[] b = System.Text.ASCIIEncoding.ASCII.GetBytes(password);
            string encrypted = Convert.ToBase64String(b);
            return encrypted;
        }

        public static string GetUniqueFileName(string filename)
        {
            //string uniquefilename = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time")).ToString("yyyyMMddHHmmss") + filename;
            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time")).ToString("yyyyMMddHHmmss") + filename.Replace(" ","");
        }
    }
}