using System;

namespace ETLAppInternal.Constants
{
    public class CacheNameConstants
    {
        public static string Samples = "ETCSamples";
        public static string Materials = "ETCMaterials";
        public const string EtcJobs = "ETCJobs";
        public const string Mappings = "ETCMappings";
        public const string DeliveryRequests = "ETCDeliveryRequests";
        public static string NewMaterials = "NewMaterials";
        public static string NewSamples = "NewSamples";

        public static string SyncData()
        {
            return "ETCSyncData_" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");
        }
    }
}
