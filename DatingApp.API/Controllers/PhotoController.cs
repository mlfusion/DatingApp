using AutoMapper;
using CloudinaryDotNet;
using DatingApp.API.Base;
using DatingApp.API.Business;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Helpers;
using DatingApp.API.Models;
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
        private readonly ILog _log;

        public PhotoController(IPhotoBus photoBus, IMapper mapper, ILog log)
        {
            _photoBus = photoBus;
            _mapper = mapper;
            _log = log;
        }

        [HttpPost]
        public async Task<IActionResult> AddPhoto(int userId, [FromForm] PhotoForCreationDto photoForCreationDto)
        {
            using(_log.BeginScope())
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
                    _log.Write(ex);
                    throw new Exception(ex.Message);
                }
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetPhotos()
        {
            using(_log.BeginScope())
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
                    _log.Write(ex);
                    throw new Exception(ex.Message);
                }
            }
        }

        // /api/users/userid/photos/{id}
        [HttpGet("{id}", Name = "GetPhoto")]
        public async Task<IActionResult> GetPhoto(int id)
        {
            using(_log.BeginScope())
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
                    _log.Write(ex);
                    throw new Exception(ex.Message);
                }
            }
        }

        [HttpPost("{id}/setMainPhoto")]
        public async Task<IActionResult> SetMainPhoto(int userId, int id)
        {
            using(_log.BeginScope())
            {
                ValidateUser(userId);

                var photo = await _photoBus.GetPhoto(id);

                if (photo == null)
                    return Unauthorized();

                var photoIsMain = await _photoBus.GetMainPhoto(userId, id);

                if (photoIsMain != null)
                    return BadRequest("This is already the main photo");

                var setIdToMainPhoto = await _photoBus.SetMainPhoto(userId, id);

                if (setIdToMainPhoto == null)
                    return BadRequest("Could not photo photo to main. Please try again");

                return NoContent();
            }
        }

        [HttpPost("{id}/deletePhoto")]
        public async Task<IActionResult> DeletePhoto(int userId, int id)
        {
            using(_log.BeginScope())
            {
                ValidateUser(userId);

                var photo = await _photoBus.GetPhoto(id);

                if (photo == null)
                {
                    _log.Write($"No photo with photoId:{id} was found with userId={userId}.");
                    return Unauthorized();
                }

                var photoIsMain = await _photoBus.GetMainPhoto(userId, id);

                if (photoIsMain != null)
                {
                    _log.Write($"UserId={userId} can\'t delete photoId={id} cause isMain photo = true.");
                    return BadRequest("You cant\'t delete your main");
                }

            // var photoDtoToPhoto = _mapper.Map<PhotoForDetailDto, Photo>(photoDto);

                var deletedPhoto = await _photoBus.DeletePhoto(userId, photo);

                if (deletedPhoto == null)
                {
                    _log.Write($"Couldn\'t delete photoId={id} cause of an issue.");
                    return BadRequest("Could not delete the photo. Please try again");
                }

                _log.Write($"Successfully deleted photoid={id} with userId={userId};");
                return NoContent();   
            }      
        }

        private IActionResult ValidateUser(int userId)
        {
            using(_log.BeginScope())
            {
                if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                {
                    _log.Write($"UserId:{userId} is unauthorized for this action.");
                    return Unauthorized();
                }

                _log.Write($"UserId:{userId} is authorized for this action.");
                return null;
            }
        }
    }
}