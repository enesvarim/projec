using projec.Models;

namespace projec.Dtos
{
    public class RegisterDto
    {
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int BirthYear { get; set; }
        public Position Position { get; set; }
    }
}
