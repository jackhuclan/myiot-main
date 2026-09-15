// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using FluentFTP;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VgAutoDrill.Fundation.Drill;

namespace VegaIot.External.Xianjin
{
    public class XianjinDrillFileLocator : IDrillFilePathLocator
    {
        private FtpDrillFilePathOptions _ftpDrillFilePathOptions;
        private readonly ILogger<XianjinDrillFileLocator> logger;

        public XianjinDrillFileLocator(IOptions<FtpDrillFilePathOptions> options, ILogger<XianjinDrillFileLocator> logger)
        {
            _ftpDrillFilePathOptions = options.Value;
            this.logger = logger;
        }

        /// <summary>
        /// ftp 服务器
        ///
        /// </summary>
        /// <param name="itemCode"></param>
        /// <returns></returns>
        public DrillInfo GetFilePath(string itemCode)
        {
            logger.LogInformation($"ftp  GetFilePath");
            try
            {
                using (FtpClient ftp = new FtpClient(_ftpDrillFilePathOptions.FtpHost, _ftpDrillFilePathOptions.FtpUsername, _ftpDrillFilePathOptions.FtpPassword, _ftpDrillFilePathOptions.FtpPort))
                {
                    ftp.Connect();
                    logger.LogInformation($"ftp 搜索的路径:{_ftpDrillFilePathOptions.FtpPath}");
                    var list = ftp.GetListing(_ftpDrillFilePathOptions.FtpPath).Where(s => s.FullName.ToLower().Contains(itemCode.ToLower()));
                    Console.WriteLine(list.Count());
                    if (list == null || list.Count() <= 0)
                    {
                        logger.LogError($"ftp 未找到文件");

                        throw new Exception("ftp 未找到文件");
                    }
                    foreach (var item in list)
                    {
                        logger.LogInformation($"ftp 搜索的路径文件有:{item.FullName}");
                    }
                    var remotePath = list.FirstOrDefault().FullName;
                    logger.LogInformation($"ftp 服务器地址{remotePath}");
                    var localFilePath = Path.Combine(_ftpDrillFilePathOptions.LocalDirectory, Path.GetFileName(remotePath));
                    logger.LogInformation($"ftp 本地地址{localFilePath}");
                    ftp.DownloadFile(localFilePath, remotePath, FtpLocalExists.Overwrite);
                    string diaPath = "";
                    string drlPth = localFilePath;
                    return new DrillInfo { DiaPath = diaPath, DrlPath = drlPth };
                }
            }
            catch (Exception ee)
            {
                logger.LogError($"和ftp服务器交互失败 {ee.Message}");
                throw new Exception($"和ftp服务器交互失败 {ee.Message}");
            }
        }
    }
}
