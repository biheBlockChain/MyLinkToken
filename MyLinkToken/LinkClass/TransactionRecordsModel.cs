using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLinkToken.LinkClass
{
    class TransactionRecordsModel
    {
        public string timestamp
        {
            set;
            get;
        }
        public int type { set; get; }
        public string tradeAccount { set; get; }
        public string amount { set; get; }
        public string cost { set; get; }
        public string hash { set; get; }
        public string title { set; get; }
        public string extra { set; get; }
        public string order_id { set; get; }
    }
}
