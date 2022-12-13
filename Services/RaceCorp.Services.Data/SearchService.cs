namespace RaceCorp.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using RaceCorp.Common;
    using RaceCorp.Data.Common.Repositories;
    using RaceCorp.Data.Models;
    using RaceCorp.Data.Models.Enums;
    using RaceCorp.Services.Data.Contracts;
    using RaceCorp.Services.Mapping;
    using RaceCorp.Web.ViewModels.Search;

    public class SearchService : ISearchService
    {
        private readonly IDeletableEntityRepository<Town> townRepo;
        private readonly IDeletableEntityRepository<ApplicationUser> userRepo;
        private readonly IDeletableEntityRepository<Mountain> mountainRepo;
        private readonly IDeletableEntityRepository<Race> raceRepo;
        private readonly IDeletableEntityRepository<Ride> rideRepo;
        private readonly IDeletableEntityRepository<Team> teamRepo;

        public SearchService(
            IDeletableEntityRepository<Town> townRepo,
            IDeletableEntityRepository<ApplicationUser> userRepo,
            IDeletableEntityRepository<Mountain> mountainRepo,
            IDeletableEntityRepository<Race> raceRepo,
            IDeletableEntityRepository<Ride> rideRepo,
            IDeletableEntityRepository<Team> teamRepo)
        {
            this.townRepo = townRepo;
            this.userRepo = userRepo;
            this.mountainRepo = mountainRepo;
            this.raceRepo = raceRepo;
            this.rideRepo = rideRepo;
            this.teamRepo = teamRepo;
        }

        public List<T> GetTeams<T>(string query)
        {
            var querySplitted = query.Split(' ', ',', '.').Take(2).ToArray();

            if (querySplitted.Count() == 2)
            {
                return this.teamRepo
               .AllAsNoTracking()
               .Where(t =>
               t.Name.ToLower().Contains(querySplitted[0].ToLower()) ||
               t.Name.ToLower().Contains(querySplitted[1].ToLower())
              ).To<T>()
              .ToList();
            }

            return this.teamRepo
              .AllAsNoTracking()
              .Where(r =>
              r.Name.ToLower().Contains(querySplitted[0].ToLower())
             ).To<T>()
             .ToList();
        }

        public List<T> GetMountains<T>(string query)
        {
            var querySplitted = query.Split(' ', ',', '.').Take(2).ToArray();

            if (querySplitted.Count() == 2)
            {
                return this.mountainRepo
               .AllAsNoTracking()
               .Where(m =>
               m.Name.ToLower().Contains(querySplitted[0].ToLower()) ||
               m.Name.ToLower().Contains(querySplitted[1].ToLower())
              ).To<T>()
              .ToList();
            }

            return this.mountainRepo
              .AllAsNoTracking()
              .Where(m =>
              m.Name.ToLower().Contains(querySplitted[0].ToLower())
             ).To<T>()
             .ToList();
        }

        public List<T> GetRaces<T>(string query)
        {
            var querySplitted = query.Split(' ', ',', '.').Take(2).ToArray();

            if (querySplitted.Count() == 2)
            {
                return this.raceRepo
               .AllAsNoTracking()
               .Where(r =>
               r.Name.ToLower().Contains(querySplitted[0].ToLower()) ||
               r.Name.ToLower().Contains(querySplitted[1].ToLower())
              ).To<T>()
              .ToList();
            }

            return this.raceRepo
              .AllAsNoTracking()
              .Where(r =>
              r.Name.ToLower().Contains(querySplitted[0].ToLower())
             ).To<T>()
             .ToList();
        }

        public List<T> GetRides<T>(string query)
        {
            var querySplitted = query
                .Split(' ', ',', '.')
                .Take(2)
                .ToArray();

            if (querySplitted.Count() == 2)
            {
                return this.rideRepo
               .AllAsNoTracking()
               .Where(r =>
               r.Name.ToLower().Contains(querySplitted[0].ToLower()) ||
               r.Name.ToLower().Contains(querySplitted[1].ToLower())
              ).To<T>()
              .ToList();
            }

            return this.rideRepo
              .AllAsNoTracking()
              .Where(r =>
              r.Name.ToLower().Contains(querySplitted[0].ToLower()))
              .To<T>()
             .ToList();
        }

        public List<T> GetTowns<T>(string query)
        {
            var querySplitted = query
                .Split(' ', ',', '.')
                .Take(2)
                .ToArray();

            if (querySplitted.Count() == 2)
            {
                return this.townRepo
               .AllAsNoTracking()
               .Where(t =>
               t.Name.ToLower().Contains(querySplitted[0].ToLower()) ||
               t.Name.ToLower().Contains(querySplitted[1].ToLower()))
               .To<T>()
              .ToList();
            }

            return this.townRepo
              .AllAsNoTracking()
              .Where(t =>
              t.Name.ToLower().Contains(querySplitted[0].ToLower()))
              .To<T>()
             .ToList();
        }

        public List<T> GetUsers<T>(string query)
        {
            var querySplitted = query.Split(' ', ',', '.').Take(2).ToArray();

            if (querySplitted.Count() == 2)
            {
                return this.userRepo
                     .AllAsNoTracking()
                     .Where(t => t.FirstName.ToLower().Contains(querySplitted[0].ToLower()) ||
                     t.FirstName.ToLower().Contains(querySplitted[1].ToLower()) ||
                    t.LastName.ToLower().Contains(querySplitted[1].ToLower()) ||
                    t.LastName.ToLower().Contains(querySplitted[0].ToLower()))
                     .To<T>().ToList();
            }

            return this.userRepo
                     .AllAsNoTracking()
                     .Where(
                    t => t.FirstName.ToLower().Contains(querySplitted[0].ToLower()) ||
                    t.LastName.ToLower().Contains(querySplitted[0].ToLower()))
                     .To<T>().ToList();

        }
    }
}
