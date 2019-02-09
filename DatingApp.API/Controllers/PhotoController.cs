using AutoMapper;
using CloudinaryDotNet;
using DatingApp.API.Business;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DatingApp.API.Controllers
{
    [Authorize]
    [Route("api/users/{userid}/photos")]
    [ApiController]
    public class PhotoController : ControllerBase
    {
        private readonly IPhotoBus _photoBus;
        private readonly IMapper _mapper;

        public PhotoController(IPhotoBus photoBus, IMapper mapper)
        {
            _photoBus = photoBus;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> AddPhoto(int userId, [FromForm] PhotoForCreationDto photoForCreationDto)
        {
            try
            {
                var useid = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

                //if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                //    return Unauthorized();

                var photoCreation = await _photoBus.AddPhoto(userId, photoForCreationDto);

                if (photoCreation == null)
                    return BadRequest("Could not add the photo");

                return CreatedAtRoute("GetPhoto", new { id = photoCreation.Id }, photoCreation);
            }
            catch(Exception ex)
            {
                 throw new Exception(ex.Message);
            }

        }

        [HttpGet]
        public async Task<IActionResult> GetPhotos()
        {
            try
            {
                var photos = await _photoBus.GetPhotos();

                if (photos == null)
                    return NotFound();

                var photoDtos = _mapper.Map<IEnumerable<PhotoForCreationDto>>(photos);

                return Ok(photoDtos); 
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // /api/users/userid/photos/{id}
        [HttpGet("{id}", Name = "GetPhoto")]
        public async Task<IActionResult> GetPhoto(int id)
        {
            try
            {
                var photo = await _photoBus.GetPhoto(id);

                if (photo == null)
                    return NotFound();

                var photoDto = _mapper.Map<PhotoForCreationDto>(photo);

                return Ok(photoDto); 
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}