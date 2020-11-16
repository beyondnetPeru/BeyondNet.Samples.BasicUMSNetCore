namespace FargoSecurity.Api.Models
{
    public class RequestLoginCommand
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string SystemCode { get; set; }
    }
}
