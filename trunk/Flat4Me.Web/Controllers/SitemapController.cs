using Flat4Me.Core;
using Flat4Me.Core.Caching;
using Flat4Me.Data.DTO;
using Flat4Me.Data.Repository.Interfaces;
using Flat4Me.Web.ActionResults;
using Flat4Me.Web.Extensions;
using Flat4Me.Web.SEO;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Flat4Me.Web.Controllers
{
    [AllowAnonymous]
    public class SitemapController : Controller
    {
        /// <summary>
        /// Ключ кеша для хранения списка SitemapItem
        /// </summary>
        public const string SITEMAP_ITEMS_KEY = "f4me.sitemap.items";
        
        [Inject]
        public ICacheManager CacheManager { get; set; }
        [Inject]
        public IAccommodationRepository AccommodationRepository { get; set; }
        [Inject]
        public ICityRepository CityRepository { get; set; }

        // GET: Sitemap
        public async Task<ActionResult> Index()
        {
            // Кешируем список SitemapItems на 6 часов. 
            List<SitemapItem> sitemapItems = await CacheManager.Get(SITEMAP_ITEMS_KEY, 360, async () =>
            {
                List<SitemapItem> result = new List<SitemapItem>();
                await AddSitemapItems(Request.Url.GetLeftPart(UriPartial.Authority), DateTime.Now, result);
                return result;
            });

            return new SitemapResult(sitemapItems);
        }

        /// <summary>
        /// Заполняет список SitemapItem
        /// </summary>
        private async Task AddSitemapItems(string basePath, DateTime changeDate, List<SitemapItem> items)
        {
            if (items == null)
                return;
            
            // Main page
            items.Add(new SitemapItem(basePath));
            
            // Cities and flats
            IEnumerable<CityDTO> cities = await CityRepository.GetAll();
            foreach (CityDTO c in cities)
            {
                string actionPath = RouteConfig.URL_PATTERN_CITY.Inject( 
                    new { city = c.Url } 
                );
                items.Add(new SitemapItem(PathUtils.CombinePaths(basePath, actionPath), changeDate));

                await AddSitemapItems(c, basePath, changeDate, items);
            }
        }

        /// <summary>
        /// Заполняет список SitemapItem значениями для конкретного города
        /// </summary>
        private async Task AddSitemapItems(CityDTO city, string basePath, DateTime changeDate, List<SitemapItem> items)
        {
            if (items == null)
                return;
            
            List<AccommodationShortMainDTO> flats = await AccommodationRepository.MainList(city.CityId);
            foreach (var f in flats)
            {
                string actionPath = RouteConfig.URL_PATTERN_FLAT.Inject(
                    new { city = city.Url, id = f.AccommodationId }
                );
                items.Add(new SitemapItem(PathUtils.CombinePaths(basePath, actionPath), changeDate));
            }
        }
    }
}
