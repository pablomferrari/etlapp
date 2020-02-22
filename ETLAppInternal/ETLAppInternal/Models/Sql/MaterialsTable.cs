using System;

namespace ETLAppInternal.Models.Sql
{
    public class MaterialsTable : BaseTable
    {
       
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
        public string Units { get; set; }
        public double Quantity { get; set; }
        public bool Assumed { get; set; }
        public bool Positive { get; set; }
        public string Location { get; set; }
        public string Description => $"{Material} {MaterialSub} {Size}";
        public bool IsLocal { get; set; }

        public bool IsNew { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? EditedDate { get; set; }

        public int? CreatedBy { get; set; }
        public int? EditedBy { get; set; }

    }
}
