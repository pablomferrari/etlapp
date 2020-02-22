namespace ETLAppInternal.Constants
{
    public static class ApiConstants
    {
#if DEBUG
        public const string BaseApiUrl = "https://surveyapi.conveyor.cloud/"; 
        //public const string BaseApiUrl = "http://labapi.2etc.com/";
#else
        public const string BaseApiUrl = "http://labapi.2etc.com/";
#endif


        public const string AuthenticateEtcLogin = "api/authentication/etclogin";
        public const string AuthenticateClientLogin = "api/authentication/clientlogin";
        public const string EtcJobs = "api/asbestossurvey/etcjobs";
        public const string ClientJobs = "api/asbestossurvey/clientjobs";
        public const string Materials = "api/asbestossurvey/materials";
        public const string Samples = "api/asbestossurvey/samples";
        public const string PostMaterials = "api/materials/addrange";
        public const string PutMaterials = "api/materials/updaterange";
        public const string PostSamples = "api/samples/addrange";
        public const string PutSamples = "api/samples/updaterange";
        public const string Mappings = "api/materialsmapping/getall";
        public const string PostDeliveries = "api/asbestossurvey/deliver";

        public const string Jobs = "api/jobs";
        public const string MaterialsV3 = "api/materials";
        public const string SamplesV3 = "api/samples";
        public const string MappingsV3 = "api/mappings";
        public const string PostData = "api/survey";
    }
}
