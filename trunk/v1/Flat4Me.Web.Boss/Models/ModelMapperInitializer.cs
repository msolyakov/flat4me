using AutoMapper;
using Flat4Me.Data.DTO.Short;
using Flat4Me.Web.Boss.Models.Short;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flat4Me.Web.Boss.Models
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

            // HotelierProfileUnapproved
            Mapper.CreateMap<HotelierProfileUnapprovedDTO, HotelierProfileUnapprovedViewModel>();
            Mapper.CreateMap<HotelierProfileUnapprovedViewModel, HotelierProfileUnapprovedDTO>();
        }
    }
}