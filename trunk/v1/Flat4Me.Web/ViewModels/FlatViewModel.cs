using Flat4Me.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flat4Me.Web.ViewModels
{
    public class FlatViewModel
    {
        #region Private

        private List<PhotoModel> _photoList;
        private List<PriceModel> _priceList;

        #endregion

        public FlatModel Flat { get; set; }

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

        public HotelierProfileModel HotelierProfile { get; set; }

        public Short_ReservationModel CurrentReservation { get; set; }

        public HotelierContactsModel HotelierContacts { get; set; }

        public bool HasReservation { get { return CurrentReservation != null; } }
    }
}