// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text.Json;
using FluentFTP;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VgAutoDrill.Fundation.Drill;
using VgAutoDrill.OpenAPI;

namespace VegaIot.External.Bomin
{
    internal class BominDrillFileLocator : IDrillFilePathLocator
    {
        private FtpDrillFilePathOptions _ftpDrillFilePathOptions;
        private readonly ILogger<BominDrillFileLocator> logger;
        private readonly IHttpRequestInvoker _httpRequestInvoker;

        public BominDrillFileLocator(IOptions<FtpDrillFilePathOptions> options, ILogger<BominDrillFileLocator> logger, IHttpRequestInvoker httpRequestInvoker)
        {
            _ftpDrillFilePathOptions = options.Value;
            this.logger = logger;
            _httpRequestInvoker = httpRequestInvoker;
        }

        /// <summary>
        /// ftp 服务器
        ///
        /// </summary>
        /// <param name="itemCode"></param>
        /// <returns></returns>
        public DrillInfo GetFilePath(string inf)
        {
            logger.LogError($"传入的插件内容是 {inf} ");
            //和中控交互获取数据
            var content = inf.Split(";");
            if (content.Count() != 2)
            {
                logger.LogError($"传入插件的内容格式不对");
                throw new Exception($"DSP,传入插件的内容格式不对");
            }

            var itemCode = content[0];
            var url = string.Format(_ftpDrillFilePathOptions.GetDrillRecipes, content[1], itemCode);
            logger.LogInformation($"向中控请求获取配方 {url}");
            var recipes = _httpRequestInvoker.GetFromJsonAsync<Dictionary<string, object?>>(url).GetAwaiter().GetResult();
            if (recipes == null || recipes.Count == 0)
            {
                logger.LogError($"向中控请求获取配方 {url} 异常");
                throw new Exception($"DSP,向中控请求获取配方异常，请手动加载钻带");
            }
            logger.LogDebug($"向中控请求配方{url}的返回值为{JsonSerializer.Serialize(recipes)}");

            if (!recipes.ContainsKey("ProgramFilePath") || string.IsNullOrEmpty(recipes["ProgramFilePath"]!.ToString()))
            {
                logger.LogError($"向中控请求{url} 返回值中 不包括 programFilePath ");
                throw new Exception($"DSP,中控返回没有programFilePath数据");
            }
            bool loadDia = recipes.ContainsKey("DiaFilePath") && !string.IsNullOrEmpty(recipes["DiaFilePath"]!.ToString());
            string diaFilePath = loadDia ? recipes["DiaFilePath"]!.ToString()! : string.Empty;
            string drlFilePath = recipes["ProgramFilePath"]!.ToString()!;
            logger.LogDebug($"向中控请求{url}  是否加载dia文件 {loadDia} ");

            //验证ftp服务相关的信息
            if (!recipes.ContainsKey("FtpHost") || string.IsNullOrEmpty(recipes["FtpHost"]!.ToString()))
            {
                logger.LogError($"向中控请求{url} 返回值中 不包括 FtpHost ");
                throw new Exception($"DSP,FtpHost 为空");
            }
            var ftpHost = recipes["FtpHost"]!.ToString()!;
            if (!recipes.ContainsKey("FtpUsername") || string.IsNullOrEmpty(recipes["FtpUsername"]!.ToString()))
            {
                logger.LogError($"向中控请求{url} 返回值中 不包括 FtpUsername ");
                throw new Exception($"DSP,FtpUsername 为空");
            }
            var ftpUsername = recipes["FtpUsername"]!.ToString()!;
            if (!recipes.ContainsKey("FtpPassword") || string.IsNullOrEmpty(recipes["FtpPassword"]!.ToString()))
            {
                logger.LogError($"向中控请求{url} 返回值中 不包括 FtpPassword ");
                throw new Exception($"DSP,FtpPassword 为空");
            }
            var ftpPassword = recipes["FtpPassword"]!.ToString()!;

            if (!recipes.ContainsKey("FtpPort") || !int.TryParse(recipes["FtpPort"]!.ToString(), out int ftpPort))
            {
                logger.LogError($"向中控请求{url} 返回值中 不包括 FtpPort ");
                throw new Exception($"DSP,FtpPort 为空");
            }

            //ftp 下载dia 和drl文件
            logger.LogDebug($"开始ftp 服务器交互");
            try
            {
                using (FtpClient ftp = new FtpClient(ftpHost, ftpUsername, ftpPassword, ftpPort))
                {
                    ftp.Connect();
                    string loadDiaFilePath = string.Empty;
                    string loadDrlFilePath = string.Empty;
                    //下载drl
                    LoadFileFromFtp(ftp, drlFilePath, out loadDrlFilePath);
                    logger.LogInformation($"ftp 下载 ProgramFilePath 成功");
                    //下载dia

                    if (loadDia)
                    {
                        LoadFileFromFtp(ftp, diaFilePath, out loadDiaFilePath);
                        logger.LogInformation($"ftp 下载 diamFilePath 成功");
                    }

                    return new DrillInfo { DiaPath = loadDiaFilePath, DrlPath = loadDrlFilePath };
                }
            }
            catch (Exception ee)
            {
                logger.LogError($"和ftp服务器交互失败 {ee.Message}");

                throw new Exception($"和ftp服务器交互失败 {ee.Message}");
            }
        }

        public void LoadFileFromFtp(FtpClient ftp, string filePath, out string localPath)
        {
            localPath = string.Empty;
            logger.LogDebug($"ftp 要搜索的文件 {filePath}");
            filePath = NormalizePath(filePath);
            if (!ftp.FileExists(filePath))
            {
                logger.LogError($"ftp   未找到文件 {filePath}");
                throw new Exception($"ftp   未找到文件 {filePath}");
            }
            if (filePath.StartsWith("\\"))
            {
                filePath = filePath.Substring(1);
            }
            var localFilePath = Path.Combine(_ftpDrillFilePathOptions.LocalDirectory, filePath);
            string directoryPath = Path.GetDirectoryName(localFilePath)!;
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            logger.LogInformation($"ftp 本地 到本地 地址 {localFilePath}");
            ftp.DownloadFile(localFilePath, filePath, FtpLocalExists.Overwrite);
            logger.LogInformation($"ftp 下载 文件 成功");
            localPath = NormalizePath(localFilePath);
        }

        private static string NormalizePath(string path)
        {
            // 替换所有非标准分隔符为当前系统的默认分隔符
            string normalizedPath = path.Replace('/', Path.DirectorySeparatorChar);
            return normalizedPath;
        }
    }
}
