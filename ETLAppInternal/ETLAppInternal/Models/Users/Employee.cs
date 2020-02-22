namespace ETLAppInternal.Models.Users
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public string Login { get; set; }
        public string SignaturePath { get; set; }
        public bool CanViewOtherTimeSheets { get; set; }
        public bool DoesFieldWork { get; set; }
        public bool DoesTraining { get; set; }
        public string Address { get; set; }
        public byte[] Signature { get; set; }
        public string SignatureExtension { get; set; }
    }
}
