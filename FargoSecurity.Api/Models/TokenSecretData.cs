namespace FargoSecurity.Api.Models
{
    public class TokenSecretData
    {
        public string SystemCode { get; set; }
        public string SecretKey { get; set; }
        public int ExpirationTime { get; set; }
        public string PeriodType { get; set; }
    }
}
