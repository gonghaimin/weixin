using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace GoogleAuthenticator
{
    /// <summary>
    /// modified from
    /// http://brandonpotter.com/2014/09/07/implementing-free-two-factor-authentication-in-net-using-google-authenticator/
    /// https://github.com/brandonpotter/GoogleAuthenticator
    /// With elements borrowed from https://github.com/stephenlawuk/GoogleAuthenticator
    /// </summary>
    public class TwoFactorAuthenticator
    {
        private readonly static DateTime _epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        private TimeSpan DefaultClockDriftTolerance { get; set; }
        public bool UseManagedSha1Algorithm { get; set; }
        public bool TryUnmanagedAlgorithmOnFailure { get; set; }

        public TwoFactorAuthenticator() : this(true, true) { }

        public TwoFactorAuthenticator(bool useManagedSha1, bool useUnmanagedOnFail)
        {
            DefaultClockDriftTolerance = TimeSpan.FromMilliseconds(30);
            UseManagedSha1Algorithm = useManagedSha1;
            TryUnmanagedAlgorithmOnFailure = useUnmanagedOnFail;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="account">Account</param>
        /// <param name="qRPixelsPerModule">Number of pixels per QR Module (2 = ~120x120px QRCode)</param>
        /// <param name="issuer">Issuer ID (the name of the system, i.e. 'MyApp'), can be omitted but not recommended https://github.com/google/google-authenticator/wiki/Key-Uri-Format </param>
        /// <returns></returns>
        public SetupCode GenerateSetupCode(string account, int qRPixelsPerModule = 3, string issuer = null)
        {
            var secretKey = Guid.NewGuid().ToString().Replace("-", "");
            return GenerateSetupCode(account, secretKey, false, qRPixelsPerModule, issuer);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="account">Account</param>
        /// <param name="secretKey">Account Secret Key</param>
        /// <param name="secretIsBase32">Flag saying if accountSecretKey is in Base32 format or original secret</param>
        /// <param name="qRPixelsPerModule">Number of pixels per QR Module (2 pixels give ~ 100x100px QRCode)</param>
        /// <param name="issuer"></param>
        /// <returns></returns>
        public SetupCode GenerateSetupCode(string account, string secretKey, bool secretIsBase32, int qRPixelsPerModule = 0, string issuer = null)
        {
            secretKey = secretIsBase32 ? Base32String.Base32Decode(secretKey) : secretKey;

            return GenerateSetupCode(account, secretKey, qRPixelsPerModule, issuer);
        }

        private SetupCode GenerateSetupCode(string account, string secretKey, int qRPixelsPerModule = 0, string issuer = null)
        {
            if (account == null) { throw new NullReferenceException("Account is null"); }
            account = account.Replace(" ", "");
            string encodedSecretKey = Base32String.Base32Encode(secretKey);
            string provisionUrl = null;
            if (String.IsNullOrWhiteSpace(issuer))
            {
                provisionUrl = String.Format("otpauth://totp/{0}?secret={1}", account, encodedSecretKey);
            }
            else
            {
                //  https://github.com/google/google-authenticator/wiki/Conflicting-Accounts
                // Added additional prefix to account otpauth://totp/Company:joe_example@gmail.com for backwards compatibility
                provisionUrl = String.Format("otpauth://totp/{2}:{0}?secret={1}&issuer={2}", account, encodedSecretKey, UrlEncode(issuer));
            }
            var setupCode = new SetupCode();
            setupCode.Account = account;
            setupCode.QrCodeContent = provisionUrl;
            setupCode.Issuer = issuer;
            setupCode.SecretKey = secretKey;
            setupCode.EncodedSecretKey = encodedSecretKey;
            if (qRPixelsPerModule > 0)
            {
                using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
                using (QRCodeData qrCodeData = qrGenerator.CreateQrCode(provisionUrl, QRCodeGenerator.ECCLevel.Q))
                using (QRCode qrCode = new QRCode(qrCodeData))
                using (Bitmap qrCodeImage = qrCode.GetGraphic(qRPixelsPerModule))
                using (MemoryStream ms = new MemoryStream())
                {
                    qrCodeImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    setupCode.QrCodeImageBase64 = Convert.ToBase64String(ms.ToArray());
                    setupCode.QrCodeImageBase64Url = "data:image/png;base64," + setupCode.QrCodeImageBase64;
                }
            }
            return setupCode;
        }


        private string UrlEncode(string value)
        {
            StringBuilder result = new StringBuilder();
            string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~";

            foreach (char symbol in value)
            {
                if (validChars.IndexOf(symbol) != -1)
                {
                    result.Append(symbol);
                }
                else
                {
                    result.Append('%' + String.Format("{0:X2}", (int)symbol));
                }
            }

            return result.ToString().Replace(" ", "%20");
        }

        public string GeneratePINAtInterval(string secretKey, long counter, int digits = 6)
        {
            return GenerateHashedCode(secretKey, counter, digits);
        }

        internal string GenerateHashedCode(string secret, long iterationNumber, int digits = 6)
        {
            byte[] key = Encoding.UTF8.GetBytes(secret);
            return GenerateHashedCode(key, iterationNumber, digits);
        }
        /// <summary>
        /// Creates a HMACSHA1 algorithm to use to hash the counter bytes. By default, this will attempt to use
        /// the managed SHA1 class (SHA1Manager) and on exception (FIPS-compliant machine policy, etc) will attempt
        /// to use the unmanaged SHA1 class (SHA1CryptoServiceProvider).
        /// </summary>
        /// <param name="key">User's secret key, in bytes</param>
        /// <returns>HMACSHA1 cryptographic algorithm</returns>        
        internal HMACSHA1 getHMACSha1Algorithm(byte[] key)
        {
            HMACSHA1 hmac;

            try
            {
                hmac = new HMACSHA1(key, UseManagedSha1Algorithm);
            }
            catch (InvalidOperationException ioe)
            {
                if (UseManagedSha1Algorithm && TryUnmanagedAlgorithmOnFailure)
                {
                    try
                    {
                        hmac = new HMACSHA1(key, false);
                    }
                    catch (InvalidOperationException ioe2)
                    {
                        throw ioe2;
                    }
                }
                else
                {
                    throw ioe;
                }
            }

            return hmac;
        }

        internal string GenerateHashedCode(byte[] key, long iterationNumber, int digits = 6)
        {
            byte[] counter = BitConverter.GetBytes(iterationNumber);

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(counter);
            }

            HMACSHA1 hmac = getHMACSha1Algorithm(key);
            byte[] hash = hmac.ComputeHash(counter);

            int offset = hash[hash.Length - 1] & 0xf;

            // Convert the 4 bytes into an integer, ignoring the sign.
            int binary =
                ((hash[offset] & 0x7f) << 24)
                | (hash[offset + 1] << 16)
                | (hash[offset + 2] << 8)
                | (hash[offset + 3]);

            int password = binary % (int)Math.Pow(10, digits);
            return password.ToString(new string('0', digits));
        }

        private long GetCurrentCounter()
        {
            return GetCurrentCounter(DateTime.UtcNow, _epoch, 30);
        }

        private long GetCurrentCounter(DateTime now, DateTime epoch, int timeStep)
        {
            return (long)(now - epoch).TotalSeconds / timeStep;
        }

        public bool ValidateTwoFactorPIN(string secretKey, string twoFactorCodeFromClient)
        {
            return ValidateTwoFactorPIN(secretKey, twoFactorCodeFromClient, DefaultClockDriftTolerance);
        }

        public bool ValidateTwoFactorPIN(string secretKey, string twoFactorCodeFromClient, TimeSpan timeTolerance)
        {
            var codes = GetCurrentPINs(secretKey, timeTolerance);
            return codes.Any(c => c == twoFactorCodeFromClient);
        }

        public string GetCurrentPIN(string secretKey)
        {
            return GeneratePINAtInterval(secretKey, GetCurrentCounter());
        }

        public string GetCurrentPIN(string secretKey, DateTime now)
        {
            return GeneratePINAtInterval(secretKey, GetCurrentCounter(now, _epoch, 30));
        }

        public string[] GetCurrentPINs(string secretKey)
        {
            return GetCurrentPINs(secretKey, DefaultClockDriftTolerance);
        }

        public string[] GetCurrentPINs(string secretKey, TimeSpan timeTolerance)
        {
            List<string> codes = new List<string>();
            long iterationCounter = GetCurrentCounter();
            int iterationOffset = 0;

            if (timeTolerance.TotalSeconds > 30)
            {
                iterationOffset = Convert.ToInt32(timeTolerance.TotalSeconds / 30.00);
            }

            long iterationStart = iterationCounter - iterationOffset;
            long iterationEnd = iterationCounter + iterationOffset;

            for (long counter = iterationStart; counter <= iterationEnd; counter++)
            {
                codes.Add(GeneratePINAtInterval(secretKey, counter));
            }

            return codes.ToArray();
        }
    }

    internal class Base32String
    {
        private static String base32Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";
        private static int[] base32Lookup = {
         0xFF, 0xFF, 0x1A, 0x1B, 0x1C, 0x1D, 0x1E, 0x1F, // '0', '1', '2', '3', '4', '5', '6', '7' 
         0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, // '8', '9', ':', ';', '<', '=', '>', '?' 
         0xFF, 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, // '@', 'A', 'B', 'C', 'D', 'E', 'F', 'G' 
         0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, // 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O' 
         0x0F, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, // 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W' 
         0x17, 0x18, 0x19, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, // 'X', 'Y', 'Z', '[', '\', ']', '^', '_' 
         0xFF, 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, // '`', 'a', 'b', 'c', 'd', 'e', 'f', 'g' 
         0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, // 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o' 
         0x0F, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, // 'p', 'q', 'r', 's', 't', 'u', 'v', 'w' 
         0x17, 0x18, 0x19, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF // 'x', 'y', 'z', '{', '|', '}', '~', 'DEL' 
         };
        /// <summary>
        /// 编码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static String Base32Encode(string input)
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes(input);
            int i = 0, index = 0, digit = 0;
            int currByte, nextByte;
            StringBuilder base32 = new StringBuilder((bytes.Length + 7) * 8 / 5);

            while (i < bytes.Length)
            {
                currByte = (bytes[i] >= 0) ? bytes[i] : (bytes[i] + 256); // unsign 

                /* Is the current digit going to span a byte boundary? */
                if (index > 3)
                {
                    if ((i + 1) < bytes.Length)
                    {
                        nextByte = (bytes[i + 1] >= 0) ? bytes[i + 1] : (bytes[i + 1] + 256);
                    }
                    else
                    {
                        nextByte = 0;
                    }

                    digit = currByte & (0xFF >> index);
                    index = (index + 5) % 8;
                    digit <<= index;
                    digit |= nextByte >> (8 - index);
                    i++;
                }
                else
                {
                    digit = (currByte >> (8 - (index + 5))) & 0x1F;
                    index = (index + 5) % 8;
                    if (index == 0)
                    {
                        i++;
                    }
                }
                base32.Append(base32Chars[digit]);
            }

            return base32.ToString();
        }
        /// <summary>
        /// 解码
        /// </summary>
        /// <param name="base32"></param>
        /// <returns></returns>
        public static String Base32Decode(String base32)
        {
            int i, index, lookup, offset, digit;
            byte[] bytes = new byte[base32.Length * 5 / 8];

            for (i = 0, index = 0, offset = 0; i < base32.Length; i++)
            {
                lookup = base32[i] - '0';

                /* Skip chars outside the lookup table */
                if (lookup < 0 || lookup >= base32Lookup.Length)
                {
                    continue;
                }

                digit = base32Lookup[lookup];

                /* If this digit is not in the table, ignore it */
                if (digit == 0xFF)
                {
                    continue;
                }

                if (index <= 3)
                {
                    index = (index + 5) % 8;
                    if (index == 0)
                    {
                        bytes[offset] = (byte)(bytes[offset] | digit);
                        offset++;
                        if (offset >= bytes.Length)
                        {
                            break;
                        }
                    }
                    else
                    {
                        bytes[offset] = (byte)(bytes[offset] | digit << (8 - index));
                    }
                }
                else
                {
                    index = (index + 5) % 8;
                    bytes[offset] = (byte)(bytes[offset] | (digit >> index));
                    offset++;

                    if (offset >= bytes.Length)
                    {
                        break;
                    }
                    bytes[offset] = (byte)(bytes[offset] | digit << (8 - index));
                }
            }
            return System.Text.Encoding.UTF8.GetString(bytes);
        }

    }
}