using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Net;

namespace Dowload
{
    public class WebHelper
    {
        #region 屬性欄位

        private string webcontent;
        private string error;
        /// <summary>
        /// 回傳URL ,這裡直接回傳設定好參數的url
        /// </summary>
        private string URL
        {
            get
            {
                return "http://www.otc.org.tw/ch/stock/aftertrading/otc_quotes_no1430/SQUOTE_ww_1000328.html";
            }
        }
        private string Params
        {
            get { return ""; }
        }
        private string WebContent
        {
            get { return webcontent; }
            set { webcontent = value; }
        }
        private string FileName
        {
            get { return "Test"; }
        }
        public string ErrorMessage
        {
            get { return error; }
            set { error = value; }
        }
        #endregion
        /// <summary>
        /// 用HttpWebRequest 取得網頁內容(目前僅Get)
        /// </summary>
        private  void GetWebRequest()
        {
            HttpWebRequest request = WebRequest.Create(this.URL) as HttpWebRequest;  //利用 as轉型
            HttpWebResponse response = request.GetResponse() as HttpWebResponse ;   //利用 as轉型

            using (StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.Default))
            {
                WebContent = sr.ReadToEnd();
            }
            request = null;
            response = null;
           
            
        }

        /// <summary>
        /// 利用WebClient.DowloaData(目前僅Get)
        /// </summary>
        private void GetQuery()
        {
            WebClient client = new WebClient();

            byte[] tempweb =
                client.DownloadData(this.URL +"?"+this.Params );
            WebContent = System.Text.Encoding.Default.GetString(tempweb);

        }
        
        private void SaveFile()
        {
            if (WebContent.Length == 0||WebContent ==null)
                return;
            string directorypath = "C:/temptest";
            if (!Directory.Exists(directorypath))
            { Directory.CreateDirectory(directorypath); }

            string filepath = directorypath + "\\" + FileName + ".html";
            if (File.Exists(filepath))
            { File.Delete(filepath); };///直接刪掉存取較快
            try
            {
                using (StreamWriter sw = new StreamWriter(filepath, false))//檔案存在覆寫檔案
                {
                    sw.WriteLine("<meta http-equiv=\"content-type\" content=\"text/html;charset=utf-8\">");
                    sw.WriteLine(WebContent);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Excute()
        {
            try
            {
                //GetQuery();
                GetWebRequest();
                SaveFile();  
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return false;
            }
            return true;
            
        }
    }
}
