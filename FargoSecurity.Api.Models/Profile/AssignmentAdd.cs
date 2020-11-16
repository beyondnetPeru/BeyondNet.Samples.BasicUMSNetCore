namespace FargoSecurity.Api.Models.Profile
{
    public class AssignmentAdd
    {
        public int ProfileId { get; set; }
        public int ModuleId { get; set; }
        public bool CanRead { get; set; }
    }
}
