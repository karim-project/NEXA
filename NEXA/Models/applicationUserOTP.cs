namespace NEXA.Models
{
    public class applicationUserOTP
    {
        public string Id { get; set; }
        public string OTP { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime ValidTo { get; set; }
        public bool Isvalid { get; set; }

        public string ApplicationUserId { get; set; }
        public ApplicationUser applicationUser { get; set; }

    }
}
