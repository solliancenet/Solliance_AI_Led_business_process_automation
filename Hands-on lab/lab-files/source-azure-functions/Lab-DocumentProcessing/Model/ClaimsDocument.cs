using Newtonsoft.Json;
using System;

namespace DocumentProcessing.Model
{
    public class ClaimsDocument
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        public string PatientName { get; set; }
        public DateTime PatientBirthDate { get; set; }
        public decimal TotalCharges { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal AmountDue { get; set; }
        public string InsuredID { get; set; }
        public DateTime DocumentDate { get; set; }
        public string Diagnosis { get; set; }
        public string FileName { get; set; }
    }
}
