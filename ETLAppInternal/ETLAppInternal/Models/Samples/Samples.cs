using ETLAppInternal.Models.Sql;
using SQLite;

namespace ETLAppInternal.Models.Samples
{
    public class Samples
    {
        public int Id { get; set; }
        public long JobId { get; set; }
        public long MaterialId { get; set; }
        public string ClientSampleId { get; set; }
        public string SampleLocation { get; set; }
        public string DateCollected { get; set; }
        public bool IsLocal { get; set; }

        public string SampleDescription { get; set; }

        public bool IsNew { get; set; }

        public bool Delete { get; set; }

        public string Header => $"{JobId} {ClientSampleId}";
        public string Description => $"{SampleDescription}";

        public Samples()
        {
            IsLocal = true;
            IsNew = true;
        }
    }
}