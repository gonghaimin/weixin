using AuthService.JWT;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;


namespace UnitTestProject
{
    [TestClass]
    public class UnitJWT
    {

        [TestMethod]
        public void TokenValidateTest()
        {
            Dictionary<string, object> payLoad = new Dictionary<string, object>();
            payLoad.Add("sub", "rober");
            payLoad.Add("jti", Guid.NewGuid().ToString());
            payLoad.Add("nbf", null);
            payLoad.Add("exp", null);
            payLoad.Add("iss", "roberIssuer");
            payLoad.Add("aud", "roberAudience");
            payLoad.Add("age", 40);

            var encodeJwt = JwtHandler.CreateTokenByHandler(payLoad, 30);

            var result = JwtHandler.Validate(encodeJwt, (load) => {
                var success = true;
                //��֤�Ƿ����aud ������ roberAudience
                success = success && load["aud"]?.ToString() == "roberAudience";

                //��֤age>20��
                int.TryParse(load["age"].ToString(), out int age);
                Assert.IsTrue(age > 30);
                //������֤ jwt�ı�ʶ jti�Ƿ�����������

                return success;
            });
            Assert.IsTrue(result);
        }
    }
}
