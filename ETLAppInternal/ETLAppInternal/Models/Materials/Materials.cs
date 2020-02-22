using System;
using System.Collections.Generic;

namespace ETLAppInternal.Models.Materials
{
    public class Materials
    {
        public int Id { get; set; }
        public int JobId { get; set; }
        public string ClientMaterialId { get; set; }
        public string Material { get; set; }
        public string Classification { get; set; }
        public string MaterialSub { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }
        public string Friable { get; set; }
        public string Note1 { get; set; }
        public string Note2 { get; set; }
        public IList<Samples.Samples> Samples { get; set; }
        public string Units { get; set; }
        public double Quantity { get; set; }
        public bool Assumed { get; set; }
        public bool Positive { get; set; }
        public string Location { get; set; }
        public bool IsLocal { get; set; }
        public bool IsNew { get; set; }
        

        public bool CanViewSamples { get; set; }

        public string Description => $"{Material} {MaterialSub} {Size}";

        public string Header => $"{JobId} {ClientMaterialId}";

        public DateTime? CreatedDate { get; set; }

        public DateTime? EditedDate { get; set; }

        public int? CreatedBy { get; set; }
        public int? EditedBy { get; set; }

        public Materials()
        {
            IsLocal = true;
            IsNew = true;
            CanViewSamples = false;
        }
        public Materials(int id)
        {
            this.ClientMaterialId = id.ToString();
            this.IsLocal = true;
            this.IsNew = true;
        }
    }
}