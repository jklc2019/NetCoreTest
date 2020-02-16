using System;

namespace NetCore.MyService
{
    public class LogService : ILogService
    {
        public void AddLog(string msg)
        {
            Console.WriteLine("日志:"+msg);
        }
    }
}
