using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M_WMS.Model
{
    public class StockInoutTenplate
    {
        public string PoNo { get; set; }
        public int? LineNo { get; set; }
        public string StockType { get; set; }
        public string GoosdCd { get; set; }
        public string GoodsName { get; set; }
        public int Qty { get; set; }
        public string Unit { get; set; }
        public string Note { get; set; }
        public string InstrNo { get; set; }
        public string Dept { get; set; }
        public string Reason { get; set; }
    }
}
