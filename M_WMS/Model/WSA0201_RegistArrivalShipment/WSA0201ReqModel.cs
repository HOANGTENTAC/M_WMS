using M_WMS.Services.Model;

namespace M_WMS.Model.WSA0201_RegistArrivalShipment
{
    public class WSA0201ReqModel : BaseModel
    {
        public int InoutKu1 { get; set; }
        public int InoutKu2 { get; set; }
        public string PoNo { get; set; }
        public string InstructionNo { get; set; }
        public string InoutNo { get; set; }
        public string OptDivision { get; set; }
        public string SequenceNo { get; set; }
        public string ScheduledShipDate { get; set; }
        public string StockLocation { get; set; }
        public List<TblInoutHistoryH> TblInoutHistoryHs { get; set; } = new List<TblInoutHistoryH>();

        /// <summary>
        /// 入出庫履歴明細
        /// </summary>
        public List<TblInoutHistoryD> TblInoutHistoryDs { get; set; } = new List<TblInoutHistoryD>();
        public class TblInoutHistoryH
        {
            /// <summary>
            /// ID
            /// </summary>
            public int Id { get; set; }

            /// <summary>
            /// inout_ku_1
            /// </summary>
            public int InoutKu1 { get; set; }

            /// <summary>
            /// inout_ku_2
            /// </summary>
            public int InoutKu2 { get; set; }

            /// <summary>
            /// inout_no
            /// </summary>
            public string InoutNo { get; set; }

            /// <summary>
            /// instruction_no
            /// </summary>
            public string InstructionNo { get; set; }

            /// <summary>
            /// po_no
            /// </summary>
            public string PoNo { get; set; }

            /// <summary>
            /// entry_date
            /// </summary>
            public string EntryDate { get; set; }

            /// <summary>
            /// entry_time
            /// </summary>
            public string EntryTime { get; set; }

            /// <summary>
            /// 無効フラグ
            /// </summary>
            public int VoidFlg { get; set; }

            /// <summary>
            /// レコードバージョン
            /// </summary>
            public int RecordVersion { get; set; }

            /// <summary>
            /// 更新日時
            /// </summary>
            public DateTimeOffset UpdatedAt { get; set; }

            /// <summary>
            /// 更新担当者CD
            /// </summary>
            public string UpdatedUserCd { get; set; }

            /// <summary>
            /// 更新担当者名
            /// </summary>
            public string UpdatedUserName { get; set; }

            /// <summary>
            /// 初回登録日
            /// </summary>
            public DateTimeOffset CreatedAt { get; set; }

            /// <summary>
            /// 初回登録担当者CD
            /// </summary>
            public string CreatedUserCd { get; set; }

            /// <summary>
            /// 初回登録担当者名
            /// </summary>
            public string CreatedUserName { get; set; }

            /// <summary>
            /// 生産備考
            /// </summary>
            public string Bikou { get; set; }

            /// <summary>
            /// 拠点CD
            /// </summary>
            public string KyotenCd { get; set; }

            /// <summary>
            /// 生産部門
            /// </summary>
            public string OptDivision { get; set; }

            /// <summary>
            /// zaiko_ku
            /// </summary>
            public string ZaikoKu { get; set; }
            public int IndexNo { get; set; }
            public string ScheduledShipDate { get; set; }
            public string Payloads { get; set; }
            public Guid IdOutbox { get; set; }
            public string TableName { get; set; }
            public string Action { get; set; }
            public string Status { get; set; }
            public int SequenceNo { get; set; }
            public Guid TransactionId { get; set; }
        }

        /// <summary>
        /// 入出庫履歴明細
        /// </summary>
        public class TblInoutHistoryD
        {
            /// <summary>
            /// ID
            /// </summary>
            public int Id { get; set; }

            /// <summary>
            /// tbl_inout_history_h_id
            /// </summary>
            public int TblInoutHistoryHId { get; set; }
            /// <summary>
            /// inout_no
            /// </summary>
            public string InoutNo { get; set; }
            /// <summary>
            /// inout_ku_1
            /// </summary>
            public int InoutKu1 { get; set; }

            /// <summary>
            /// inout_ku_2
            /// </summary>
            public int InoutKu2 { get; set; }

            /// <summary>
            /// inout_gyou
            /// </summary>
            public int InoutGyou { get; set; }

            /// <summary>
            /// instruction_no
            /// </summary>
            public string InstructionNo { get; set; }

            /// <summary>
            /// instruction_gyou
            /// </summary>
            public int? InstructionGyou { get; set; }

            /// <summary>
            /// PONo
            /// </summary>
            public string PoNo { get; set; }
            /// <summary>
            /// PO行
            /// </summary>
            public int? PoGyou { get; set; }

            /// <summary>
            /// entry_date
            /// </summary>
            public string EntryDate { get; set; }

            /// <summary>
            /// entry_time
            /// </summary>
            public string EntryTime { get; set; }

            /// <summary>
            /// 商品CD
            /// </summary>
            public string ShoCd { get; set; }

            /// <summary>
            /// 品名
            /// </summary>
            public string Hinmei { get; set; }

            /// <summary>
            /// 単位
            /// </summary>
            public string Unit { get; set; }

            /// <summary>
            /// 数量
            /// </summary>
            public int? Suu { get; set; }

            /// <summary>
            /// グループCD
            /// </summary>
            public string GrpCd { get; set; }

            /// <summary>
            /// zaiko_ku
            /// </summary>
            public string ZaikoKu { get; set; }

            /// <summary>
            /// inout_reason
            /// </summary>
            public string InoutReason { get; set; }

            /// <summary>
            /// 無効フラグ
            /// </summary>
            public int VoidFlg { get; set; }

            /// <summary>
            /// レコードバージョン
            /// </summary>
            public int RecordVersion { get; set; }

            /// <summary>
            /// 更新日
            /// </summary>
            public DateTimeOffset UpdatedAt { get; set; }

            /// <summary>
            /// 更新担当者CD
            /// </summary>
            public string UpdatedUserCd { get; set; }

            /// <summary>
            /// 更新担当者名
            /// </summary>
            public string UpdatedUserName { get; set; }

            /// <summary>
            /// 初回登録日時
            /// </summary>
            public DateTimeOffset CreatedAt { get; set; }

            /// <summary>
            /// 初回登録担当者CD
            /// </summary>
            public string CreatedUserCd { get; set; }

            /// <summary>
            /// 初回登録担当者名
            /// </summary>
            public string CreatedUserName { get; set; }

            /// <summary>
            /// 有効期限
            /// </summary>
            public string ExpirationDate { get; set; }

            /// <summary>
            /// 予定数量
            /// </summary>
            public int PlanQty { get; set; }
            //public int PlanQtyIn { get; set; }
            public int MstCrShoKyotensId { get; set; }
            public string StockBasho { get; set; }
            public string FirstReceivedDate { get; set; }
            public string LastReceivedDate { get; set; }
            public string FirstShippedDate { get; set; }
            public string LastShippedDate { get; set; }
            public int NetInventoryQty { get; set; }
            //public int PreStockingQty { get; set; }
            //public int StockingQty { get; set; }
            public int ActualQty { get; set; }
            public int StockQty { get; set; }
        }
    }
}
