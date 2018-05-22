using System.ComponentModel.DataAnnotations;

namespace DatingApp.API.Dto
{
    public class UserDto
    {
        [Required]
        public string Name { get; set; }
        
        [Required]
        [StringLength(12, MinimumLength = 4, ErrorMessage = "Please specify a password between 4 and 12 characters")]
        public string Password { get; set; }
    }
}