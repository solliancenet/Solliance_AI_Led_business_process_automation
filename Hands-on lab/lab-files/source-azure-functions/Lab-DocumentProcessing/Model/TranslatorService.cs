using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DocumentProcessing.Model
{
    public class TranslatorService
    {
        public partial class Root
        {
            [JsonProperty("detectedLanguage")]
            public DetectedLanguage DetectedLanguage { get; set; }

            [JsonProperty("translations")]
            public List<Translation> Translations { get; set; }
        }

        public partial class DetectedLanguage
        {
            [JsonProperty("language")]
            public string Language { get; set; }

            [JsonProperty("score")]
            public long Score { get; set; }
        }

        public partial class Translation
        {
            [JsonProperty("text")]
            public string Text { get; set; }

            [JsonProperty("to")]
            public string To { get; set; }
        }
    }
}
