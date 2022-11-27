using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace BankAppBusineesTier
{
    [ServiceContract]
    public interface BusinessServerInterface
    {
        [OperationContract]
        int GetNumEntries();
        [OperationContract]
        [FaultContract(typeof(ConsoleAppBank.IndexOutOfRangeFault))]
        void GetValuesForEntry(int index, out uint acctNo, out uint pin, out int bal, out
        string fName, out string lName, out Bitmap icon);

        [OperationContract]
        [FaultContract(typeof(SearchTextNotFound))]
        void GetValuesForSearch(string searchText, out uint acctNo, out uint pin, out int bal, out
        string fName, out string lName, out Bitmap icon);
    }
}
