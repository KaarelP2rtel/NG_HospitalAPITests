using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace HospitalApiTests
{
    public class DiseaseDTO
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? Id { get; set; }
        public String Name { get; set; }
        public List<SymptomDTO> Symptoms { get; set; }

        public override string ToString()
        {
            return $"Id:{Id} Name:{Name} Symptoms:{Symptoms}";
        }
    }
}