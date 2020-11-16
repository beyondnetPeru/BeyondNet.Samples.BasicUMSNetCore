namespace FargoSecurity.Api.Models
{
    public class TokenData
    {
        public int UserId { get; set; }
        public int ProfileId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string SystemCode { get; set; }
        public string SystemName { get; set; }
    }
}
