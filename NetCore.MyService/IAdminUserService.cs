using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCore.MyService
{
    public interface IAdminUserService: IServiceSupport
    {
        string GetPwd(string username);
    }
}
