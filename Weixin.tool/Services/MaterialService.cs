using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Weixin.Tool.Enums;
using Weixin.Tool.Models;
using Weixin.Tool.Utility;
using Weixin.Tool.WxResults;
using Newtonsoft.Json;

namespace Weixin.Tool.Services
{
    public class MaterialService:IService
    {
        /// <summary>
        /// 新增永久图文素材
        /// </summary>
        /// <param name="news"></param>
        /// <returns></returns>
        public AddMaterialResult AddMaterialNews(Material_News news)
        {
            return ApiHandler.TryCommonApi(delegate (string accessToken)
            {
                HttpContent content = new StringContent(JsonConvert.SerializeObject(news));
                var result = ApiHandler.PostGetJson<AddMaterialResult>(string.Format(ApiConfig.materialadd_news, accessToken), content);

                return result;
            });
        }
        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="media"></param>
        /// <returns></returns>
        public AddMaterialResult UploadiImg(string media)
        {
            if (!FileUtility.IsLocalFile(media))
            {
                media= ApiHandler.Download(media);
            }
            return ApiHandler.TryCommonApi(delegate (string accessToken)
            {
                var filedict = new Dictionary<string, string>();
                filedict.Add("media", media);
                var content = ApiHandler.CreateMultipartFormDataContent(filedict);
                var result = ApiHandler.PostGetJson<AddMaterialResult>(string.Format(ApiConfig.uploadimg, accessToken), content);

                return result;
            });
        }
        /// <summary>
        /// 新增永久素材
        /// </summary>
        /// <param name="type"></param>
        /// <param name="media">从用户消息中取</param>
        public AddMaterialResult AddMaterial(UploadMediaFileType type,string media)
        {
            if (!FileUtility.IsLocalFile(media))
            {
                media = ApiHandler.Download(media);
            }
            return ApiHandler.TryCommonApi(delegate (string accessToken)
            {
                var filedict = new Dictionary<string, string>();
                filedict.Add("media", media);
                var content = ApiHandler.CreateMultipartFormDataContent(filedict);
                var result = ApiHandler.PostGetJson<AddMaterialResult>(string.Format(ApiConfig.materialadd_material, accessToken, type), content);

                return result;
            });
        }
        /// <summary>
        /// 新增临时素材
        /// </summary>
        /// <param name="type"></param>
        /// <param name="media"></param>
        /// <returns></returns>
        public AddMaterialResult UploadMaterial(UploadMediaFileType type, string media)
        {
            if (!FileUtility.IsLocalFile(media))
            {
                media = ApiHandler.Download(media);
            }
            return ApiHandler.TryCommonApi(delegate (string accessToken)
            {
                var filedict = new Dictionary<string, string>();
                filedict.Add("media", media);
                var content = ApiHandler.CreateMultipartFormDataContent(filedict);
                var result = ApiHandler.PostGetJson<AddMaterialResult>(string.Format(ApiConfig.mediaUpload, accessToken, type), content);

                return result;
            });
        }
        /// <summary>
        /// 素材总数
        /// </summary>
        /// <returns></returns>
        public AddMaterialResult Get_materialcount()
        {
            return ApiHandler.TryCommonApi(delegate (string accessToken)
            {
                var result = ApiHandler.GetJson<AddMaterialResult>(string.Format(ApiConfig.materialget_materialcount, accessToken));

                return result;
            });
        }
        /// <summary>
        /// 获取素材列表
        /// </summary>
        /// <returns></returns>
        public MaterialListResult Batchget_material(UploadMediaFileType type, int offset,int count)
        {
            return ApiHandler.TryCommonApi(delegate (string accessToken)
            {
                var dict = new Dictionary<string, string>();
                dict.Add("type", type.ToString());
                dict.Add("offset", offset.ToString());
                dict.Add("count", count.ToString());
                HttpContent content = new StringContent(JsonConvert.SerializeObject(dict));
                var result = ApiHandler.PostGetJson<MaterialListResult>(string.Format(ApiConfig.materialbatchget_material, accessToken), content);

                return result;
            });
        }
        /// <summary>
        /// 获取永久素材
        /// </summary>
        /// <param name="media_id"></param>
        /// <returns></returns>
        public Dictionary<string, object> Get_material(string media_id)
        {
            return ApiHandler.TryCommonApi(delegate (string accessToken)
            {
                var dict = new Dictionary<string, string>();
                dict.Add("media_id", media_id);
                HttpContent content = new StringContent(JsonConvert.SerializeObject(dict));
                var result = ApiHandler.PostGetJson<Dictionary<string, object>>(string.Format(ApiConfig.materialget_material, accessToken), content);

                return result;
            });
        }
    }
}
