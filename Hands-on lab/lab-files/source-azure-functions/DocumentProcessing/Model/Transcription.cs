using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DocumentProcessing.Model
{
    public class Transcription
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        public string TranscribedText { get; set; }
        public string FileName { get; set; }
        public DateTime DocumentDate { get; set; }
    }
}
