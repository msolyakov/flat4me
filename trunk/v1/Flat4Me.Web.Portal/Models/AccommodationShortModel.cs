using Flat4Me.Data.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Flat4Me.Web.Portal.Models
{
    public class AccommodationShortModel : IValidatableObject
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
        public byte MinDurationDays { get; set; }
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
                    _priceList.OrderBy(p => p.DurationDays).ToList().ForEach(p => p.ViewIndex = ix++);
                }
            }
        }

        public ViewLocationModel Location { get; set; }

        #endregion

        #region Validation

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var result = new List<ValidationResult>();

            #region Validate simple required fields

            // Accommodation is new
            if (AccommodationId == null)
            {
                // Only when accommodation is new should check this fields.
                // We do not allow change this fields while editing
                if (string.IsNullOrEmpty(Name))
                    result.Add(new ValidationResult("Обязательно заполните", new[] { "Name" }));

                if (string.IsNullOrEmpty(StreetName))
                    result.Add(new ValidationResult("Обязательно заполните", new[] { "StreetName" }));

                if (string.IsNullOrEmpty(HouseNumber))
                    result.Add(new ValidationResult("Обязательно заполните", new[] { "HouseNumber" }));

                if (CityId == null)
                    result.Add(new ValidationResult("Обязательно заполните", new[] { "CityId" }));

                if (string.IsNullOrEmpty(AccommodationTypeCode))
                    result.Add(new ValidationResult("Обязательно заполните", new[] { "AccommodationTypeCode" }));
            }


            if (string.IsNullOrEmpty(CurrencyCode))
                result.Add(new ValidationResult("Обязательно заполните", new[] { "CurrencyCode" }));

            if (string.IsNullOrEmpty(SleepsCount))
                result.Add(new ValidationResult("Обязательно заполните", new[] { "SleepsCount" }));

            if (string.IsNullOrEmpty(Infrastructure))
                result.Add(new ValidationResult("Обязательно заполните", new[] { "Infrastructure" }));

            if (string.IsNullOrEmpty(Furniture))
                result.Add(new ValidationResult("Обязательно заполните", new[] { "Furniture" }));

            if (Area == null)
                result.Add(new ValidationResult("Обязательно заполните", new[] { "Area" }));



            if (RoomCount == null)
                result.Add(new ValidationResult("Обязательно заполните", new[] { "RoomCount" }));

            if (MinDurationDays == null)
                result.Add(new ValidationResult("Обязательно заполните", new[] { "MinDurationDays" }));

            if (HasDeposit && Deposit == null)
                result.Add(new ValidationResult("Обязательно заполните", new[] { "Deposit" }));

            #endregion

            ValidatePriceList(result);

            return result;
        }
        private void ValidatePriceList(List<ValidationResult> result)
        {
            if (PriceList == null || PriceList.Count == 0)
            {
                result.Add(new ValidationResult("Обязательно заполните", new[] { "PriceList" }));
                return;
            }

            for (int i = 0; i < PriceList.Count; i++)
            {
                var price = PriceList[i];
                var durationFieldName = string.Format("PriceList[{0}].DurationDays", i);
                var amountFieldName = string.Format("PriceList[{0}].Amount", i);

                if (!price.DurationDays.HasValue)
                {
                    result.Add(new ValidationResult("Обязательно заполните", new[] { durationFieldName }));
                }
                else
                {
                    if (price.DurationDays.Value <= 0)
                        result.Add(new ValidationResult("Должно быть больше 0", new[] { durationFieldName }));
                }

                if (!price.Amount.HasValue)
                {
                    result.Add(new ValidationResult("Обязательно заполните", new[] { amountFieldName }));
                }
                else
                {
                    var amount = price.Amount.Value;
                    if (amount <= 0)
                        result.Add(new ValidationResult("Должно быть больше 0", new[] { amountFieldName }));
                }
            }

            var priceListHasError = result.Any(p => p.MemberNames.Any(pp => pp.StartsWith("PriceList")));
            if (!priceListHasError)
            {
                // Business checking starts here

                // Two or more price item has same duration
                var hasSameDuration = PriceList.GroupBy(p => p.DurationDays).Any(p => p.Count() > 1);
                if (hasSameDuration)
                    result.Add(new ValidationResult("Длительность проживания не должна иметь одинаковые значения", new[] { "PriceList" }));

                // Two or more price item has same amount
                var hasSameAmount = PriceList.GroupBy(p => p.Amount).Any(p => p.Count() > 1);
                if (hasSameAmount)
                    result.Add(new ValidationResult("Стоимость суток не должна иметь одинаковые значения", new[] { "PriceList" }));

                var minPriceListDuration = PriceList.Min(p => p.DurationDays);
                if (minPriceListDuration < MinDurationDays)
                {
                    result.Add(new ValidationResult("Минимальный срок проживания не соответствует", new[] { "PriceList" }));
                    result.Add(new ValidationResult("Минимальный срок проживания не соответствует", new[] { "MinDurationDays" }));
                }
            }
        }

        #endregion
    }
}