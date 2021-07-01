using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DocumentProcessing.Model
{
    public class Transcription
    {
        public Transcription()
        {
            HealthcareEntities = new List<HealthcareEntity>();
        }
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        public string TranscribedText { get; set; }
        public string FileName { get; set; }
        public DateTime DocumentDate { get; set; }
        public List<HealthcareEntity> HealthcareEntities { get; set; }
    }

    public class HealthcareEntity
    {
        public string Category { get; set; }
        public string Text { get; set; }
    }
}
