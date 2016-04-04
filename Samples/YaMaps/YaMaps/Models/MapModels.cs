using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace YaMaps.Models
{
    [Table("AccommodationShortLocation")]
    public class AccommodationShortLocation
    {
        [Key] // Identity
        public int LocationId { get; set; }
        public int AccommodationId { get; set; }
        public string FullAddress { get; set; }
        public double PointY { get; set; } // Широта
        public double PointX { get; set; } // Долгота
        public bool IsConfirmed { get; set; }
        public bool IsDeleted { get; set; }
    }

    /// <summary>
    /// Справочник мест, рядом с которыми планируется пребывание.
    /// Например - Ж/д вокзал, Клиника Бранчевского, Самарский Мед.институт.
    /// </summary>
    [Table("CityLandmark")]
    public class CityLandmark
    {
        [Key]
        public int LandmarkId { get; set; }
        public int CityId { get; set; }
        public string ShortName { get; set; }
        public string FullName { get; set; }
        public string FullAddress { get; set; }
        public double PointY { get; set; } // Широта
        public double PointX { get; set; } // Долгота
    }

    /// <summary>
    /// Расстояние от квартиры (посуточная аренда) до места пребывания.
    /// </summary>
    /// <remarks>
    /// Хранится код удаленности, примерно соответствующий следующим значениям:
    /// 0 - 8 мин. пешком; 
    /// 1 - 15 мин. пешком; 
    /// 2 - 20 мин. на транспорте и пешком.
    /// </remarks>
    [Table("AccommodationShortDistance")]
    public class AccommodationShortDistance
    {
        [Key]
        [Column(Order = 1)]
        public int LocationId { get; set; }
        [Key]
        [Column(Order = 2)]
        public int LandmarkId { get; set; }
        public uint DistanceCode { get; set; } // 0 - ~8 мин. пешком; 1 - ~15 мин. пешком; 2 - ~20 мин. на транспорте и пешком.
        public long Distance { get; set; } // Расстояние по прямой в метрах
    }

    /// <summary>
    /// Требуется для определения значений DistanceCode в таблице 
    /// AccommodationShortDistance, в зависимости от города.
    /// </summary>
    /// <remarks>
    /// Для Самары три градации - 500, 1000 и 2000 м.
    /// </remarks>
    [Table("CityDistanceCode")]
    public class CityDistanceCode
    {
        [Key]
        [Column(Order = 1)]
        public int CityId { get; set; }
        [Key]
        [Column(Order = 2)]
        public uint DistanceCode { get; set; } // 0 - ~8 мин. пешком; 1 - ~15 мин. пешком; 2 - ~20 мин. на транспорте и пешком.
        public long Distance { get; set; } // Расстояние по прямой в метрах. 
    }

    public class MapDbContext : DbContext
    {
        public DbSet<CityLandmark> CityLandmarkList { get; set; }
        public DbSet<AccommodationShortLocation> AccommodationShortLocationList { get; set; }
        public DbSet<AccommodationShortDistance> AccommodationShortDistanceList { get; set; }
    }
}