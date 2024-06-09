namespace Web_ASM_Nhom6.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public bool IsDelete { get; set; }
        public string role {  get; set; }
    }
}
