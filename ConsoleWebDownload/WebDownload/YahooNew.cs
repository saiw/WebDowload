/*TO DO 
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Net;

namespace ConsoleDownload.WebDownload
{
    class YahooNew:AbstractDownload 
    {
        private string charset = "utf-8"; //預設utf-8
        private string content;
        public override string URL
        {
            get { return "http://tw.news.yahoo.com/"; }
        }

        public override string Params
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public override string Content
        {
            get
            {
               return content;
            }
            set
            {
                content =value ;
            }
        }

        public override string FileName
        {
            get { return "yahoo 奇摩新聞.html"; }
        }

        public override string CharSet
        {
            get { return charset; }
        }

        public override void Download()
        {
            WebClient wc = new WebClient();
            //加入一些header ,用來騙某些web
            wc.Headers.Add("Accept" ,"image/gif, image/x-xbitmap ,image/jpeg ,image/pjpeg ,*/*");
            wc.Headers.Add("Accept-Language","zh-TW");
            wc.Headers.Add("User-Agent" ,"Mozilla/4.0(comptible; MSIE 6.0;Window NT 5.2;SV1; .NET CLR 1.1.4322; .NET CLR2.0.50727)");
            
            Stream s = wc.OpenRead(this.URL);
           
            using (StreamReader sr = new StreamReader(s, Encoding.UTF8 ))
            {
                content = sr.ReadToEnd();
            }
           
        }

        public override void Parse()
        {
            //throw new Exception("The method or operation is not implemented.");
        }

        public override void SaveFile()
        {
            if (Content.Length == 0 || Content == null)
                return;
            string directorypath = this.path + "temp";

            if (!Directory.Exists(directorypath))
            { Directory.CreateDirectory(directorypath); }

            string filepath = directorypath + "\\" + FileName;
            if (File.Exists(filepath))
            { File.Delete(filepath); };///直接刪掉存取較快

            //根據charset的類型寫出
            try
            {
                using (StreamWriter sw = new StreamWriter(filepath, false, Encoding.GetEncoding(this.charset)))//檔案存在覆寫檔案
                {

                    sw.WriteLine(string.Format("<meta http-equiv=\"content-type\" content=\"text/html;charset={0}\">", this.charset));
                    sw.WriteLine(Content);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
