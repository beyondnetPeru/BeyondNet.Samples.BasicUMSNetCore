namespace FargoSecurity.Api.Models.System
{
    public class SystemEdit
    {
        public string SystemType { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string SecretToken { get; set; }
        public string PeriodTypeToken { get; set; }
        public int ExpirationTimeToken { get; set; }
        public int Status { get; set; }
    }
}
