using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using Weixin.Tool.Enums;

namespace Weixin.Tool.Utility
{
    /// <summary>
    /// 用于收到用户回复消息的解密以及开发者回复消息的加密
    /// </summary>
    public class MsgCryptUtility
    {
        private string m_sToken;
        private string m_sEncodingAESKey;
        private string m_sAppID;

        //构造函数
        // @param sToken: 公众平台上，开发者设置的Token
        // @param sEncodingAESKey: 公众平台上，开发者设置的EncodingAESKey
        // @param sAppID: 公众帐号的appid
        public MsgCryptUtility(string sToken, string sEncodingAESKey, string sAppID)
        {
            m_sToken = sToken;
            m_sAppID = sAppID;
            m_sEncodingAESKey = sEncodingAESKey;
        }



        // 检验消息的真实性，并且获取解密后的明文
        // @param sMsgSignature: 签名串，对应URL参数的msg_signature
        // @param sTimeStamp: 时间戳，对应URL参数的timestamp
        // @param sNonce: 随机串，对应URL参数的nonce
        // @param sPostData: 密文，对应POST请求的数据
        // @param sMsg: 解密后的原文，当return返回0时有效
        // @return: 成功0，失败返回对应的错误码
        public int DecryptMsg(string sMsgSignature, string sTimeStamp, string sNonce, string sPostData, ref string sMsg)
        {
            if (m_sEncodingAESKey.Length != 43)
            {
                return (int)ReturnCode.不合法的EncodingAESKey;
            }
            XmlDocument doc = new XmlDocument();
            XmlNode root;
            string sEncryptMsg;
            try
            {
                doc.LoadXml(sPostData);
                root = doc.FirstChild;
                sEncryptMsg = root["Encrypt"].InnerText;
            }
            catch (Exception)
            {
                return (int)ReturnCode.xml解析失败;
            }
            //verify signature
            int ret = 0;
            ret = VerifySignature(m_sToken, sTimeStamp, sNonce, sEncryptMsg, sMsgSignature);
            if (ret != 0)
                return ret;
            //decrypt
            string cpid = "";
            try
            {
                sMsg = Cryptography.AES_decrypt(sEncryptMsg, m_sEncodingAESKey, ref cpid);
            }
            catch (Exception)
            {
                return (int)ReturnCode.Encrypt解密失败;
            }
            if (cpid != m_sAppID)
                return (int)ReturnCode.不合法的APPID;
            return 0;
        }

        //将企业号回复用户的消息加密打包
        // @param sReplyMsg: 企业号待回复用户的消息，xml格式的字符串
        // @param sTimeStamp: 时间戳，可以自己生成，也可以用URL参数的timestamp
        // @param sNonce: 随机串，可以自己生成，也可以用URL参数的nonce
        // @param sEncryptMsg: 加密后的可以直接回复用户的密文，包括msg_signature, timestamp, nonce, encrypt的xml格式的字符串,
        //						当return返回0时有效
        // return：成功0，失败返回对应的错误码
        public int EncryptMsg(string sReplyMsg, string sTimeStamp, string sNonce, ref string sEncryptMsg)
        {
            if (m_sEncodingAESKey.Length != 43)
            {
                return (int)ReturnCode.不合法的EncodingAESKey;
            }
            string raw = "";
            try
            {
                raw = Cryptography.AES_encrypt(sReplyMsg, m_sEncodingAESKey, m_sAppID);
            }
            catch (Exception)
            {
                return (int)ReturnCode.Encrypt加密失败;
            }
            string MsgSigature = "";
            int ret = 0;
            ret = GenarateSinature(m_sToken, sTimeStamp, sNonce, raw, ref MsgSigature);
            if (0 != ret)
                return ret;
            sEncryptMsg = "";

            string EncryptLabelHead = "<Encrypt><![CDATA[";
            string EncryptLabelTail = "]]></Encrypt>";
            string MsgSigLabelHead = "<MsgSignature><![CDATA[";
            string MsgSigLabelTail = "]]></MsgSignature>";
            string TimeStampLabelHead = "<TimeStamp><![CDATA[";
            string TimeStampLabelTail = "]]></TimeStamp>";
            string NonceLabelHead = "<Nonce><![CDATA[";
            string NonceLabelTail = "]]></Nonce>";
            sEncryptMsg = sEncryptMsg + "<xml>" + EncryptLabelHead + raw + EncryptLabelTail;
            sEncryptMsg = sEncryptMsg + MsgSigLabelHead + MsgSigature + MsgSigLabelTail;
            sEncryptMsg = sEncryptMsg + TimeStampLabelHead + sTimeStamp + TimeStampLabelTail;
            sEncryptMsg = sEncryptMsg + NonceLabelHead + sNonce + NonceLabelTail;
            sEncryptMsg += "</xml>";
            return 0;
        }

