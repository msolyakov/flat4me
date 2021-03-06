﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flat4Me.Data.DTO;
using Flat4Me.Data.DTO.Short;
using Flat4Me.Data.DTO.Auth;

namespace Flat4Me.Web.Models
{
    /// <summary>
    /// AutoMapper initializer for view model items
    /// </summary>
    public class ModelMapperInitializer
    {
        /// <summary>
        /// Initialize mapping between DTO and Web View Model objects
        /// </summary>
        public static void Init()
        {
            // MAP TEMPLATE: <FROM, TO>

            // AccommodationShort
            Mapper.CreateMap<AccommodationShortDTO, FlatModel>();
            Mapper.CreateMap<FlatModel, AccommodationShortDTO>();

            // Photo
            Mapper.CreateMap<PhotoDTO, PhotoModel>();
            Mapper.CreateMap<PhotoModel, PhotoDTO>();

            // Price
            Mapper.CreateMap<PriceDTO, PriceModel>();
            Mapper.CreateMap<PriceModel, PriceDTO>();

            // Location
            Mapper.CreateMap<AccommodationLocationDTO, ViewLocationModel>();

            // Reservation
            Mapper.CreateMap<Short_ReservationModel, ReservationDTO>();
            Mapper.CreateMap<ReservationDTO, Short_ReservationModel>();

            // HotelierProfile
            Mapper.CreateMap<HotelierProfileDTO, HotelierProfileModel>();

            // HotelierContacts
            Mapper.CreateMap<UserDTO, HotelierContactsModel>();
        }
    }
}