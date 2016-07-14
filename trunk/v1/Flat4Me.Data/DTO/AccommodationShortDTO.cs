using Flat4Me.Core.Consts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flat4Me.Data.DTO
{
    public class AccommodationShortDTO
    {
        public int AccommodationId { get; set; }
        public int UserId { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsPublished { get; set; }
        public bool IsApproved { get; set; }
        public string Name { get; set; }

        public int CityId { get; set; }
        public string CityName { get; set; }

        public short Area { get; set; }
        public string StreetName { get; set; }
        public string HouseNumber { get; set; }

        public byte RoomCount { get; set; }
        public byte BedroomCount { get; set; }
        public byte BathroomCount { get; set; }
        public byte LoungeRoomCount { get; set; }

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


        public bool BaseProperiesHasChanged(AccommodationShortDTO compare)
        {
            var isSame = Area == compare.Area
                && RoomCount == compare.RoomCount
                && BedroomCount == compare.BedroomCount
                && BathroomCount == compare.BathroomCount
                && LoungeRoomCount == compare.LoungeRoomCount;

            return isSame == false;
        }
        public bool ShortProperiesHasChanged(AccommodationShortDTO compare)
        {
            var isSame = CurrencyCode == compare.CurrencyCode
                && MinDurationDays == compare.MinDurationDays
                && SleepsCount == compare.SleepsCount
                && MaxGuestsCount == compare.MaxGuestsCount
                && LoungeRoomCount == compare.LoungeRoomCount
                && HasAirConditioning == compare.HasAirConditioning
                && HasWashingMachine == compare.HasWashingMachine
                && HasRefrigerator == compare.HasRefrigerator
                && HasKitchen == compare.HasKitchen
                && HasElevator == compare.HasElevator
                && HasParking == compare.HasParking
                && HasJacuzzi == compare.HasJacuzzi
                && HasInternet == compare.HasInternet
                && HasWiFi == compare.HasWiFi
                && HasGasWaterHeater == compare.HasGasWaterHeater
                && HasElectricWaterHeater == compare.HasElectricWaterHeater
                && HasDish == compare.HasDish
                && HasDishwasher == compare.HasDishwasher
                && HasMicrowave == compare.HasMicrowave
                && HasElectricKettle == compare.HasElectricKettle
                && HasHairDryer == compare.HasHairDryer
                && HasIron == compare.HasIron
                && HasIntercom == compare.HasIntercom
                && HasConcierge == compare.HasConcierge
                && HasSecurity == compare.HasSecurity
                && HasToiletries == compare.HasToiletries
                && HasTV == compare.HasTV
                && HasTVCable == compare.HasTVCable
                && HasDeposit == compare.HasDeposit
                && IsSmokingAllowed == compare.IsSmokingAllowed
                && IsAnimalsAllowed == compare.IsAnimalsAllowed
                && IsPrivateAllowed == compare.IsPrivateAllowed
                && IsPhotoSessionAllowed == compare.IsPhotoSessionAllowed
                && IsHypoallergenic == compare.IsHypoallergenic
                && IsDepositWhenReservation == compare.IsDepositWhenReservation
                && Deposit == compare.Deposit
                && Furniture == compare.Furniture
                && Infrastructure == compare.Infrastructure;

            return isSame == false;
        }
        public bool Equals(AccommodationShortDTO p)
        {
            if (p == null)
                return false;

            return AccommodationId == p.AccommodationId
                && UserId == p.UserId
                && IsDeleted == p.IsDeleted
                && IsPublished == p.IsPublished
                && IsApproved == p.IsApproved
                //&& Name == p.Name doesn't changes by logic
                && CityId == p.CityId
                && Area == p.Area
                //&& StreetName == p.StreetName doesn't changes by logic
                //&& HouseNumber == p.HouseNumber doesn't changes by logic
                && RoomCount == p.RoomCount
                && BedroomCount == p.BedroomCount
                && BathroomCount == p.BathroomCount
                && LoungeRoomCount == p.LoungeRoomCount
                && AccommodationTypeCode == p.AccommodationTypeCode
                && CurrencyCode == p.CurrencyCode
                && MinDurationDays == p.MinDurationDays
                && SleepsCount == p.SleepsCount
                && MaxGuestsCount == p.MaxGuestsCount
                && HasAirConditioning == p.HasAirConditioning
                && HasWashingMachine == p.HasWashingMachine
                && HasRefrigerator == p.HasRefrigerator
                && HasKitchen == p.HasKitchen
                && HasElevator == p.HasElevator
                && HasParking == p.HasParking
                && HasJacuzzi == p.HasJacuzzi
                && HasInternet == p.HasInternet
                && HasWiFi == p.HasWiFi
                && HasGasWaterHeater == p.HasGasWaterHeater
                && HasElectricWaterHeater == p.HasElectricWaterHeater
                && HasDish == p.HasDish
                && HasDishwasher == p.HasDishwasher
                && HasMicrowave == p.HasMicrowave
                && HasElectricKettle == p.HasElectricKettle
                && HasHairDryer == p.HasHairDryer
                && HasIron == p.HasIron
                && HasIntercom == p.HasIntercom
                && HasConcierge == p.HasConcierge
                && HasSecurity == p.HasSecurity
                && HasToiletries == p.HasToiletries
                && HasTV == p.HasTV
                && HasTVCable == p.HasTVCable
                && IsSmokingAllowed == p.IsSmokingAllowed
                && IsAnimalsAllowed == p.IsAnimalsAllowed
                && IsPrivateAllowed == p.IsPrivateAllowed
                && IsPhotoSessionAllowed == p.IsPhotoSessionAllowed
                && IsHypoallergenic == p.IsHypoallergenic
                && HasDeposit == p.HasDeposit
                && IsDepositWhenReservation == p.IsDepositWhenReservation
                && Deposit == p.Deposit
                && Furniture == p.Furniture
                && Infrastructure == p.Infrastructure;
        }

        public static AccommodationShortDTO Generate_1(int accommodationId, int cityId, int userId)
        {
            return new AccommodationShortDTO
            {
                AccommodationId = accommodationId,
                IsApproved = true,
                IsPublished = true,
                AccommodationTypeCode = AccommodationTypeList.Apartment,
                CurrencyCode = CurrencyList.Rub,
                Area = 33,
                BathroomCount = 2,
                BedroomCount = 2,
                CityId = cityId,
                Deposit = 2000,
                Furniture = "Furniture",
                HasAirConditioning = true,
                HasConcierge = true,
                HasDeposit = true,
                HasDish = true,
                HasDishwasher = true,
                HasElectricKettle = true,
                HasElectricWaterHeater = true,
                HasElevator = true,
                HasGasWaterHeater = true,
                HasHairDryer = true,
                HasIntercom = true,
                HasInternet = true,
                HasIron = true,
                HasJacuzzi = true,
                HasKitchen = true,
                HasMicrowave = true,
                HasParking = true,
                HasRefrigerator = true,
                HasSecurity = true,
                HasToiletries = true,
                HasTV = true,
                HasTVCable = true,
                HasWashingMachine = true,
                HasWiFi = true,
                Infrastructure = "Infrastructure",
                IsAnimalsAllowed = true,
                IsDepositWhenReservation = true,
                IsHypoallergenic = true,
                IsPhotoSessionAllowed = true,
                IsPrivateAllowed = true,
                IsSmokingAllowed = true,
                LoungeRoomCount = 2,
                MaxGuestsCount = 2,
                MinDurationDays = 1,
                Name = "Name",
                RoomCount = 3,
                SleepsCount = "2+1",
                StreetName = "Чернореченская",
                HouseNumber = "13",
                UserId = userId
            };
        }
        public static AccommodationShortDTO Generate_2(int accommodationId, int cityId, int userId)
        {
            return new AccommodationShortDTO
            {
                AccommodationId = accommodationId,
                IsApproved = true,
                IsPublished = true,
                AccommodationTypeCode = AccommodationTypeList.Apartment,
                CurrencyCode = CurrencyList.Rub,
                Area = 23,
                BathroomCount = 1,
                BedroomCount = 1,
                CityId = cityId,
                Deposit = 1000,
                Furniture = "ture",
                HasAirConditioning = false,
                HasConcierge = false,
                HasDeposit = false,
                HasDish = false,
                HasDishwasher = false,
                HasElectricKettle = false,
                HasElectricWaterHeater = false,
                HasElevator = false,
                HasGasWaterHeater = false,
                HasHairDryer = false,
                HasIntercom = false,
                HasInternet = false,
                HasIron = false,
                HasJacuzzi = false,
                HasKitchen = false,
                HasMicrowave = false,
                HasParking = false,
                HasRefrigerator = false,
                HasSecurity = false,
                HasToiletries = false,
                HasTV = false,
                HasTVCable = false,
                HasWashingMachine = false,
                HasWiFi = false,
                Infrastructure = "ucture",
                IsAnimalsAllowed = false,
                IsDepositWhenReservation = false,
                IsHypoallergenic = false,
                IsPhotoSessionAllowed = false,
                IsPrivateAllowed = false,
                IsSmokingAllowed = false,
                LoungeRoomCount = 1,
                MaxGuestsCount = 1,
                MinDurationDays = 2,
                Name = "Eman",
                RoomCount = 2,
                SleepsCount = "1+1",
                StreetName = "Осипенко",
                HouseNumber = "138",
                UserId = userId
            };
        }
    }
}
