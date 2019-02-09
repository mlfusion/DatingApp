using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Helpers;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System;

namespace DatingApp.API.Business
{
    public class PhotoBus : IPhotoBus
    {
        private readonly IPhotoRepository _photoRepository;
        private readonly IDatingRepository _datingRepository;
        private readonly IMapper _mapper;
        private readonly IOptions<CloudinarySettings> _cloudinarySettings;
        private Cloudinary _cloudinary;

        public PhotoBus(IPhotoRepository photoRepository, IDatingRepository datingRepository, 
                               IMapper mapper, IOptions<CloudinarySettings> cloudinarySettings)
        {
            _photoRepository = photoRepository;
            _datingRepository = datingRepository;
            _mapper = mapper;
            _cloudinarySettings = cloudinarySettings;

            _cloudinary = new Cloudinary(GetAcc());
        }

        
        private CloudinaryDotNet.Account GetAcc()
         => new CloudinaryDotNet.Account(
                _cloudinarySettings.Value.ApiName,
                _cloudinarySettings.Value.ApiKey,
                _cloudinarySettings.Value.ApiSerect
        );

        public async Task<PhotoForCreationDto> AddPhoto(int userId, PhotoForCreationDto photoForCreationDto)
        {
            var file = photoForCreationDto.File;

            var uploadResult = new ImageUploadResult();

            if (file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams
                    {
                        PublicId = "Dating/" + $"image_{DateTime.Now.ToString("yyyyMMddHHmmss")}",
                        File = new FileDescription(file.Name, stream),
                        Transformation = new Transformation()
                            .Width(500).Height(500).Crop("fill").Gravity("face")
                    };

                    uploadResult = _cloudinary.Upload(uploadParams);
                }
            }

            if (uploadResult == null)
                return null;

            photoForCreationDto.Url = uploadResult.Uri.ToString();
            photoForCreationDto.PublicId = uploadResult.PublicId;
            photoForCreationDto.UserId = userId;

            var photo = _mapper.Map<Photo>(photoForCreationDto);
            var photoIsMain = await _photoRepository.GetPhoto(x => x.UserId == userId && x.IsMain);

            if (photoIsMain == null)
                photo.IsMain = true;
            
            _photoRepository.Add(photo);

            if (await _photoRepository.SaveAll())
            {
                photoForCreationDto.Id = photo.Id;
                return photoForCreationDto;
            }

            // Return null is Save() failed
            return photoForCreationDto = null;
        }

 
        public async Task<Photo> GetPhoto(int id)
        {
            var photo = await _photoRepository.GetPhoto(id);

            return photo;
        }

        public async Task<IEnumerable<Photo>> GetPhotos()
        {
            var photos = await _photoRepository.GetPhotos();

            return photos;
        }
    }

    public interface IPhotoBus
    {
        Task<PhotoForCreationDto> AddPhoto(int userId, PhotoForCreationDto photoForCreationDto);

        Task<Photo> GetPhoto(int id);

        Task<IEnumerable<Photo>> GetPhotos();
    }
}