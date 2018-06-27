using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace HospitalApiTests
{
    public class SymptomDTO
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? Id { get; set; }
        public String Name { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<DiseaseDTO> Diseases { get; set; }

        public override string ToString()
        {
            return $"Id:{Id} Name:{Name}";
        }


        //Does not cover Diseases List
        internal bool HasAllFields()
        {
            return Id != null && Name != null;
        }
    }
}