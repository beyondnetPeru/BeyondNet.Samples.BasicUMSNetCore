namespace FargoSecurity.Api.Models.Profile
{
    public class AssignmentItemEdit
    {
        public int AssignmentId { get; set; }
        public int OptionId { get; set; }
        public bool CanRead { get; set; }
        public int Status { get; set; }
    }
}
