using Newtonsoft.Json;
using System;

namespace HospitalApiTests
{
    public class SymptomDTO
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? Id { get; set; }
        public String Name { get; set; }
        public override string ToString()
        {
            return $"Id:{Id} Name:{Name}";
        }
    }
}