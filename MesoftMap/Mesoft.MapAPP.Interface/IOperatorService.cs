using MesoftMap.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mesoft.MapAPP.Interface
{
    public interface IOperatorService
    {
        bool IsUserValid(string id, string passWord, out string msg);

        Operator AddOperator(string id, string name, string password, int departmentID, int groupID, out string msg);
        Operator GetUser(string id);
        List<OperatorRight> GetUserRights(Operator currentUser);
    }
}
