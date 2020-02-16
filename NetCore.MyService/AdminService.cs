namespace NetCore.MyService
{
    public class AdminService : IAdminUserService
    {
        private ILogService logservice;
        public AdminService(ILogService logservice)
        {
            this.logservice = logservice;
        }
        public string GetPwd(string username)
        {
            logservice.AddLog("getPwd" + username);
            return "666";
        }
    }
}
