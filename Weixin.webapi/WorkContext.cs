using AuthService;


namespace Weixin.WebApi
{
    public class WorkContext: IAuthContext
    {
        

        public override bool IsAllowAnonymous(string path)
        {
            return true;
        }

        public override void TryInit(string json)
        {

        }

        public override bool Authorize(string path)
        {
            return true;
        }
    }
}
