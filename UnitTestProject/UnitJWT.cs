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
                //验证是否包含aud 并等于 roberAudience
                success = success && load["aud"]?.ToString() == "roberAudience";

                //验证age>20等
                int.TryParse(load["age"].ToString(), out int age);
                Assert.IsTrue(age > 30);
                //其他验证 jwt的标识 jti是否加入黑名单等

                return success;
            });
            Assert.IsTrue(result);
        }
    }
}
