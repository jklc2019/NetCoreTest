namespace NetCore.Web
{
    public class TestService
    {
        private DataService dataservice;
        public TestService(DataService dataservice)
        {
            this.dataservice = dataservice;
        }
        public string TestHello()
        {
            return "Hello!"+this.dataservice.DataHello();
        }
    }
}
