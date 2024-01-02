namespace ChauffeurApp.Core.Entities
{
    public class OTP
    {
        public long UserID { get; set; }
        public string TranName { get; set; }
        public string OneTimePassword { get; set; }
        public DateTime ExpiresOn { get; set; }
        public bool IsActive { get; set; }
    }
}
