using System.Collections.Generic;


namespace ETLAppInternal.Models.General
{
    public class SurveyData
    {
        public IEnumerable<Materials.Materials> NewMaterials { get; set; }
        public IEnumerable<Materials.Materials> UpdatedMaterials { get; set; }
        public IEnumerable<Samples.Samples> NewSamples { get; set; }
        public IEnumerable<Samples.Samples> UpdatedSamples { get; set; }
        public IEnumerable<DeliveryRequest> Deliveries { get; set; }
    }
}
