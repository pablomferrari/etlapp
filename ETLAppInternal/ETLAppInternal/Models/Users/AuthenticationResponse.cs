namespace ETLAppInternal.Models.Users
{
    public class AuthenticationResponse
    {
        public bool IsAuthenticated { get; set; }
        public Employee Employee { get; set; }
    }
}
