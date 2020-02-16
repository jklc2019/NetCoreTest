namespace NetCore.MyService
{
    public interface ILogService: IServiceSupport
    {
        void AddLog(string msg);
        
    }
}
