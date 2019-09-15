using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weixin.Tool.Utility
{
    class FileUtility
    {
        /// <summary>
        /// 读取文件内容
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns>string</returns>
        public static string Read(string path)
        {
            string result = null;
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                StreamReader reader = new StreamReader(fs, Encoding.UTF8);
                result = reader.ReadToEnd();
            }
            return result;
        }
        //
        // 摘要:
        //     /// 根据完整文件路径获取FileStream ///
        //
        // 参数:
        //   fileName:
        public static FileStream GetFileStream(string fileName)
        {
            FileStream result = null;
            if (!string.IsNullOrEmpty(fileName) && File.Exists(fileName))
            {
                result = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            }
            return result;
        }

        //
        // 摘要:
        //     /// 从Url下载文件 ///
        //
        // 参数:
        //   url:
        //
        //   fullFilePathAndName:
        public static void DownLoadFileFromUrl(string url, string fullFilePathAndName)
        {
            using (FileStream fileStream = new FileStream(fullFilePathAndName, FileMode.OpenOrCreate))
            {
                ApiHandler.Download(url, fileStream);
                fileStream.Flush(flushToDisk: true);
            }
        }

        //
        // 摘要:
        //     /// 判断文件是否正在被使用 ///
        //
        // 参数:
        //   filePath:
        //     文件路径
        public static bool FileInUse(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    using (new FileStream(filePath, FileMode.Open))
                    {
                        return false;
                    }
                }
                return false;
            }
            catch
            {
                return true;
            }
        }
        /// <summary>
        /// 将文件流转换成base64
        /// </summary>
        /// <param name="fileStream"></param>
        /// <returns></returns>
        public static string GetFileBase64FromStream(Stream fileStream)
        {
            fileStream.Seek(0L, SeekOrigin.Begin);
            byte[] array = new byte[fileStream.Length];
            fileStream.Read(array, 0, array.Length);
            var fileBase64 = Convert.ToBase64String(array);
            return fileBase64;
        }
        /// <summary>
        /// 判断是否本地文件还是远程文件
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static bool IsLocalFile(string url)
        {
            Uri uri = new Uri(url);
            return string.IsNullOrEmpty(uri.Host);
        }
    }
}
