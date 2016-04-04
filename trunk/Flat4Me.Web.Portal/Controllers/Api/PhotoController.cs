using Flat4Me.Core.Auth;
using Flat4Me.Core.Rabbit;
using Flat4Me.Data.DTO;
using Flat4Me.Data.Repository.Interfaces;
using Flat4Me.Identity;
using Flat4Me.Web.Portal.App_Start;
using Flat4Me.Web.Portal.Models;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Web.Http;

namespace Flat4Me.Web.Portal.Controllers.Api
{
    [RoutePrefix("api/photo")]
    [F4MeAuthorize(Roles = UserRoleList.Hotelier)]
    // EmailConfirmed
    [F4MeAuthorize(ClaimType = F4MeClaims.EmailConfirmed, ClaimValue = F4MeClaims.EmptyValue)]
    // PhoneConfirmed
    [F4MeAuthorize(ClaimType = F4MeClaims.PhoneConfirmed, ClaimValue = F4MeClaims.EmptyValue)]
    // HotelierApproved
    [F4MeAuthorize(ClaimType = F4MeClaims.HotelierApproved, ClaimValue = F4MeClaims.EmptyValue)]
    public class PhotoController : BaseApiController
    {
        [Inject]
        public IPhotoRepository PhotoRepository { get; set; }

        [HttpPost]
        public async Task<IHttpActionResult> Post(int id)
        {
            var accommodationId = id;
            // Check file type
            if (!Request.Content.IsMimeMultipartContent())
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            // Create reader
            var provider = new MultipartMemoryStreamProvider();
            // request content
            await Request.Content.ReadAsMultipartAsync(provider);
            // Read photos to model
            var incomePhotoList = new List<PhotoUploadModel>();
            foreach (var stream in provider.Contents)
            {
                var buffer = stream.ReadAsByteArrayAsync().Result;
                incomePhotoList.Add(new PhotoUploadModel
                {
                    FileName = stream.Headers.ContentDisposition.FileName.Replace("\"", ""),
                    Data = buffer
                });
            }
            // Create empty dto list. TODO: Remove Approved
            var dbPhotoList = incomePhotoList.Select(p => new PhotoDTO { IsApproved = true }).ToList();
            // Save to db, fill PhotoId
            await PhotoRepository.Add(accommodationId, dbPhotoList);
            // Fill income photo list
            for (int i = 0; i < incomePhotoList.Count; i++)
                incomePhotoList[i].PhotoId = dbPhotoList[i].PhotoId;

            // MIKHAIL IMAGE SERVICE HERE
            foreach (var pf in incomePhotoList)
            {
                using (RabbitFacade rbt = new RabbitFacade(RabbitFacadeConsts.QUEUE_IMAGE_MAIN))
                {
                    try
                    {
                        Dictionary<string, object> headers = new Dictionary<string, object>();
                        headers.Add(RabbitFacadeConsts.HEADER_IMAGE_ID, (Int32)pf.PhotoId.Value);

                        rbt.BeginTran();
                        rbt.Publish(MediaTypeNames.Image.Jpeg, headers, pf.Data);
                        rbt.Commit();
                    }
                    catch (Exception imgMsgEx)
                    {
                        Logger.LogException("PhotoController.Add.Rabbit", imgMsgEx);
                        rbt.Rollback();
                    }
                }
            }

            // Return only name and id            
            return Ok(incomePhotoList.Select(p => new { p.PhotoId, p.FileName }));
        }

        [HttpPut]
        [Route("primary/{id:int}")]
        public async Task Primary(int id)
        {
            await PhotoRepository.SetPrimary(id);
        }

        [HttpDelete]
        public async Task Delete(int id)
        {
            await PhotoRepository.Delete(new List<int>() { id });
        }
    }
}