        public class DictionarySort : System.Collections.IComparer
        {
            public int Compare(object oLeft, object oRight)
            {
                string sLeft = oLeft as string;
                string sRight = oRight as string;
                int iLeftLength = sLeft.Length;
                int iRightLength = sRight.Length;
                int index = 0;
                while (index < iLeftLength && index < iRightLength)
                {
                    if (sLeft[index] < sRight[index])
                        return -1;
                    else if (sLeft[index] > sRight[index])
                        return 1;
                    else
                        index++;
                }
                return iLeftLength - iRightLength;

            }
        }
        //Verify Signature
        public static int VerifySignature(string sToken, string sTimeStamp, string sNonce, string sMsgEncrypt, string sSigture)
        {
            string hash = "";
            int ret = 0;
            ret = GenarateSinature(sToken, sTimeStamp, sNonce, sMsgEncrypt, ref hash);
            if (ret != 0)
                return ret;
            //System.Console.WriteLine(hash);
            if (hash == sSigture)
                return 0;
            else
            {
                return (int)ReturnCode.msg_signature验签失败;
            }
        }

        public static int GenarateSinature(string sToken, string sTimeStamp, string sNonce, string sMsgEncrypt, ref string sMsgSignature)
        {
            ArrayList AL = new ArrayList();
            AL.Add(sToken);
            AL.Add(sTimeStamp);
            AL.Add(sNonce);
            AL.Add(sMsgEncrypt);
            AL.Sort(new DictionarySort());
            string raw = "";
            for (int i = 0; i < AL.Count; ++i)
            {
                raw += AL[i];
            }

            SHA1 sha;
            ASCIIEncoding enc;
            string hash = "";
            try
            {
                sha = new SHA1CryptoServiceProvider();
                enc = new ASCIIEncoding();
                byte[] dataToHash = enc.GetBytes(raw);
                byte[] dataHashed = sha.ComputeHash(dataToHash);
                hash = BitConverter.ToString(dataHashed).Replace("-", "");
                hash = hash.ToLower();
            }
            catch (Exception)
            {
                return (int)ReturnCode.msg_signature生成失败;
            }
            sMsgSignature = hash;
            return 0;
        }
    }

    internal class Cryptography
    {
        public static UInt32 HostToNetworkOrder(UInt32 inval)
        {
            UInt32 outval = 0;
            for (int i = 0; i < 4; i++)
                outval = (outval << 8) + ((inval >> (i * 8)) & 255);
            return outval;
        }

        public static Int32 HostToNetworkOrder(Int32 inval)
        {
            Int32 outval = 0;
            for (int i = 0; i < 4; i++)
                outval = (outval << 8) + ((inval >> (i * 8)) & 255);
            return outval;
        }
        /// <summary>
        /// 解密方法
        /// </summary>
        /// <param name="Input">密文</param>
        /// <param name="EncodingAESKey"></param>
        /// <returns></returns>
        /// 
        public static string AES_decrypt(String Input, string EncodingAESKey, ref string appid)
        {
            byte[] Key;
            Key = Convert.FromBase64String(EncodingAESKey + "=");
            byte[] Iv = new byte[16];
            Array.Copy(Key, Iv, 16);
            byte[] btmpMsg = AES_decrypt(Input, Iv, Key);

            int len = BitConverter.ToInt32(btmpMsg, 16);
            len = IPAddress.NetworkToHostOrder(len);


            byte[] bMsg = new byte[len];
            byte[] bAppid = new byte[btmpMsg.Length - 20 - len];
            Array.Copy(btmpMsg, 20, bMsg, 0, len);
            Array.Copy(btmpMsg, 20 + len, bAppid, 0, btmpMsg.Length - 20 - len);
            string oriMsg = Encoding.UTF8.GetString(bMsg);
            appid = Encoding.UTF8.GetString(bAppid);


            return oriMsg;
        }

