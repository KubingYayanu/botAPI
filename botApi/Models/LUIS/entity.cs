using System.Runtime.Serialization;

namespace botApi.Models
{
    public class entity
    {
        [DataMember(Name="entity")]
        public string name;

        [DataMember(Name = "type")]
        public string type;

        [DataMember(Name = "startIndex")]
        public int startIndex;

        [DataMember(Name = "endIndex")]
        public int endIndex;

        [DataMember(Name = "scroe")]
        public decimal scroe;
    }
}