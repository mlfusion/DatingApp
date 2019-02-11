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

        public async Task<Photo> SetMainPhoto(int userId, int id)
        {
            var isMainPhotoCurrent = await _photoRepository.GetPhoto(x => x.IsMain && x.UserId == userId);

            if (isMainPhotoCurrent != null)
            {
                isMainPhotoCurrent.IsMain = false;
                isMainPhotoCurrent.Modified = DateTime.Now;
            }

            var isMainPhotoUpdate = await _photoRepository.GetPhoto(x => x.Id == id);
            isMainPhotoUpdate.IsMain = true;
            isMainPhotoUpdate.Modified = DateTime.Now;

            if (await _photoRepository.SaveAll())
                return isMainPhotoUpdate;

            return null;
        }

// List<Model> modelList = _unitOfWork.ModelRepository.Get(m => m.FirstName == "Jan" || m.LastName == "Holinka", includeProperties: "Account")
        public async Task<Photo> GetMainPhoto(int userId, int id)
        {
            var photo = await _photoRepository.GetPhoto(x => x.Id == id && x.IsMain);
          //  var photos = await _photoRepository.GetPhoto(x => x.Id == id && x.IsMain, null, s => s.Comment, s => s.User);

            return photo;
        }

        public async Task<Photo> DeletePhoto(int userId, Photo photo)
        {

            if (photo.PublicId != null)
            {
                var deleteParams = new DeletionParams(photo.PublicId);

                var result = _cloudinary.Destroy(deleteParams);

                if(result.Result != "ok")
                    return null;
            }

            photo.Modified = DateTime.Now;
            _photoRepository.Delete(photo);

            if (await _photoRepository.SaveAll())
                return photo;

            return null;        
        }
    }

    public interface IPhotoBus
    {
        Task<PhotoForCreationDto> AddPhoto(int userId, PhotoForCreationDto photoForCreationDto);

        Task<Photo> GetPhoto(int id);

        Task<IEnumerable<Photo>> GetPhotos();

         Task<Photo> GetMainPhoto(int userId, int id);

          Task<Photo> SetMainPhoto(int userId, int id);

           Task<Photo> DeletePhoto(int userId, Photo photo);
    }
}