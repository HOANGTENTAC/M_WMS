namespace M_WMS.Model
{
    public class WSAXX03Model
    {
        public Guid Id { get; set; }
        public string TableName { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public int RetryCount { get; set; }
        public string LastError { get; set; } = string.Empty;

        // Payload ánh xạ thành class mạnh kiểu
        public OrderPayload Payload { get; set; }
        public string KyotenCd { get; set; } = string.Empty;
        public string InoutNo { get; set; } = string.Empty;
        public int IndexNo { get; set; }
        public int HasInserted { get; set; }
        public int SequenceNo { get; set; }
        public Guid TransactionId { get; set; }
    }
    public class OutboxSync
    {
        public Guid Id { get; set; }
        public string TableName { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public string Payload { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public int RetryCount { get; set; }
        public string LastError { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string KyotenCd { get; set; } = string.Empty;
        public string InoutNo { get; set; } = string.Empty;
        public int IndexNo { get; set; }
        public int HasInserted { get; set; }
    }
    public class OrderPayload
    {
        public GoodsH Header { get; set; }
        public List<GoodsM> Details { get; set; }
    }

    public class GoodsH
    {
        public int InoutKu1 { get; set; }
        public int InoutKu2 { get; set; }
        public string CreatedAt { get; set; } = string.Empty;
        public string UpdatedAt { get; set; } = string.Empty;
        public string StockLocation { get; set; } = string.Empty;
        public string Instruction { get; set; } = string.Empty;
        public string Stock_InCharge { get; set; } = string.Empty;
        public string OrderNo { get; set; } = string.Empty;
        public string Memo { get; set; } = string.Empty;
        public string StockType { get; set; } = string.Empty;
        public string Process_InCharge { get; set; } = string.Empty;
        public string OptDivision { get; set; } = string.Empty;
        public string GoodsCd { get; set; } = string.Empty;
        public string GoodsName { get; set; } = string.Empty;
        public int Qty { get; set; }
    }

    public class GoodsM
    {
        public int LineNo { get; set; }
        public string GoodsCd { get; set; } = string.Empty;
        public string GoodsName { get; set; } = string.Empty;
        public string Unit { get; set; } = string.Empty;
        public int Qty { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalAmount { get; set; }
        public string StockType { get; set; } = string.Empty;
        public string Memo { get; set; } = string.Empty;
        public string InoutReason { get; set; } = string.Empty;
        public int _ARRIVAL_QTY { get; set; }
        public int _SHIPMENT_QTY { get; set; }
        public string _InCharge { get; set; } = string.Empty;
        public string StockBasho { get; set; } = string.Empty;
        public string ToDay { get; set; } = string.Empty;
        public int PlanQty { get; set; }
        public int QtyCalculation { get; set; }
        public int StockQty { get; set; }
    }
}
