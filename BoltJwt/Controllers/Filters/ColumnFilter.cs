using System.Runtime.Serialization;

namespace BoltJwt.Controllers.Filters
{
    [DataContract]
    public class ColumnFilter
    {
        [DataMember(Name = "name")]
        public string Name { get; set;}
        [DataMember(Name = "operand")]
        public string Operand { get; set; }
        [DataMember(Name = "value")]
        public string Value { get; set; }
    }
}