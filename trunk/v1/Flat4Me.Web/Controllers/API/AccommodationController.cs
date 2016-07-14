using AutoMapper;
using Flat4Me.Core.Auth;
using Flat4Me.Data.DTO.Short;
using Flat4Me.Data.Repository.Interfaces;
using Flat4Me.Data.Repository.Interfaces.Short;
using Flat4Me.Identity;
using Flat4Me.Web.App_Start;
using Flat4Me.Web.Models;
using Flat4Me.Web.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Flat4Me.Web.Controllers.Api
{
    [F4MeWebApiAuthorize(Roles = UserRoleList.Guest)]
    // PhoneConfirmed
    [F4MeWebApiAuthorize(ClaimType = F4MeClaims.PhoneConfirmed, ClaimValue = F4MeClaims.EmptyValue)]
    [RoutePrefix("api/accommodation")]
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
        [Inject]
        public IHotelierProfileRepository HotelierProfileRepository { get; set; }
        [Inject]
        public ISearchRepository SearchRepository { get; set; }

        #endregion

        // main list
        [HttpGet]
        [AllowAnonymous]
        [Route("city/{cityId:int}")]
        public async Task<IHttpActionResult> GetByCity(int cityId)
        {
            var dbResult = await SearchRepository.SearchByCity(cityId);
            return Ok(dbResult);
        }

        // get one
        [HttpGet]
        [AllowAnonymous]
        public async Task<IHttpActionResult> Get(int id)
        {
            var tasksToPerform = new List<Task>();

            var accommodationTask = AccommodationRepository.Get(id);
            tasksToPerform.Add(accommodationTask);

            var priceListTask = AccommodationRepository.GetPriceList(id);
            tasksToPerform.Add(priceListTask);

            var photoListTask = PhotoRepository.GetList(id);
            tasksToPerform.Add(photoListTask);

            var locationTask = MapRepository.GetConfirmedLocation(id);
            tasksToPerform.Add(locationTask);

            Task<ReservationDTO> reservationTask = null;
            if (User.Identity.IsAuthenticated)
            {
                // Reservation information
                reservationTask = ReservationRepository.Get(id, UserId);
                tasksToPerform.Add(reservationTask);
            }
            // Wait all tasks
            await Task.WhenAll(tasksToPerform);

            var flatModel = new FlatViewModel();
            flatModel.Flat = Mapper.Map<FlatModel>(accommodationTask.Result);
            flatModel.PhotoList = photoListTask.Result.Select(p => Mapper.Map<PhotoModel>(p)).ToList();
            flatModel.PriceList = priceListTask.Result.Select(p => Mapper.Map<PriceModel>(p)).ToList();
            flatModel.Location = Mapper.Map<ViewLocationModel>(locationTask.Result);
            if (reservationTask != null)
            {
                flatModel.CurrentReservation = Mapper.Map<Short_ReservationModel>(reservationTask.Result);

                if (flatModel.CurrentReservation != null)
                {
                    flatModel.HotelierContacts = Mapper.Map<HotelierContactsModel>(await UserManager.FindByIdAsync(flatModel.Flat.UserId));
                }
            }
            // Hotelier profile
            var hotelierProfileDTO = await HotelierProfileRepository.Get(flatModel.Flat.UserId);
            flatModel.HotelierProfile = Mapper.Map<HotelierProfileModel>(hotelierProfileDTO);

            return Ok(flatModel);
        }


        [HttpPost]
        [Route("addReservation")]
        public async Task<IHttpActionResult> AddReservation([FromBody]Short_ReservationModel reservation)
        {
            var accommodation = await AccommodationRepository.Get(reservation.AccommodationId.Value);
            if (accommodation == null)
            {
                return BadRequest();
            }

            var dto = Mapper.Map<ReservationDTO>(reservation);
            dto.UserId = UserId;
            dto.CreatedOnUtc = DateTime.UtcNow;

            await ReservationRepository.Add(dto);

            Exception ex = null;
            try
            {
                // Send mail to guest
                await UserManager.SendEmailAsync(UserId, "Резервирование успешно создано", "Мы отправили Хотельеру информацию о вашем заселении.<br /> Вы также можете связаться с хозяином самостоятельно. Телефон +79277607222");
                // Send mail to hotelier
                await UserManager.SendEmailAsync(accommodation.UserId, "Новое резервирование", "На вашу квартиру поступило новое резервирование!");
            }
            catch (Exception exx)
            {
                ex = exx;
            }

            if (ex != null)
            {
                // Cannot await in catch block
                await Logger.LogException(ex);
            }

            return Ok();
        }

        [HttpPut]
        [Route("reservationList")]
        public async Task<IHttpActionResult> GetReservationList([FromBody]GetReservationListCriteriaModel criteria)
        {
            var listDto = await ReservationRepository.GetList(criteria.AccommodationId.Value, criteria.DateArrivalStart);
            var list = listDto.Select(p => Mapper.Map<Short_ReservationModel>(p));

            return Ok(list);
        }

        // cancelReservation
        [HttpPut]
        [Route("cancelReservation/{id:int}")]
        public async Task<IHttpActionResult> CancelReservation(int id)
        {
            await ReservationRepository.Cancel(id, true);

            return Ok();
        }

    }
}