using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace SimpleP2PExample
{
    [ServiceContract(CallbackContract = typeof(IPing))]
    public interface IPing
    {
        [OperationContract(IsOneWay = true)]
        void Ping(string sender, string message);
    }
}
