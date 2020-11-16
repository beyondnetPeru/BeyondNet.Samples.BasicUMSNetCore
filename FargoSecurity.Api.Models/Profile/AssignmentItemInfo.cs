namespace FargoSecurity.Api.Models.Profile
{
    public class AssignmentItemInfo
    {
        public int AssignmentItemId { get; set; }
        public int AssignmentId { get; set; }
        public int OptionId { get; set; }
        public string OptionName { get; set; }
        public bool CanRead { get; set; }
        public string Status { get; set; }
    }
}
