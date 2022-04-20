using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.IO;
using System.Threading;

namespace MyAgencyVault.BusinessLibrary.Masters
{
    [DataContract]
    public class ComDeptSupportData
    {
        [DataMember]
        public string FileName;
        [DataMember]
        public string FileType;
        [DataMember]
        public string FileDate;
        [DataMember]
        public string FilePath;
    }

    [DataContract]
    public class ComDeptSupport
    {
        private List<ComDeptSupportData> supportFileList;
        private AutoResetEvent autoResetEvent;

        public List<ComDeptSupportData> GetSupportFiles()
        {
            if (supportFileList == null)
                supportFileList = new List<ComDeptSupportData>();
            else
                supportFileList.Clear();
            
            string KeyValue = SystemConstant.GetKeyValue("ServerWebDevPath");
            WebDevPath ObjWebDevPath = WebDevPath.GetWebDevPath(KeyValue);
            FileUtility ObjDownload = FileUtility.CreateClient(ObjWebDevPath.URL, ObjWebDevPath.UserName, ObjWebDevPath.Password, ObjWebDevPath.DomainName);

            autoResetEvent = new AutoResetEvent(false);
            ObjDownload.ErrorOccured += new ErrorOccuredDel(ObjDownload_ErrorOccured);
            ObjDownload.ListComplete += new ListCompleteDel(ObjDownload_ListComplete);
            ObjDownload.List("Support");
            autoResetEvent.WaitOne();

            return supportFileList;
        }

        void ObjDownload_ErrorOccured(Exception error)
        {
            autoResetEvent.Set();
        }

        private void ObjDownload_ListComplete(List<FileData> files, int statusCode)
        {
            string status = statusCode.ToString();
            if (status.StartsWith("20"))
            {
                if (files != null && files.Count > 1)
                {
                    bool isFirst = true;
                    foreach (FileData file in files)
                    {
                        if (isFirst)
                        {
                            isFirst = false;
                            continue;
                        }
                        ComDeptSupportData supFile = new ComDeptSupportData { FileName = Path.GetFileNameWithoutExtension(file.FileName),FileType = Path.GetExtension(file.FileName), FilePath = file.FileName, FileDate = file.LastModifiedDate };
                        supportFileList.Add(supFile);
                    }
                }
            }
            autoResetEvent.Set();
        }
    }
}
