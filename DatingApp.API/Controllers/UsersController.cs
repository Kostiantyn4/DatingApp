using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Data;
using DatingApp.API.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IDatingRepository _repo;
        private readonly IMapper _mapper;
        public UsersController(IDatingRepository repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _repo.GetUsers();
            var resultUsers = _mapper.Map<IEnumerable<UserDto>>(users);
            return Ok(resultUsers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _repo.GetUser(id);
            var resultUser = _mapper.Map<DetailedUserDto>(user);
            return Ok(resultUser);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(DetailedUserDto user)
        {
            var userId = user.Id;
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            
            if (userId != int.Parse(claim.Value))
                return Unauthorized();

            var existingUser = await _repo.GetUser(userId);
            
            _mapper.Map(user, existingUser);
            
            if (await _repo.SaveAll())
                return NoContent();
            
            throw new ApplicationException($"Updating user {userId} failed on save");
        }
    }
}