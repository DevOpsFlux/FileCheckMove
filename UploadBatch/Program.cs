/*-------------------------------------------------------
'	프로그램명	: UploadBatch.exe
'	작성자		: DevOpsFlux
'	작성일		: 2016-06-03
'	설명		: Local file -> Nas Move
' -------------------------------------------------------*/
using System;
using System.IO;
using System.Linq;
using System.Xml;

namespace UploadBatch
{
    class Program
    {        
        //public static IEnumerable<System.IO.FileInfo> list1;
        //public static IEnumerable<System.IO.FileInfo> list2;
        //public static string filelist = string.Empty;

        static void Main(string[] args)
        {
            //UnitTest();
            /**/

            // 재 튜닝 예정 옵션 기능 추가. MaxLoop MaxTermTime Thread까지는 불필요 향후 업로드서버 구축 예정
            try
            {
                string mvfName = string.Empty;
                string drName = string.Empty;
                string dirName = string.Empty;
                string nasdirName = string.Empty;
                //string excludeExt = string.Empty;
                string strFileExt = string.Empty;
                DirectoryInfo RsDir;

                dirName = GetInfoStr("dir"); //원본
                nasdirName = GetInfoStr("nas_dir"); //NAS 
                //excludeExt = GetInfoStr("exclude"); //Extention
                                
                //strFileExt = Right(strTxt,4);

                Console.WriteLine("start : 원본-" + dirName + "/ NAS-" + nasdirName);
                    
                RsDir = new System.IO.DirectoryInfo(dirName);
                Console.WriteLine("start : 원본-" + dirName + "/ NAS-" + nasdirName);
                Log("start : " + dirName + "---------------------------------------------------") ;
                FileInfo[] files = RsDir.GetFiles("*.*", SearchOption.AllDirectories).Where(f => f.CreationTime.Date == DateTime.Today.Date || f.LastWriteTime.Date == DateTime.Today.Date ).ToArray();

                foreach (FileInfo file in files)
                {
                    mvfName = file.FullName.Replace(dirName, nasdirName);
                    drName = file.DirectoryName.Replace(dirName, nasdirName);
                    
                    Log("원본 : " + file.FullName + " / 이동 : " + mvfName);
                                                
                    if (!System.IO.Directory.Exists(drName))
                    {
                        System.IO.Directory.CreateDirectory(drName);
                    }
                                        
                    //strFileExt = Right(mvfName, 4);
                    //if (excludeExt.IndexOf(strFileExt) == -1)
                    if(CheckFile(mvfName))
                    {
                        file.CopyTo(mvfName, true);
                        file.Delete();
                    }
                }
                Log("End : " + dirName + "");
                Console.WriteLine("end");
            }
            catch (IOException copyError)
            {
                Log(copyError.Message);
            }
            catch (Exception ex)
            {
                Log(ex.Message);
            }
            /**/
        }

        #region # UnitTest    
        public static void UnitTest()
        {            
            Console.WriteLine("WhiteList : " + GetInfoStr("WhiteList").ToLower());
            Console.WriteLine("BlackList : " + GetInfoStr("BlackList").ToLower());
            Console.WriteLine("ExceptDir : " + GetInfoStr("ExceptDir").ToLower());
            Console.WriteLine(" ");

            string strTxt = @"D:\ImgUpload\UnitTest\testestt.jpg";
            strTxt = strTxt.ToLower();
            string strFileExt = Path.GetExtension(strTxt).Replace(".","");
            Console.WriteLine("strTxt : " + strTxt);
            Console.WriteLine("strFileExt : " + strFileExt);

            if (CheckFile(strTxt))
                {
                    Console.WriteLine("OK OK OK");
                }

            
            bool bWhiteListCheck = CheckInfo("EXT", GetInfoStr("WhiteList"), strFileExt);
            bool bBlackListCheck = CheckInfo("EXT", GetInfoStr("BlackList"), strFileExt);
            bool bExceptDirCheck = CheckInfo("DIR", GetInfoStr("ExceptDir"), strTxt);

            Console.WriteLine("bWhiteListCheck : " + bWhiteListCheck.ToString());
            Console.WriteLine("bBlackListCheck : " + bBlackListCheck.ToString());
            Console.WriteLine("bExceptDirCheck : " + bExceptDirCheck.ToString());

            Console.WriteLine(" ");
            Console.WriteLine("strTxt : " + strTxt);
            Console.WriteLine("ExceptDir : " + GetInfoStr("ExceptDir"));
            
            string strExceptDir = GetInfoStr("ExceptDir").ToLower().Replace(";","");
            Console.WriteLine(strTxt.IndexOf(strExceptDir.ToString()));


            /*
            string strTxt = "testestt.jpg";
            string strChkExt = "asp;vbs;php;jsp;";
            strChkExt = GetInfoStr("BlackList");
            string strFileExt = Right(strTxt,3);
            
            Console.WriteLine(strTxt);
            Console.WriteLine(strFileExt);
            if (strChkExt.IndexOf(strFileExt) == -1)
            {
                Console.WriteLine("True");
            }
            else
            {
                Console.WriteLine("False");
            }
            */
            /*
            string strTxt = "User_UploadFile.asp";
            Console.WriteLine(strTxt);
            Console.WriteLine(strTxt.IndexOf(".asp"));
            if (strTxt.IndexOf(".asp") == -1 && strTxt.IndexOf(".vbs") == -1 && strTxt.IndexOf(".php") == -1 && strTxt.IndexOf(".jsp") == -1)
            {
                Console.WriteLine(strTxt);
            }
            */
        }
        #endregion

