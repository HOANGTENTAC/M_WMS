using M_WMS.Extensions;

namespace M_WMS.Services.Model
{
    public class BaseModel
    {
        public int Result { get; set; }

        public string Mess { get; set; }
        public override string ToString()
        {
            return this.ToStringObject();
        }
    }
}
