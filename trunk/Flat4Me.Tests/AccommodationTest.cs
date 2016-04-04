using Flat4Me.Core.Consts;
using Flat4Me.Data.DTO;
using Flat4Me.Data.DTO.Short;
using Flat4Me.Data.Repository.Interfaces;
using Flat4Me.Data.Repository.Interfaces.Short;
using Flat4Me.Data.Repository.MsSql;
using Flat4Me.Data.Repository.MsSql.Short;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flat4Me.Tests
{
    [TestClass]
    public sealed class AccommodationTest
    {
        // Set in Init
        public static int? AccommodationId { get; set; }
        public static int UserId { get { return 1; } }
        public static int CityId { get { return 1; } }

        public static byte Price_First_DurationDays = 1;
        public static int Price_First_Amount = 2000;

        public static byte Price_Second_DurationDays = 2;
        public static int Price_Second_Amount = 3000;

        public static IAccommodationRepository AccommodationRepository { get { return new AccommodationRepository(); } }
        public static ICityRepository CityRepository { get { return new CityRepository(); } }
        public static IPhotoRepository PhotoRepository { get { return new PhotoRepository(); } }
        public static IMapRepository MapRepository { get { return new MapRepository(); } }
        public static IReservationRepository ReservationRepository { get { return new ReservationRepository(); } }

        [ClassInitialize]
        public static void Init(TestContext context)
        {
            var draft = AccommodationRepository.AddDraft(UserId).Result;
            // Generate short accommodation data
            var accommodationShort = AccommodationShortDTO.Generate_1(draft.AccommodationId, CityId, UserId);
            // add price
            var priceList = new List<PriceDTO>();
            priceList.Add(new PriceDTO { DurationDays = 1, Amount = Price_First_Amount });
            AccommodationRepository.CompleteDraft(accommodationShort, priceList).Wait();// Complete draft
            AccommodationId = draft.AccommodationId;// Set accommodation id for test method
            // add fake photo
            var photoList = new List<PhotoDTO>();
            var photo = new PhotoDTO { IsPrimary = true, IsApproved = true, TinyPath = "TinyPath", SmallPath = "SmallPath", MediumPath = "MediumPath", LargePath = "LargePath" };
            photoList.Add(photo);
            PhotoRepository.Add(AccommodationId.Value, photoList).Wait();
            // add fake location
            MapRepository.AddLocation(new AccommodationLocationDTO
            {
                AccommodationId = AccommodationId.Value,
                FullAddress = string.Format("{0}, {1}", accommodationShort.StreetName, accommodationShort.HouseNumber),
                PointY = 0,
                PointX = 0
            }).Wait();
            var location = MapRepository.GetLocationList(AccommodationId.Value).Result.FirstOrDefault();
            MapRepository.ConfirmLocation(location.LocationId).Wait();
        }

        [ClassCleanup]
        public static void Clean()
        {
            AccommodationRepository.DeleteCompletly(AccommodationId.Value).Wait();
        }


        [TestMethod]
        public void GetAccommodation()
        {
            Assert.IsNotNull(AccommodationId);

            var accommodation = AccommodationShortDTO.Generate_1(AccommodationId.Value, CityId, UserId);
            var accommodationDb = AccommodationRepository.Get(AccommodationId.Value).Result;

            Assert.IsNotNull(accommodationDb);
            var isEqual = accommodation.Equals(accommodationDb);
            Assert.IsTrue(isEqual);
        }

        [TestMethod]
        public void EditAccommodation()
        {
            // Update
            var accommodation = AccommodationShortDTO.Generate_2(AccommodationId.Value, CityId, UserId);

            // Save update
            AccommodationRepository.Update(accommodation).Wait();

            // Check after update
            var accommodationUpdated = AccommodationRepository.Get(AccommodationId.Value).Result;
            var isEqual = accommodationUpdated.Equals(accommodation);
            Assert.IsTrue(isEqual);

            // Restore
            accommodation = AccommodationShortDTO.Generate_1(AccommodationId.Value, CityId, UserId);
            AccommodationRepository.Update(accommodation).Wait();
        }


        [TestMethod]
        public void GetPriceList()
        {
            var priceList = AccommodationRepository.GetPriceList(AccommodationId.Value).Result;

            Assert.IsNotNull(priceList);
            Assert.IsTrue(priceList.Count > 0);

            var price = priceList.OrderBy(p => p.PriceId).First();

            Assert.IsTrue(price.DurationDays == Price_First_DurationDays);
            Assert.IsTrue(price.Amount == Price_First_Amount);
        }

        [TestMethod]
        public void EditPriceList()
        {
            var accommodation = AccommodationRepository.Get(AccommodationId.Value).Result;
            var priceList = AccommodationRepository.GetPriceList(AccommodationId.Value).Result;
            var price = priceList.OrderBy(p => p.PriceId).First();
            price.DurationDays = Price_Second_DurationDays;
            price.Amount = Price_Second_Amount;

            AccommodationRepository.Update(accommodation, null, priceList).Wait();

            priceList = AccommodationRepository.GetPriceList(AccommodationId.Value).Result;
            price = priceList.OrderBy(p => p.PriceId).First();

            Assert.IsTrue(price.DurationDays == Price_Second_DurationDays);
            Assert.IsTrue(price.Amount == Price_Second_Amount);

            // Restore
            price.DurationDays = Price_First_DurationDays;
            price.Amount = Price_First_Amount;
            AccommodationRepository.Update(accommodation, null, priceList).Wait();
        }

        [TestMethod]
        public void AddPriceList()
        {
            var accommodation = AccommodationRepository.Get(AccommodationId.Value).Result;
            var priceList = new List<PriceDTO>();
            var price = new PriceDTO
            {
                Amount = Price_Second_Amount,
                DurationDays = Price_Second_DurationDays
            };
            priceList.Add(price);

            AccommodationRepository.Update(accommodation, priceList, null).Wait();

            priceList = AccommodationRepository.GetPriceList(AccommodationId.Value).Result;
            price = priceList.OrderBy(p => p.PriceId).Last();

            Assert.IsTrue(price.DurationDays == Price_Second_DurationDays);
            Assert.IsTrue(price.Amount == Price_Second_Amount);
        }


        [TestMethod]
        public void GetMainListByCity()
        {
            var mainList = AccommodationRepository.MainList(CityId).Result;

            Assert.IsNotNull(mainList);
            Assert.IsTrue(mainList.Count > 0);
        }

        [TestMethod]
        public void GetMyList()
        {
            var mainList = AccommodationRepository.MyList(UserId).Result;

            Assert.IsNotNull(mainList);
            Assert.IsTrue(mainList.Count > 0);
        }


        [TestMethod]
        public void Publish()
        {
            AccommodationRepository.Publish(AccommodationId.Value, false).Wait();
            var accommodation = AccommodationRepository.Get(AccommodationId.Value).Result;
            Assert.IsNotNull(accommodation);
            Assert.IsFalse(accommodation.IsPublished);

            AccommodationRepository.Publish(AccommodationId.Value, true).Wait();
            accommodation = AccommodationRepository.Get(AccommodationId.Value).Result;
            Assert.IsNotNull(accommodation);
            Assert.IsTrue(accommodation.IsPublished);
        }

        [TestMethod]
        public void Approve()
        {
            AccommodationRepository.Approve(AccommodationId.Value, false).Wait();
            var accommodation = AccommodationRepository.Get(AccommodationId.Value).Result;
            Assert.IsNotNull(accommodation);
            Assert.IsFalse(accommodation.IsApproved);

            AccommodationRepository.Approve(AccommodationId.Value, true).Wait();
            accommodation = AccommodationRepository.Get(AccommodationId.Value).Result;
            Assert.IsNotNull(accommodation);
            Assert.IsTrue(accommodation.IsApproved);
        }

        [TestMethod]
        public void Delete()
        {
            AccommodationRepository.Delete(AccommodationId.Value, true).Wait();
            var accommodation = AccommodationRepository.Get(AccommodationId.Value).Result;
            Assert.IsNotNull(accommodation);
            Assert.IsTrue(accommodation.IsDeleted);

            AccommodationRepository.Delete(AccommodationId.Value, false).Wait();
            accommodation = AccommodationRepository.Get(AccommodationId.Value).Result;
            Assert.IsNotNull(accommodation);
            Assert.IsFalse(accommodation.IsDeleted);
        }

        [TestMethod]
        public void Reservation()
        {
            //// Add simple contact. Should be destroyed at the end
            //var contact = new ContactDTO
            //{
            //    Name = "Сильвестр Петрович",
            //    Phone = "9876543210",
            //    Email = "test@test.com",
            //    IsActivated = true,
            //    LastVisitOn = DateTime.UtcNow
            //};
            //ContactRepository.Add(contact).Wait();
            //// Fill reservation
            //var reservation = new ReservationDTO
            //{
            //    AccommodationId = AccommodationId.Value,
            //    ContactId = contact.ContactId,
            //    DateArrival = DateTime.Now.AddDays(1).Date,
            //    DateDeparture = DateTime.Now.AddDays(3).Date,
            //    GuestsCount = 2,
            //    ChildrenCount = 1,
            //    EstimatedAmount = 5000
            //};

            //try
            //{
            //    // Try add reservation
            //    ReservationRepository.Add(reservation).Wait();
            //    // Check reservation
            //    Assert.IsTrue(reservation.ReservationId > 0);

            //    // Try get reservation by accommodation
            //    var reservationList = ReservationRepository.GetList(AccommodationId.Value).Result;
            //    // Check it
            //    var dbReservation = reservationList.FirstOrDefault(p => p.ReservationId == reservation.ReservationId);
            //    Assert.IsNotNull(dbReservation);

            //    // Check all properties
            //    var allPropertiesIsSame = reservation.Compare(dbReservation);
            //    Assert.IsTrue(allPropertiesIsSame);

            //    // Try get reservation by DateArrival
            //    reservationList = ReservationRepository.GetList(AccommodationId.Value, reservation.DateArrival).Result;
            //    // Check it
            //    dbReservation = reservationList.FirstOrDefault(p => p.ReservationId == reservation.ReservationId);
            //    Assert.IsNotNull(dbReservation);

            //    // Try get reservation by DateArrival. Should be no result
            //    reservationList = ReservationRepository.GetList(AccommodationId.Value, reservation.DateArrival.AddDays(10)).Result;
            //    // Check it
            //    dbReservation = reservationList.FirstOrDefault(p => p.ReservationId == reservation.ReservationId);
            //    Assert.IsNull(dbReservation);

            //    // Cancel
            //    ReservationRepository.Cancel(reservation.ReservationId, true);
            //    // Check 
            //    dbReservation = ReservationRepository.GetList(AccommodationId.Value).Result.FirstOrDefault();
            //    Assert.IsTrue(dbReservation.IsCanceled);

            //    // Cancel back
            //    ReservationRepository.Cancel(reservation.ReservationId, false);
            //    // Check 
            //    dbReservation = ReservationRepository.GetList(AccommodationId.Value).Result.FirstOrDefault();
            //    Assert.IsFalse(dbReservation.IsCanceled);
            //}
            //finally
            //{
            //    if (reservation.ReservationId > 0)
            //        ReservationRepository.Destroy(reservation.ReservationId).Wait();

            //    ContactRepository.Destroy(contact.ContactId).Wait();
            //}
        }

    }
}
