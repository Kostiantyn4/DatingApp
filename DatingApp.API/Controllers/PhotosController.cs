using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DatingApp.API.Data;
using DatingApp.API.Dto;
using DatingApp.API.Helpers;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DatingApp.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/users/{userId}/photos")]
    public class PhotosController : ControllerBase
    {
        private readonly IDatingRepository _repo;
        private readonly IMapper _mapper;
        private readonly Cloudinary _cloudinary;

        public PhotosController(IDatingRepository repo, 
            IMapper mapper, 
            IOptions<CloudinarySettings> cloudinaryConfig)
        {
            _repo = repo;
            _mapper = mapper;
            
            var acc = new Account(
                cloudinaryConfig.Value.CloudName,
                cloudinaryConfig.Value.ApiKey,
                cloudinaryConfig.Value.ApiSecret
            );
            
            _cloudinary = new Cloudinary(acc);
        }

        [HttpGet("{id}", Name = "GetPhoto")]
        public async Task<IActionResult> GetPhoto(int id)
        {
            var existingPhoto = await _repo.GetPhoto(id);
            var photo = _mapper.Map<UploadPhotoDto>(existingPhoto);

            return Ok(photo);
        }

        [HttpPost]
        public async Task<IActionResult> AddPhotoForUser(int userId, [FromForm]UploadPhotoDto uploadPhotoDto)
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            
            if (userId != int.Parse(claim.Value))
                return Unauthorized();
            
            var existingUser = await _repo.GetUser(userId);

            // Upload photo to cloud storage
            var file = uploadPhotoDto.File;

            var uploadResults = new ImageUploadResult();

            if (file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.Name, stream),
                        Transformation = new Transformation()
                            .Width(500)
                            .Height(500)
                            .Crop("fill")
                            .Gravity("face")
                    };

                    uploadResults = _cloudinary.Upload(uploadParams);
                }
            }

            // Update photo by the response data from cloud storage
            uploadPhotoDto.Url = uploadResults.Uri.ToString();
            uploadPhotoDto.PublicId = uploadResults.PublicId;
            
            // Map uploadPhotoDto into a photo itself based on property that we already have
            var photo = _mapper.Map<Photo>(uploadPhotoDto);
            
            // If the user have no photo, set uploaded photo as main
            if (!existingUser.Photos.Any(x => x.IsMain))
            {
                photo.IsMain = true;
            }
            
            existingUser.Photos.Add(photo);

            if (!await _repo.SaveAll()) 
                return BadRequest("Could not add the photo");
            
            var photoToReturn = _mapper.Map<UploadPhotoDto>(photo);
            return CreatedAtRoute("GetPhoto", new { id = photo.Id }, photoToReturn);
        }

        [HttpPost("{id}/setMain")]
        public async Task<IActionResult> SetMainPhoto(int userId, int id)
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            
            if (userId != int.Parse(claim.Value))
                return Unauthorized();

            var user = await _repo.GetUser(userId);

            if (user.Photos.All(x => x.Id != id))
                return Unauthorized();

            var photoFromRepo = await _repo.GetPhoto(id);

            if (photoFromRepo.IsMain)
                return BadRequest("This is already the main photo!");

            var currentMainPhoto = await _repo.GetMainPhotoForUser(userId);
            currentMainPhoto.IsMain = false;
            
            photoFromRepo.IsMain = true;

            if (await _repo.SaveAll())
                return NoContent();

            return BadRequest("Could not set photo to main!");
        }
    }
}