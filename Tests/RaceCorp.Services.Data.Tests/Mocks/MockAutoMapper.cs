namespace RaceCorp.Services.Data.Tests.Mocks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using AutoMapper;
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Mapping;
    using RaceCorp.Web.ViewModels;
    using RaceCorp.Web.ViewModels.Request;
    using RaceCorp.Web.ViewModels.User;

    public static class MockAutoMapper
    {
        public static void InitializeAutoMapper()
        {
            // Because all tests use one instance of mapper there will be no need to instance it every time.
            // For now, this is the solution I found. Maybe it has a better way!.
            if (AutoMapperConfig.MapperInstance != null)
            {
                return;
            }

            AutoMapperConfig.MapperInstance = RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);
        }

        private static Mapper RegisterMappings(params Assembly[] assemblies)
        {
            var types = assemblies.SelectMany(a => a.GetExportedTypes()).ToList();

            var config = new MapperConfigurationExpression();
            config.CreateProfile(
                "ReflectionProfile",
                configuration =>
                {
                    configuration.CreateMap<ApplicationUser, ApplicationUser>();
                    configuration.CreateMap<ApplicationUser, UserAllViewModel>();
                    configuration.CreateMap<UserProfileViewModel, UserProfileViewModel>();
                    configuration.CreateMap<Request, RequestModel>();
                    configuration.CreateMap<Connection, UserConnectionsViewModel>();
                    configuration.CreateMap<Ride, CreatedRideBaseModel>();
                    configuration.CreateMap<Race, CreatedRaceBaseModel>();
                    configuration.CreateMap<ApplicationUserRide, UserRideBaseModel>();
                    configuration.CreateMap<ApplicationUserTrace, UserTraceBaseModel>();
                    //configuration.CreateMap<>();

                    // IHaveCustomMappings
                    // Some of the tests have used models with custom mappings.
                    foreach (var map in GetCustomMappings(types))
                    {
                        map.CreateMappings(configuration);
                    }
                });
            return new Mapper(new MapperConfiguration(config));
        }

        private static IEnumerable<IHaveCustomMappings> GetCustomMappings(IEnumerable<Type> types)
        {
            var customMaps = from t in types
                             from i in t.GetTypeInfo().GetInterfaces()
                             where typeof(IHaveCustomMappings).GetTypeInfo().IsAssignableFrom(t) &&
                                   !t.GetTypeInfo().IsAbstract &&
                                   !t.GetTypeInfo().IsInterface
                             select (IHaveCustomMappings)Activator.CreateInstance(t);

            return customMaps;
        }
    }
}