        public static String AES_encrypt(String Input, string EncodingAESKey, string appid)
        {
            byte[] Key;
            Key = Convert.FromBase64String(EncodingAESKey + "=");
            byte[] Iv = new byte[16];
            Array.Copy(Key, Iv, 16);
            string Randcode = CreateRandCode(16);
            byte[] bRand = Encoding.UTF8.GetBytes(Randcode);
            byte[] bAppid = Encoding.UTF8.GetBytes(appid);
            byte[] btmpMsg = Encoding.UTF8.GetBytes(Input);
            byte[] bMsgLen = BitConverter.GetBytes(HostToNetworkOrder(btmpMsg.Length));
            byte[] bMsg = new byte[bRand.Length + bMsgLen.Length + bAppid.Length + btmpMsg.Length];

            Array.Copy(bRand, bMsg, bRand.Length);
            Array.Copy(bMsgLen, 0, bMsg, bRand.Length, bMsgLen.Length);
            Array.Copy(btmpMsg, 0, bMsg, bRand.Length + bMsgLen.Length, btmpMsg.Length);
            Array.Copy(bAppid, 0, bMsg, bRand.Length + bMsgLen.Length + btmpMsg.Length, bAppid.Length);

            return AES_encrypt(bMsg, Iv, Key);

        }
        private static string CreateRandCode(int codeLen)
        {
            string codeSerial = "2,3,4,5,6,7,a,c,d,e,f,h,i,j,k,m,n,p,r,s,t,A,C,D,E,F,G,H,J,K,M,N,P,Q,R,S,U,V,W,X,Y,Z";
            if (codeLen == 0)
            {
                codeLen = 16;
            }
            string[] arr = codeSerial.Split(',');
            string code = "";
            int randValue = -1;
            Random rand = new Random(unchecked((int)DateTime.Now.Ticks));
            for (int i = 0; i < codeLen; i++)
            {
                randValue = rand.Next(0, arr.Length - 1);
                code += arr[randValue];
            }
            return code;
        }

