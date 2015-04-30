using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleDownload
{
    abstract class AbstractDownload
    {
        #region 屬性欄位
        protected string lasterror;
        public string ErrorMessage
        {
            get { return lasterror; }
        }
        #endregion
        protected string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase  ;
        //protected string path = 
        #region abstract 欄位
        public abstract string URL { get;}
        public abstract string Params { get;}
        public abstract string Content { get; set;}
        public abstract string FileName { get;}
        public abstract string CharSet { get;}
        #endregion
        /// <summary>
        /// 下載網頁
        /// </summary>
        public abstract void  Download ();
        /// <summary>
        /// 解析網頁
        /// </summary>
        public abstract void  Parse();
        /// <summary>
        /// 儲存
        /// </summary>
        public abstract void  SaveFile();
        public bool Excute()
        {
            try
            {
                Download();
                Parse();
                SaveFile();
            }
            catch (Exception ex)
            {
                lasterror = ex.Message;
                return false;
            }
            return true;
          
        }
    }
}
