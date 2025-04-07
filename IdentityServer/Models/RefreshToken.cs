namespace IdentityServer.Models
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Token { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsRevoked { get; set; }
        //public string AccessToken { get; set; }
        public User User { get; set; }
    }
}
