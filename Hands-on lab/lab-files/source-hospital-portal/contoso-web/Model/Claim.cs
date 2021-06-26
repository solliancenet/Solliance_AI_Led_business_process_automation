using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace contoso_web.Model
{
    public class Claim
    {
        public string Id { get; set; }
        public string PatientName { get; set; }
        public DateTimeOffset PatientBirthDate { get; set; }
        public long TotalCharges { get; set; }
        public long AmountPaid { get; set; }
        public long AmountDue { get; set; }
        public string InsuredId { get; set; }
        public DateTimeOffset DocumentDate { get; set; }
        public string Diagnosis { get; set; }
        public string FileName { get; set; }
    }
}
