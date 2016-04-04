using Flat4Me.Data.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Flat4Me.Web.Models
{
    public class FlatModel
    {
        #region Db properties

        public int? AccommodationId { get; set; }
        public int UserId { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsPublished { get; set; }
        public bool IsApproved { get; set; }
        public string Name { get; set; }
        public int? CityId { get; set; }
        public string CityName { get; set; }
        public short? Area { get; set; }
        public string StreetName { get; set; }
        public string HouseNumber { get; set; }
        public byte? RoomCount { get; set; }
        public byte? BedroomCount { get; set; }
        public byte? BathroomCount { get; set; }
        public byte? LoungeRoomCount { get; set; }
        public string AccommodationTypeCode { get; set; }
        public string CurrencyCode { get; set; }
        public byte? MinDurationDays { get; set; }
        public string SleepsCount { get; set; }
        public byte MaxGuestsCount { get; set; }
        public bool HasAirConditioning { get; set; }
        public bool HasWashingMachine { get; set; }
        public bool HasRefrigerator { get; set; }
        public bool HasKitchen { get; set; }
        public bool HasElevator { get; set; }
        public bool HasParking { get; set; }
        public bool HasJacuzzi { get; set; }
        public bool HasInternet { get; set; }
        public bool HasWiFi { get; set; }
        public bool HasGasWaterHeater { get; set; }
        public bool HasElectricWaterHeater { get; set; }
        public bool HasDish { get; set; }
        public bool HasDishwasher { get; set; }
        public bool HasMicrowave { get; set; }
        public bool HasElectricKettle { get; set; }
        public bool HasHairDryer { get; set; }
        public bool HasIron { get; set; }
        public bool HasIntercom { get; set; }
        public bool HasConcierge { get; set; }
        public bool HasSecurity { get; set; }
        public bool HasToiletries { get; set; }
        public bool HasTV { get; set; }
        public bool HasTVCable { get; set; }
        public bool IsSmokingAllowed { get; set; }
        public bool IsAnimalsAllowed { get; set; }
        public bool IsPrivateAllowed { get; set; }
        public bool IsPhotoSessionAllowed { get; set; }
        public bool IsHypoallergenic { get; set; }
        public bool HasDeposit { get; set; }
        public bool IsDepositWhenReservation { get; set; }
        public Nullable<int> Deposit { get; set; }
        public string Furniture { get; set; }
        public string Infrastructure { get; set; }

        #endregion

        #region View properties

        private List<PhotoModel> _photoList;
        public List<PhotoModel> PhotoList
        {
            get
            {
                return _photoList;
            }
            set
            {
                _photoList = value;

                if (_photoList == null)
                    return;

                var ix = 1;// Must start from 1. 0 - reserverd for Map
                _photoList = _photoList
                    .OrderByDescending(p => p.IsPrimary)
                    .ThenBy(p => p.PhotoId)
                    .ToList();

                _photoList.ForEach(p => p.Index = ix++);
            }
        }

        private List<PriceModel> _priceList;
        public List<PriceModel> PriceList
        {
            get
            {
                return _priceList;
            }
            set
            {
                _priceList = value;

                if (_priceList != null)
                {
                    // Make Index in order by duration 1, 2, 3
                    // Should start from 0.
                    var ix = 0;
                    _priceList = _priceList.OrderBy(p => p.DurationDays).ToList();
                    _priceList.ForEach(p => p.ViewIndex = ix++);
                    PriceMin = _priceList.LastOrDefault();
                }
            }
        }

        public PriceModel PriceMin { get; set; }

        public ViewLocationModel Location { get; set; }

        #endregion
    }
}