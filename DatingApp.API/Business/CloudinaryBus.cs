using CloudinaryDotNet;
using DatingApp.API.Helpers;
using Microsoft.Extensions.Options;

namespace DatingApp.API.Business
{
   public abstract class CloudinaryBus
    {
        private readonly IOptions<CloudinarySettings> _cloudinarySettings;
        public Cloudinary _cloudinary;
        public abstract string SelectedPhoto {get;set;} 

        public CloudinaryBus(IOptions<CloudinarySettings> cloudinarySettings)
        {
            _cloudinarySettings = cloudinarySettings;

            Account acc = GetAcc();
            _cloudinary = new Cloudinary(acc);
        }

        private CloudinaryDotNet.Account GetAcc() => new CloudinaryDotNet.Account(
                _cloudinarySettings.Value.ApiName,
                _cloudinarySettings.Value.ApiKey,
                _cloudinarySettings.Value.ApiSerect
        );
    }

}