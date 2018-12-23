using System;
using System.Collections;
using System.Collections.Generic;
using DatingApp.API.Models;

namespace DatingApp.API.Dto
{
    public class DetailedUserDto : UserDto
    {
        public string Introduction { get; set; }
        public string LookingFor { get; set; }
        public string Interests { get; set; }
        public ICollection<PhotoDto> Photos { get; set; }
    }
}