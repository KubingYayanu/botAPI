using System.Runtime.Serialization;

namespace botApi.Models
{
    public class intent
    {
        [DataMember(Name ="intent")]
        public string name;

        [DataMember(Name = "scroe")]
        public decimal score;
    }
}