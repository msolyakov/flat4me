[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Flat4Me.Web.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(Flat4Me.Web.App_Start.NinjectWebCommon), "Stop")]

namespace Flat4Me.Web.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using Flat4Me.Data.Repository.Interfaces;
    using Flat4Me.Data.Repository.MsSql;
    using Flat4Me.Data.Repository.Interfaces.Short;
    using Flat4Me.Data.Repository.MsSql.Short;
    using Flat4Me.Data.Repository.Azure;
    using Flat4Me.Data.Repository.Caching;
    using Flat4Me.Core.Caching;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<ILogRepository>().To<StorageLogRepository>();
            kernel.Bind<ICacheManager>().To<MemoryCacheManager>();

            kernel.Bind<IAccommodationRepository>().To<AccommodationRepository>();
            kernel.Bind<IPhotoRepository>().To<PhotoRepository>();
            kernel.Bind<IMapRepository>().To<MapRepository>();
            kernel.Bind<IReservationRepository>().To<ReservationRepository>();            
            kernel.Bind<ICityRepository>().To<CityRepository>();
            kernel.Bind<IHotelierProfileRepository>().To<HotelierProfileRepository>();
            kernel.Bind<ISearchRepository>().To<SearchRepository>();
        }        
    }
}
