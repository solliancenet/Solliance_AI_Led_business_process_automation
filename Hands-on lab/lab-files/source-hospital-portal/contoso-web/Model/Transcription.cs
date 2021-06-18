using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace contoso_web.Model
{
    public class Transcription
    {
        public Transcription()
        {
            HealthcareEntities = new List<HealthcareEntity>();
        }
        public string TranscribedText { get; set; }
        public string HTML { get; set; }
        public string AudioFileUrl { get; set; }
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
