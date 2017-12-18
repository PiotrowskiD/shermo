using System;
using System.Collections.Generic;
using System.IO;
using ServerMonitor.Model;
using log4net;
using System.Reflection;

namespace ServerReporterService
{
    class Disk
    {
        private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public List<DiskInfo> CheckParameters()
        {
            DiskInfo diskInfo;
            List<DiskInfo> disksInfo = new List<DiskInfo>();
            var today = DateTime.Now;
            DriveInfo[] drives = DriveInfo.GetDrives();

            foreach (DriveInfo drive in drives)
            {
                if (drive.IsReady)
                {
                    diskInfo = new DiskInfo();
                    diskInfo.Name = drive.Name + " " + drive.VolumeLabel;
                    diskInfo.TotalSize = Math.Round((double)drive.TotalSize / 1024 / 1024, 2, MidpointRounding.AwayFromZero);
                    diskInfo.FreeSpace = Math.Round((double)drive.TotalFreeSpace / 1024 / 1024, 2, MidpointRounding.AwayFromZero);
                    diskInfo.PercentOfFreeSpace = Math.Round(diskInfo.FreeSpace / diskInfo.TotalSize * 100, 2, MidpointRounding.AwayFromZero);
                    diskInfo.Time = today.ToString("yyyy-MM-dd HH:mm:ss");
                    disksInfo.Add(diskInfo);
                }
            }
            logger.Info("Drive details checked succesfully");
            return disksInfo;
        }

    }

}