﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YaMaps.Data.Classes
{
    public class GeocoderResponseMetaData
    {
        public string request { get; set; }
        public string found { get; set; }
        public string results { get; set; }
    }

    public class MetaDataProperty
    {
        public GeocoderResponseMetaData GeocoderResponseMetaData { get; set; }
    }

    public class Premise
    {
        public string PremiseNumber { get; set; }
    }

    public class Thoroughfare
    {
        public string ThoroughfareName { get; set; }
        public Premise Premise { get; set; }
    }

    public class Thoroughfare2
    {
        public string ThoroughfareName { get; set; }
    }

    public class DependentLocality
    {
        public string DependentLocalityName { get; set; }
        public Thoroughfare2 Thoroughfare { get; set; }
    }

    public class Locality
    {
        public string LocalityName { get; set; }
        public Thoroughfare Thoroughfare { get; set; }
        public DependentLocality DependentLocality { get; set; }
    }

    public class SubAdministrativeArea
    {
        public string SubAdministrativeAreaName { get; set; }
        public Locality Locality { get; set; }
    }

    public class AdministrativeArea
    {
        public string AdministrativeAreaName { get; set; }
        public SubAdministrativeArea SubAdministrativeArea { get; set; }
    }

    public class Country
    {
        public string AddressLine { get; set; }
        public string CountryNameCode { get; set; }
        public string CountryName { get; set; }
        public AdministrativeArea AdministrativeArea { get; set; }
    }

    public class AddressDetails
    {
        public Country Country { get; set; }
    }

    public class GeocoderMetaData
    {
        public string kind { get; set; }
        public string text { get; set; }
        public string precision { get; set; }
        public AddressDetails AddressDetails { get; set; }
    }

    public class MetaDataProperty2
    {
        public GeocoderMetaData GeocoderMetaData { get; set; }
    }

    public class Envelope
    {
        public string lowerCorner { get; set; }
        public string upperCorner { get; set; }
    }

    public class BoundedBy
    {
        public Envelope Envelope { get; set; }
    }

    public class Point
    {
        public string pos { get; set; }
    }

    public class GeoObject
    {
        public MetaDataProperty2 metaDataProperty { get; set; }
        public string description { get; set; }
        public string name { get; set; }
        public BoundedBy boundedBy { get; set; }
        public Point Point { get; set; }
    }

    public class FeatureMember
    {
        public GeoObject GeoObject { get; set; }
    }

    public class GeoObjectCollection
    {
        public MetaDataProperty metaDataProperty { get; set; }
        public List<FeatureMember> featureMember { get; set; }
    }

    public class Response
    {
        public GeoObjectCollection GeoObjectCollection { get; set; }
    }

    public class RootObject
    {
        public Response response { get; set; }
    }
}