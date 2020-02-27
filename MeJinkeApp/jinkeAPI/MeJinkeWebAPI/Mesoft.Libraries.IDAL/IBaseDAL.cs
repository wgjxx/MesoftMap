using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mesoft.Libraries.IDAL
{
    public interface IBaseDAL
    {
        /// <summary>
        /// 返回插入后的ID值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        int Add<T>(T t) where T : BaseModel;
        T Find<T>(int id) where T : BaseModel;
        List<T> FindAll<T>() where T : BaseModel;

    }

    public abstract class BaseModel
    {
        public abstract string GetTableName();
        //public abstract string GetColumn();

        public int ID { get; set; }
    }
}
