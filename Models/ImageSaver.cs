using Baganova.Controllers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Baganova.Models
{
    public class ImageSaver
    {
        private ImageSaver()
        {

        }

        private static ImageSaver obj { get; set; }

        public static ImageSaver CreateObject()
        {
            if (obj == null)
            {
                obj = new ImageSaver();
            }
            return obj;
        }

        public string SaveImage(HttpPostedFileBase File)
        {
            string _FileName = Path.GetFileName(File.FileName);
            string _path = Path.Combine(HttpContext.Current.Server.MapPath("~/Images/Products/"), CommonController.GetUniqueFileName(_FileName));
            string fileuploaded = "./../Images/Products/" + CommonController.GetUniqueFileName(_FileName);
            File.SaveAs(_path);
            return fileuploaded;
        }

        public void Delete(string path)
        {
            string Paths = path;
            string Filename = Path.GetFileName(Paths);
            string fullpath = "~/Images/Products/" + Filename;
            string oldimage = HttpContext.Current.Request.MapPath(fullpath);

            // TODO:AS improment Requiremet
            if (System.IO.File.Exists(oldimage))
            {
                System.IO.File.Delete(oldimage);
            }
        }

    }
}