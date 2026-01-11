using System.ComponentModel.DataAnnotations;

namespace NotatnikAPI.DTOs
{
    public class UserDto
    {
        [Required]
        public required string Email { get; set; }

        [Required]
        public required string Password { get; set; }
    }
}

