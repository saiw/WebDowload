using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Text;

using HtmlAgilityPack;
namespace ConsoleDownload.WebDownload
{
    class ExchangeRageByDay:AbstractDownload
    {
        #region 屬性
        private string content;
        private string charset;
        public override string URL
        {
            get { return "http://rate.bot.com.tw/Pages/Static/UIP003.zh-TW.htm"; }
        }

        public override string CharSet
        {
            get { return charset; }
        }

        public override string Params
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public override string Content
        {
            get
            {return content ;}
            set
            {content = value ;}
        }

        public override string FileName
        {
            get { return "每日匯率.html"; }
        }
        #endregion  
        public override void Download()
        {
            HttpWebRequest request;
            HttpWebResponse response;

            try
            {
                request = WebRequest.Create(this.URL) as HttpWebRequest ;
                if (request == null)
                    return;

                response = request.GetResponse() as HttpWebResponse ;
                if (response == null)
                    return;

                charset = response.CharacterSet ;  /*回應的編碼*/
                using (StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.Default))
                {content = sr.ReadToEnd();}
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
            // 直接讀取content 整份文件
            doc.LoadHtml(content);

            HtmlDocument docExchangerate = new HtmlDocument();
            
            //取Exchange Rate資料
            HtmlNode nodedata = 
                doc.DocumentNode.SelectSingleNode("/html[1]/body[1]/ul[1]/li[2]/center[1]/div[1]/div[2]/table[2]");
            docExchangerate.LoadHtml(nodedata.InnerHtml);

            //取得Header
            HtmlNodeCollection nodeHeaders = docExchangerate.DocumentNode.SelectNodes ("./tr[2]/td");
            //這裡取日圓
            //string[] values = docExchangerate.DocumentNode.SelectSingleNode("./tr[10]").InnerText.Trim ().Split ('\n');
            HtmlNodeCollection nodeValues = docExchangerate.DocumentNode.SelectNodes("./tr[10]/td");

            Console.WriteLine("幣別: {0}", ((HtmlNode)nodeValues[0]).InnerText.Replace("&nbsp;",""));
           
            HtmlNode header;
            HtmlNode value;
            for (int i = 0; i < nodeHeaders.Count; i++)
            {
                header = nodeHeaders[i];
                value = nodeValues[i + 1];
                Console.WriteLine("{0} :{1}", header.InnerText, value.InnerText);
            }

            doc = null;
            docExchangerate = null;
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
