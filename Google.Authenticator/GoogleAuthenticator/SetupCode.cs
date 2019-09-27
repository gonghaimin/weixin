using System;
using System.Collections.Generic;
using System.Text;

namespace GoogleAuthenticator
{
    public class SetupCode
    {
        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; internal set; }
        /// <summary>
        /// 发行人
        /// </summary>
        public string Issuer { get; internal set; }
        /// <summary>
        /// 明文秘钥
        /// </summary>
        public string SecretKey { get; internal set; }
        /// <summary>
        /// 加密后的秘钥
        /// </summary>
        public string EncodedSecretKey { get; internal set; }
        /// <summary>
        /// base64 image string
        /// </summary>
        public string QrCodeImageBase64 { get; internal set; }
        /// <summary>
        /// base64 image url
        /// </summary>
        public string QrCodeImageBase64Url { get; internal set; }
        /// <summary>
        /// 二维码中的内容
        /// </summary>
        public string QrCodeContent { get; internal set; }
    }
}
