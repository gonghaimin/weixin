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
    public class GoogleAuthenticator
    {    /// <summary>
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
            private string EncodeAccountSecretKey(string accountSecretKey)
            {
                return Base32Encode(Encoding.UTF8.GetBytes(accountSecretKey));
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="account">Account Title</param>
            /// <param name="QRPixelsPerModule">Number of pixels per QR Module (2 = ~120x120px QRCode)</param>
            /// <param name="issuer">Issuer ID (the name of the system, i.e. 'MyApp'), can be omitted but not recommended https://github.com/google/google-authenticator/wiki/Key-Uri-Format </param>
            /// <returns></returns>
            public SetupCode GenerateSetupCode(string account, int QRPixelsPerModule = 4, string issuer = null)
            {
                var accountSecretKey = Guid.NewGuid().ToString().Replace("-","");
                return GenerateSetupCode(account, accountSecretKey, QRPixelsPerModule, issuer);
            }
            /// <summary>
            /// Generate a setup code for a Google Authenticator user to scan
            /// </summary>
            /// <param name="accountTitleNoSpaces">Account Title (no spaces)</param>
            /// <param name="accountSecretKey">Account Secret Key as byte[]</param>
            /// <param name="QRPixelsPerModule">Number of pixels per QR Module (2 = ~120x120px QRCode)</param>
            /// <param name="issuer">Issuer ID (the name of the system, i.e. 'MyApp'), can be omitted but not recommended https://github.com/google/google-authenticator/wiki/Key-Uri-Format </param>
            /// <returns>SetupCode object</returns>
            public SetupCode GenerateSetupCode(string accountTitleNoSpaces, string accountSecretKey, int QRPixelsPerModule = 0, string issuer = null)
            {
                if (accountTitleNoSpaces == null) { throw new NullReferenceException("Account is null"); }
                accountTitleNoSpaces = accountTitleNoSpaces.Replace(" ", "");
                string encodedSecretKey = EncodeAccountSecretKey(accountSecretKey);
                string provisionUrl = null;
                if (String.IsNullOrWhiteSpace(issuer))
                {
                    provisionUrl = String.Format("otpauth://totp/{0}?secret={1}", accountTitleNoSpaces, encodedSecretKey);
                }
                else
                {
                    //  https://github.com/google/google-authenticator/wiki/Conflicting-Accounts
                    // Added additional prefix to account otpauth://totp/Company:joe_example@gmail.com for backwards compatibility
                    provisionUrl = String.Format("otpauth://totp/{2}:{0}?secret={1}&issuer={2}", accountTitleNoSpaces, encodedSecretKey, UrlEncode(issuer));
                }
                var setupCode = new SetupCode();
                setupCode.Account = accountTitleNoSpaces;
                setupCode.Otpauth = provisionUrl;
                setupCode.Issuer = issuer;
                setupCode.SecretKey = accountSecretKey;
                if (QRPixelsPerModule > 0)
                {
                    using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
                    using (QRCodeData qrCodeData = qrGenerator.CreateQrCode(provisionUrl, QRCodeGenerator.ECCLevel.Q))
                    using (QRCode qrCode = new QRCode(qrCodeData))
                    using (Bitmap qrCodeImage = qrCode.GetGraphic(QRPixelsPerModule))
                    using (MemoryStream ms = new MemoryStream())
                    {
                        qrCodeImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        setupCode.QrCodeImageBase64 = Convert.ToBase64String(ms.ToArray());
                        setupCode.QrCodeImageUrl = "data:image/png;base64," + setupCode.QrCodeImageBase64;
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

            public string GeneratePINAtInterval(string accountSecretKey, long counter, int digits = 6)
            {
                return GenerateHashedCode(accountSecretKey, counter, digits);
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

            public bool ValidateTwoFactorPIN(string accountSecretKey, string twoFactorCodeFromClient)
            {
                return ValidateTwoFactorPIN(accountSecretKey, twoFactorCodeFromClient, DefaultClockDriftTolerance);
            }

            public bool ValidateTwoFactorPIN(string accountSecretKey, string twoFactorCodeFromClient, TimeSpan timeTolerance)
            {
                var codes = GetCurrentPINs(accountSecretKey, timeTolerance);
                return codes.Any(c => c == twoFactorCodeFromClient);
            }

            public string GetCurrentPIN(string accountSecretKey)
            {
                return GeneratePINAtInterval(accountSecretKey, GetCurrentCounter());
            }

            public string GetCurrentPIN(string accountSecretKey, DateTime now)
            {
                return GeneratePINAtInterval(accountSecretKey, GetCurrentCounter(now, _epoch, 30));
            }

            public string[] GetCurrentPINs(string accountSecretKey)
            {
                return GetCurrentPINs(accountSecretKey, DefaultClockDriftTolerance);
            }
            private static string Base32Encode(byte[] data)
            {
                int inByteSize = 8;
                int outByteSize = 5;
                char[] alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567".ToCharArray();

                int i = 0, index = 0, digit = 0;
                int current_byte, next_byte;
                StringBuilder result = new StringBuilder((data.Length + 7) * inByteSize / outByteSize);

                while (i < data.Length)
                {
                    current_byte = (data[i] >= 0) ? data[i] : (data[i] + 256); // Unsign

                    /* Is the current digit going to span a byte boundary? */
                    if (index > (inByteSize - outByteSize))
                    {
                        if ((i + 1) < data.Length)
                            next_byte = (data[i + 1] >= 0) ? data[i + 1] : (data[i + 1] + 256);
                        else
                            next_byte = 0;

                        digit = current_byte & (0xFF >> index);
                        index = (index + outByteSize) % inByteSize;
                        digit <<= index;
                        digit |= next_byte >> (inByteSize - index);
                        i++;
                    }
                    else
                    {
                        digit = (current_byte >> (inByteSize - (index + outByteSize))) & 0x1F;
                        index = (index + outByteSize) % inByteSize;
                        if (index == 0)
                            i++;
                    }
                    result.Append(alphabet[digit]);
                }

                return result.ToString();
            }

            public string[] GetCurrentPINs(string accountSecretKey, TimeSpan timeTolerance)
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
                    codes.Add(GeneratePINAtInterval(accountSecretKey, counter));
                }

                return codes.ToArray();
            }
        }
    }
}