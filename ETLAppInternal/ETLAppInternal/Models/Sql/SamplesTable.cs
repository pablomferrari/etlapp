using SQLite;

namespace ETLAppInternal.Models.Sql
{
    public class SamplesTable : BaseTable
    {
        public long JobId { get; set; }
        public long MaterialId { get; set; }
        public string ClientSampleId { get; set; }
        public string SampleLocation { get; set; }
        public string DateCollected { get; set; }
        public string SampleDescription { get; set; }
        public bool IsLocal { get; set; }
        public bool IsNew { get; set; }
    }
}
