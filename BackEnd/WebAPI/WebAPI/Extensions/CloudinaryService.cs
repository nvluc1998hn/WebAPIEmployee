//using System.Collections.Generic;
//using System.Web;
//using CloudinaryDotNet;
//using CloudinaryDotNet.Actions;
//using Microsoft.AspNetCore.Http;

//public class CloudinaryService
//{
//    private readonly Cloudinary _cloudinary;

//    /// <summary>
//    /// Set up cloudinary acccount 
//    /// </summary>
//    /// <param name="apiKey">The Api Key</param>
//    /// <param name="apiSecret">The Api Secret</param>
//    /// <param name="cloudName">Optional CloudName</param>


//    public CloudinaryService(string apiKey = "252435488743454", string apiSecret = "-ehz4TbU3KmBEiT7krNU2IJRkdI", string cloudName = "thangdovan")
//    {
//        var myAccount = new Account { ApiKey = apiKey, ApiSecret = apiSecret, Cloud = cloudName };
//        _cloudinary = new Cloudinary(myAccount);
//    }


//    //var CLOUD_NAME = "thangdovan";
//    //var API_KEY = "252435488743454";
//    //var API_SERECT = "-ehz4TbU3KmBEiT7krNU2IJRkdI";

//    /// <summary>
//    /// Upload image using HttpPostedFileBase
//    /// </summary>
//    /// <param name="file"></param>
//    /// <returns></returns>

//    public ImageUploadResult UploadImage(IFormFile file)
//    {
//        if (file != null)
//        {
//            var uploadParams = new ImageUploadParams
//            {
//                File = new FileDescription(file.FileName, file.OpenReadStream()),
//                Transformation = new Transformation().Crop("thumb").Gravity("face")
//            };

//            var uploadResult = _cloudinary.Upload(uploadParams);
//            return uploadResult;
//        }
//        return null;
//    }

//    ///// <summary>
//    ///// Upload image using HttpPostedFile
//    ///// </summary>
//    ///// <param name="file"></param>
//    ///// <returns></returns>

//    //public ImageUploadResult UploadImage(HttpPostedFile file)
//    //{
//    //    if (file != null)
//    //    {
//    //        var uploadParams = new ImageUploadParams
//    //        {
//    //            File = new FileDescription(file.FileName, file.InputStream),
//    //            Transformation = new Transformation().Width(200).Height(200).Crop("thumb").Gravity("face")
//    //        };

//    //        var uploadResult = _cloudinary.Upload(uploadParams);
//    //        return uploadResult;
//    //    }
//    //    return null;
//    //}
//}
