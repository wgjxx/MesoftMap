using System.ServiceModel;

namespace WcfService1
{
    /// <summary>
    /// 不需要实现，由客户端实现传过来.用于WCF的双工通信模式
    /// </summary>
    public interface ICallBack
    {
        [OperationContract(IsOneWay=true)]   //必须指定IsOneWay=true，这样客户端才能实现并调用
        void GetUser(int id, string username);
    }
}