using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace botApi.Models
{
    public class LUISResult
    {
        [DataMember(Name= "query")]
        public string name;

        [DataMember(Name = "topScoringIntent")]
        public intent topScoringIntent;

        [DataMember(Name = "intents")]
        public List<intent> intents;

        [DataMember(Name = "entities")]
        public List<entity> entities;
    }
}