        #region # Get Config
        public static string GetConfigFile()
        {
            string strPath = "upf.config";
            return strPath;
        }

        public static string GetInfoStr(string pKind)
        {
            string rtnVal = "";
            string LogPath = "/Config/Drive/" + pKind;

            XmlDocument xmlDom = new XmlDocument();
            xmlDom.Load(GetConfigFile());

            if (xmlDom.SelectSingleNode(LogPath) != null)
            {
                rtnVal = xmlDom.SelectSingleNode(LogPath).InnerText;
            }

            return rtnVal;
        }
        #endregion

        #region # Etc Util  
        public static string GetDateTime()
        {
            DateTime NowDate = DateTime.Now;
            return NowDate.ToString("yyyy-MM-dd HH:mm:ss") + ":" + NowDate.Millisecond.ToString("000");
        }

        private static string Right(string target, int length)
        {
            if (length <= target.Length)
            {
                return target.Substring(target.Length - length);
            }
            return target;
        }
        private static bool CheckInfo(string p, string p1, string p2)
        {
            bool bResult = false;
            if (p == "EXT")
            {
                if (p1.IndexOf(p2) >= 0)
                {
                    bResult = true;
                }
            }
            else if (p == "DIR")
            {
                p1 = p1.Replace(";","");
                if (p2.IndexOf(p1) >= 0)
                {
                    bResult = true;
                }
            }
            return bResult;
        }
        // p:파일확장자
        private static bool CheckFile(string p)
        {
            bool bResult = false;
            string strFileExt = Path.GetExtension(p).Replace(".", "");
            bool bWhiteListCheck = CheckInfo("EXT", GetInfoStr("WhiteList").ToLower(), strFileExt.ToLower());
            bool bBlackListCheck = CheckInfo("EXT", GetInfoStr("BlackList").ToLower(), strFileExt.ToLower());
            bool bExceptDirCheck = CheckInfo("DIR", GetInfoStr("ExceptDir").ToLower(), p.ToLower());

            if (GetInfoStr("WhiteList").ToString() == "Y")
            {
                Log("bWhiteListCheck : " + bWhiteListCheck.ToString() + " // " + "bBlackListCheck : " + bBlackListCheck.ToString() + " // " + "bExceptDirCheck : " + bExceptDirCheck.ToString() + "");
            }
            if (bWhiteListCheck) // 포함
            {
                if (!bBlackListCheck) // 제외
                {
                    if (!bExceptDirCheck) // 제외
                    {
                        bResult = true;
                    }
                }
            }
            return bResult;
        }


        private static void Log(string str)
        {

            string FilePath = GetInfoStr("log") +"UTF_" + DateTime.Today.ToString("yyyyMMdd") + ".log";
            string DirPath = GetInfoStr("log");
            string temp;

            DirectoryInfo di = new DirectoryInfo(DirPath);
            FileInfo fi = new FileInfo(FilePath);

            try
            {
                if (di.Exists != true) Directory.CreateDirectory(DirPath);

                if (fi.Exists != true)
                {
                    using (StreamWriter sw = new StreamWriter(FilePath))
                    {
                        temp = string.Format("[{0}] : {1}", GetDateTime(), str);
                        sw.WriteLine(temp);
                        sw.Close();
                    }
                }
                else
                {
                    using (StreamWriter sw = File.AppendText(FilePath))
                    {
                        temp = string.Format("[{0}] : {1}", GetDateTime(), str);
                        sw.WriteLine(temp);
                        sw.Close();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        #endregion

    }
}
