using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BankAppBusineesTier
{
    [DataContract]
    public class SearchTextNotFound
    {
        [DataMember]
        public string TextNotFound { get; set; }
    }
}
