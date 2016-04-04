using AutoMapper;
using Flat4Me.Core.Auth;
using Flat4Me.Core.Consts;
using Flat4Me.Data.DTO;
using Flat4Me.Data.DTO.Short;
using Flat4Me.Data.Repository.Interfaces;
using Flat4Me.Data.Repository.Interfaces.Short;
using Flat4Me.Identity;
using Flat4Me.Web.Portal.App_Start;
using Flat4Me.Web.Portal.Models;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace Flat4Me.Web.Portal.Controllers.Api
{
    [RoutePrefix("api/accommodation")]
    [F4MeAuthorize(Roles = UserRoleList.Hotelier)]
    // EmailConfirmed
    [F4MeAuthorize(ClaimType = F4MeClaims.EmailConfirmed, ClaimValue = F4MeClaims.EmptyValue)]
    // PhoneConfirmed
    [F4MeAuthorize(ClaimType = F4MeClaims.PhoneConfirmed, ClaimValue = F4MeClaims.EmptyValue)]
    // HotelierApproved
    [F4MeAuthorize(ClaimType = F4MeClaims.HotelierApproved, ClaimValue = F4MeClaims.EmptyValue)]
    public class AccommodationController : BaseApiController
    {
        #region Repositories

        [Inject]
        public IAccommodationRepository AccommodationRepository { get; set; }
        [Inject]
        public IPhotoRepository PhotoRepository { get; set; }
        [Inject]
        public IMapRepository MapRepository { get; set; }
        [Inject]
        public IReservationRepository ReservationRepository { get; set; }

        #endregion

        // main list
        [HttpGet]
        [AllowAnonymous]
        [Route("city/{cityId:int}")]
        public async Task<IHttpActionResult> GetByCity(int cityId)
        {
            var dbResult = await AccommodationRepository.MainList(cityId);
            return Ok(dbResult);
        }

        // list for user
        [HttpGet]
        [Route("user/{userId:int}")]
        public async Task<IHttpActionResult> GetByUser(int userId)
        {
            var result = await AccommodationRepository.MyList(userId);
            return Ok(result);
        }

        // get one
        [HttpGet]
        [AllowAnonymous]
        public async Task<IHttpActionResult> Get(int id)
        {
            var accommodationTask = AccommodationRepository.Get(id);
            var priceListTask = AccommodationRepository.GetPriceList(id);
            var photoListTask = PhotoRepository.GetList(id);
            var locationTask = MapRepository.GetConfirmedLocation(id);

            await Task.WhenAll(new Task[] { accommodationTask, photoListTask, priceListTask, locationTask });

            var accommodation = Mapper.Map<AccommodationShortModel>(accommodationTask.Result);
            accommodation.PhotoList = photoListTask.Result.Select(p => Mapper.Map<PhotoModel>(p)).ToList();
            accommodation.PriceList = priceListTask.Result.Select(p => Mapper.Map<PriceModel>(p)).ToList();
            accommodation.Location = Mapper.Map<ViewLocationModel>(locationTask.Result);

            return Ok(accommodation);
        }

        // Create draft version
        [HttpGet]
        [Route("addDraft")]
        public async Task<IHttpActionResult> AddDraft()
        {
            var dbItem = await AccommodationRepository.AddDraft(UserId);
            // Default params
            dbItem.AccommodationTypeCode = AccommodationTypeList.Apartment;
            dbItem.CurrencyCode = CurrencyList.Rub;

            var item = Mapper.Map<AccommodationShortModel>(dbItem);
            // DTO return 0, because has struct propertie, but in fact this value is empty.
            item.Area = null;
            // Add one price item with default values
            item.PriceList = new List<PriceModel> { new PriceModel { DurationDays = 1 } };

            return Ok(item);
        }

        // complete accommodation adding.
        [HttpPost]
        [Route("completeDraft")]
        public async Task CompleteDraft([FromBody] AccommodationShortModel accommodation)
        {
            var accommodationDTO = Mapper.Map<AccommodationShortDTO>(accommodation);
            var priceListDTO = accommodation.PriceList.Select(p => Mapper.Map<PriceDTO>(p));            
            accommodationDTO.UserId = UserId;
            accommodationDTO.IsPublished = true;
            accommodationDTO.IsApproved = true;

            await AccommodationRepository.CompleteDraft(accommodationDTO, priceListDTO);
        }

        // Update accommodation
        [HttpPost]
        [Route("update")]
        public async Task Post([FromBody] AccommodationShortModel accommodation)
        {
            var accommodationDTO = Mapper.Map<AccommodationShortDTO>(accommodation);
            var priceListNew = accommodation.PriceList.Where(p => p.PriceId.HasValue == false).Select(p => Mapper.Map<PriceDTO>(p));
            var priceListUpdate = accommodation.PriceList.Where(p => p.PriceId.HasValue == true).Select(p => Mapper.Map<PriceDTO>(p));

            accommodationDTO.UserId = UserId;
            accommodationDTO.IsPublished = true;
            accommodationDTO.IsApproved = true;

            await AccommodationRepository.Update(accommodationDTO, priceListNew, priceListUpdate);
        }

        // publish
        [HttpPut]
        [Route("publish/{id:int}")]
        public async Task<IHttpActionResult> Publish(int id)
        {
            return Ok();
        }

        // delete
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            return Ok();
        }

        
        [HttpPut]
        [Route("reservationList")]
        public async Task<IHttpActionResult> GetReservationList([FromBody] GetReservationListCriteriaModel criteria)
        {
            var listDto = await ReservationRepository.GetList(criteria.AccommodationId.Value, criteria.CheckinStart);
            var list = listDto.Select(p => Mapper.Map<Short_ReservationModel>(p));

            return Ok(list);
        }

        [HttpPost]
        [Route("searchAccommodation")]
        public async Task<IHttpActionResult> SearchAccommodation([FromBody] SearchCriteria criteria)
        {
            // Db search
            var model = new SearchResult
            {
                AccommodationList = new List<AccommodationShortMainDTO>(),
                YandexMapJson = string.Empty
            };

            return Ok(model);
        }
    }
}