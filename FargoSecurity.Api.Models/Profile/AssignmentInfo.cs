namespace FargoSecurity.Api.Models.Profile
{
    public class AssignmentInfo
    {
        public int ProfileId { get; set; }
        public int AssignmentId { get; set; }
        public int ModuleId { get; set; }
        public string ModuleName { get; set; }
        public bool CanRead { get; set; }
        public string Status { get; set; }
    }
}
