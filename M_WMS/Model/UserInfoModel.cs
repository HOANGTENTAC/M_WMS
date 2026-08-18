namespace M_WMS.Model
{
    public class UserInfoModel
    {
        public class UserInfoResponse
        {
            public List<MstUser> MstUserList { get; set; }
            public List<object> GrpCdList { get; set; }
            public List<int> PermissionIdList { get; set; }
            public int Result { get; set; }
            public string Mess { get; set; }
        }

        public class MstUser
        {
            public int Id { get; set; }
            public string User_Cd { get; set; }
            public string Kyoten_Cd { get; set; }
            public string User_Name { get; set; }
            public string Culture { get; set; }
            public int Auth_Flg { get; set; }
            public string User_Name_Alias { get; set; }
            public string Sales_Tan_Cd { get; set; }
            public string EMail { get; set; }
            public int Void_Flg { get; set; }
            public int Record_Version { get; set; }
            public DateTime Updated_At { get; set; }
            public string Updated_User_Cd { get; set; }
        }
    }
}
