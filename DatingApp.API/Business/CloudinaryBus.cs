using CloudinaryDotNet;
using DatingApp.API.Helpers;
using Microsoft.Extensions.Options;

namespace DatingApp.API.Business
{
    public class CloudinaryBus
    {
        public readonly IOptions<CloudinarySettings> _cloudinarySettings;
        public Cloudinary _cloudinary;

        public CloudinaryBus(IOptions<CloudinarySettings> cloudinarySettings)
        {
            _cloudinarySettings = cloudinarySettings;

            Account acc = GetAcc();
            _cloudinary = new Cloudinary(acc);
        }

        public CloudinaryDotNet.Account GetAcc() => new CloudinaryDotNet.Account(
                _cloudinarySettings.Value.ApiName,
                _cloudinarySettings.Value.ApiKey,
                _cloudinarySettings.Value.ApiSerect
        );
    }
}