        private static String AES_encrypt(String Input, byte[] Iv, byte[] Key)
        {
            var aes = new RijndaelManaged();
            //秘钥的大小，以位为单位
            aes.KeySize = 256;
            //支持的块大小
            aes.BlockSize = 128;
            //填充模式
            aes.Padding = PaddingMode.PKCS7;
            aes.Mode = CipherMode.CBC;
            aes.Key = Key;
            aes.IV = Iv;
            var encrypt = aes.CreateEncryptor(aes.Key, aes.IV);
            byte[] xBuff = null;

            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, encrypt, CryptoStreamMode.Write))
                {
                    byte[] xXml = Encoding.UTF8.GetBytes(Input);
                    cs.Write(xXml, 0, xXml.Length);
                }
                xBuff = ms.ToArray();
            }
            String Output = Convert.ToBase64String(xBuff);
            return Output;
        }

        private static String AES_encrypt(byte[] Input, byte[] Iv, byte[] Key)
        {
            var aes = new RijndaelManaged();
            //秘钥的大小，以位为单位
            aes.KeySize = 256;
            //支持的块大小
            aes.BlockSize = 128;
            //填充模式
            //aes.Padding = PaddingMode.PKCS7;
            aes.Padding = PaddingMode.None;
            aes.Mode = CipherMode.CBC;
            aes.Key = Key;
            aes.IV = Iv;
            var encrypt = aes.CreateEncryptor(aes.Key, aes.IV);
            byte[] xBuff = null;

            #region 自己进行PKCS7补位，用系统自己带的不行
            byte[] msg = new byte[Input.Length + 32 - Input.Length % 32];
            Array.Copy(Input, msg, Input.Length);
            byte[] pad = KCS7Encoder(Input.Length);
            Array.Copy(pad, 0, msg, Input.Length, pad.Length);
            #endregion

            #region 注释的也是一种方法，效果一样
            //ICryptoTransform transform = aes.CreateEncryptor();
            //byte[] xBuff = transform.TransformFinalBlock(msg, 0, msg.Length);
            #endregion

            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, encrypt, CryptoStreamMode.Write))
                {
                    cs.Write(msg, 0, msg.Length);
                }
                xBuff = ms.ToArray();
            }

            String Output = Convert.ToBase64String(xBuff);
            return Output;
        }

        private static byte[] KCS7Encoder(int text_length)
        {
            int block_size = 32;
            // 计算需要填充的位数
            int amount_to_pad = block_size - (text_length % block_size);
            if (amount_to_pad == 0)
            {
                amount_to_pad = block_size;
            }
            // 获得补位所用的字符
            char pad_chr = chr(amount_to_pad);
            string tmp = "";
            for (int index = 0; index < amount_to_pad; index++)
            {
                tmp += pad_chr;
            }
            return Encoding.UTF8.GetBytes(tmp);
        }
        /**
         * 将数字转化成ASCII码对应的字符，用于对明文进行补码
         * 
         * @param a 需要转化的数字
         * @return 转化得到的字符
         */
        static char chr(int a)
        {

            byte target = (byte)(a & 0xFF);
            return (char)target;
        }
        private static byte[] AES_decrypt(String Input, byte[] Iv, byte[] Key)
        {
            RijndaelManaged aes = new RijndaelManaged();
            aes.KeySize = 256;
            aes.BlockSize = 128;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.None;
            aes.Key = Key;
            aes.IV = Iv;
            var decrypt = aes.CreateDecryptor(aes.Key, aes.IV);
            byte[] xBuff = null;
            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, decrypt, CryptoStreamMode.Write))
                {
                    byte[] xXml = Convert.FromBase64String(Input);
                    byte[] msg = new byte[xXml.Length + 32 - xXml.Length % 32];
                    Array.Copy(xXml, msg, xXml.Length);
                    cs.Write(xXml, 0, xXml.Length);
                }
                xBuff = decode2(ms.ToArray());
            }
            return xBuff;
        }
        private static byte[] decode2(byte[] decrypted)
        {
            int pad = (int)decrypted[decrypted.Length - 1];
            if (pad < 1 || pad > 32)
            {
                pad = 0;
            }
            byte[] res = new byte[decrypted.Length - pad];
            Array.Copy(decrypted, 0, res, 0, decrypted.Length - pad);
            return res;
        }
        public static string MD5(string s, string input_charset = "UTF-8")
        {
            char[] hexDigits = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
                    'A', 'B', 'C', 'D', 'E', 'F' };
            try
            {

                byte[] btInput = Encoding.GetEncoding(input_charset).GetBytes(s);
                // 获得MD5摘要算法的 MessageDigest 对象
                MD5 mdInst = System.Security.Cryptography.MD5.Create();
                // 使用指定的字节更新摘要
                mdInst.ComputeHash(btInput);
                // 获得密文
                byte[] md = mdInst.Hash;
                // 把密文转换成十六进制的字符串形式
                int j = md.Length;
                char[] str = new char[j * 2];
                int k = 0;
                for (int i = 0; i < j; i++)
                {
                    byte byte0 = md[i];
                    str[k++] = hexDigits[(int)(((byte)byte0) >> 4) & 0xf];
                    str[k++] = hexDigits[byte0 & 0xf];
                }
                return new string(str);
            }
            catch
            {
                return null;
            }
        }
        public static string CalcHMACSHA256Hash(string content, string salt)
        {
            string result = "";
            var enc = Encoding.Default;
            byte[]
            baText2BeHashed = enc.GetBytes(content),
            baSalt = enc.GetBytes(salt);
            System.Security.Cryptography.HMACSHA256 hasher = new HMACSHA256(baSalt);
            byte[] baHashedText = hasher.ComputeHash(baText2BeHashed);
            result = string.Join("", baHashedText.ToList().Select(b => b.ToString("x2")).ToArray());
            return result;
        }
    }
}
