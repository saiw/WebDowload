using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Text;


#region private namespace import
using HtmlAgilityPack;
#endregion
namespace ConsoleDownload.WebDownload
{
    class DayStockDowload:AbstractDownload 
    {

        #region 屬性欄位
        private string content;
        private string pid = "2317";
        private string charset;
        public override string URL
        {
            get {return  @"http://tw.stock.yahoo.com/q/q?" + this.Params ; }
        }

        public override string Params
        {
            get { return "s="+pid; }
        }

        public override string Content
        {
            get { return content; }
            set { content = value; }
        }

        public override string FileName
        {
            get { return "每日股市行情.html"; }
        }

        public override string CharSet
        {
            get { return charset ; }
        }
        #endregion 

        public override void Download()
        {
            HttpWebRequest request;
            HttpWebResponse response;
            try
            {
                //httpWebRequest無法初始化 ,用WebRequest Create 方法建立
                request = WebRequest.Create(this.URL) as HttpWebRequest;
                request.KeepAlive = false;

                //request.Method = "GET";
                response = request.GetResponse() as HttpWebResponse;   
                charset  = response.CharacterSet.ToLower(); 
                using (StreamReader sr = new StreamReader(response.GetResponseStream() ,Encoding.Default))
                {
                    Content = sr.ReadToEnd();    
                }
            }
            catch (Exception ex)
            {
                lasterror = ex.Message;
            }
            finally
            {
                request = null;
                response = null;
            }
 
        }

        public override void Parse()
        {
            if (String.IsNullOrEmpty(content) || content == "")
                return;

            HtmlDocument doc = new HtmlDocument();

            //doc.Load(webcontent ,Encoding.Default); //這個只能load Memory
            doc.LoadHtml(content);
            //建立stock資料
            HtmlDocument docStockContent = new HtmlDocument();
            HtmlNode htmlnode =
                doc.DocumentNode.SelectSingleNode("/html[1]/body[1]/center[1]/table[2]/tr[1]/td[1]/table[1]") ;
            docStockContent.LoadHtml(htmlnode.InnerHtml );

            //取得個股標頭
            HtmlNodeCollection nodeHeaders = docStockContent.DocumentNode.SelectNodes("./tr[1]/th");

            //取得個股values array
            string[] values = docStockContent.DocumentNode.SelectSingleNode("./tr[2]").InnerText.Trim().Split('\n');
            HtmlNode nodeheader;
            for (int i = 0; i < nodeHeaders.Count; i++)
            {
                nodeheader = nodeHeaders[i];
                Console.WriteLine(" Header: {0} ,Value: {1}", nodeheader.InnerText, values[i].Trim());
            }

            //只能取到 meta 的content
            //HtmlNode nodemetadata = doc.DocumentNode.SelectSingleNode("/html[1]/head[1]//meta[1]");
            //charset = nodemetadata.GetAttributeValue("content", "");


            doc = null;
            docStockContent = null; 

        }
        public override void SaveFile()
        {
            if (Content.Length == 0 || Content == null)
                return;
            string directorypath = this.path +"temp";

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
