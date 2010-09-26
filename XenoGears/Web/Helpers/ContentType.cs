using System;
using System.Diagnostics;

namespace XenoGears.Web.Helpers
{
    [DebuggerNonUserCode]
    public class ContentType
    {
        public String Mime { get; private set; }
        public bool IsBinary { get { return IsMimeTypeBinary(Mime); } }
        public bool IsText { get { return !IsMimeTypeBinary(Mime); } }

        public ContentType(String mime)
        {
            // this allows us to correctly process ctypes like "text/html; charset=utf-8"
            // todo. investigate the MIME spec and implement this thouroughly
            var sliceAt = mime.IndexOf(";") == -1 ? mime.Length : mime.IndexOf(";");
            Mime = mime.Substring(0, sliceAt); ;
        }

        public static implicit operator ContentType(String mime)
        {
            return new ContentType(mime);
        }

        public static implicit operator String(ContentType contentType)
        {
            return contentType.Mime;
        }

        private static bool IsMimeTypeBinary(String mime)
        {
            switch (mime)
            {
                case "application/json":
                case "application/xml":
                case "application/atom+xml":
                case "text/css":
                case "text/javascript":
                case "text/html":
                case "multipart/mixed":
                case "application/x-www-form-urlencoded":
                    return false;
                case "image/gif":
                case "image/jpeg":
                case "image/png":
                case "image/x-emz":
                    return true;
                default:
                    throw new NotSupportedException(Environment.NewLine + String.Format(
                        "Failed to understand the '{0}' mime type.", mime));
            }
        }

        public static ContentType FromFileExtension(String ext)
        {
            switch (ext)
            {
                case "html": 
                case "htm":
                    return "text/html";
                case "js":
                    return "text/javascript";
                case "css":
                    return "text/css";
                case "png":
                    return "image/png";
                case "jpeg":
                case "jpg":
                    return "image/jpeg";
                case "emz":
                    return "image/x-emz";
                case "gif":
                    return "image/gif";
                case "xml":
                    return "application/xml";
                default:
                    throw new NotSupportedException(Environment.NewLine + String.Format(
                        "Failed to understand the '{0}' file extension.", ext));
            }
        }

        public override String ToString()
        {
            return Mime;
        }
    }
}