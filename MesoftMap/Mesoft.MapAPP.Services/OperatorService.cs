using Mesoft.Libraries.Shared;
using Mesoft.MapAPP.Interface;
using MeSoft.Encrypts;
using MesoftMap.DataContext;
using MesoftMap.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mesoft.MapAPP.Services
{
    public class OperatorService : IOperatorService
    {
        private MeEPADBContext _DBContext;
        private const string _PassSalt = "MEsoft0622";
        public OperatorService(MeEPADBContext context)
        {
            this._DBContext = context;
        }

        public Operator AddOperator(string id, string name, string password, int departmentID, int groupID, out string msg)
        {
            var oldOperator = _DBContext.Operators.Find(id);
            if (null != oldOperator)
            {
                msg = "用户名已存在，请重新输入一个新用户名！";
                return null;
            }
            password = string.IsNullOrWhiteSpace(password) ? _PassSalt : _PassSalt + password;
            var pass =  MD5Helper.MD5Encrypt( password);
            var item = new Operator
            {
                ID = id,
                Password = pass,
                DepartmentID = departmentID,
                Name = name,
                GroupID = groupID,
                RecTime = DateTime.Now
            };
            _DBContext.Operators.Add(item);
            msg = "ok";
             _DBContext.SaveChanges() ;
            return item;
        }

        public bool IsUserValid(string id, string passWord, out string msg)
        {
            var item =_DBContext.Operators.Find(id);
            if (item == null)
            {
                msg = "未找到用户,请检查用户名是否输入正确";
                return false;
            }
            int codeLength = 32;
            passWord = string.IsNullOrWhiteSpace(passWord) ? _PassSalt : _PassSalt + passWord;
            var pass = MD5Helper.MD5Encrypt(passWord, codeLength);
            if (!pass.Equals(item.Password))
            {
                msg = "密码不对，请重新输入";
                return false;
            }
            msg = "ok";
            return true;
        }

        public Operator GetUser(string id)
        {
            return this._DBContext.Operators.Find(id);
        }

        public List<OperatorRight> GetUserRights(Operator user)
        {
            if (null == user) throw new Exception("GetUserRights(user)user不能为空");

            var items =
                _DBContext.OperatorRights.Where(item => item.GroupID == user.GroupID).ToList();
            return items;
        }
    }
}
