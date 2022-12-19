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
    using RaceCorp.Web.ViewModels.User;

    public class SearchService : ISearchService
    {
        private readonly IDeletableEntityRepository<Town> townRepo;
        private readonly IDeletableEntityRepository<ApplicationUser> userRepo;
        private readonly IDeletableEntityRepository<Mountain> mountainRepo;
        private readonly IDeletableEntityRepository<Race> raceRepo;
        private readonly IDeletableEntityRepository<Ride> rideRepo;
        private readonly IDeletableEntityRepository<Team> teamRepo;
        private readonly IDeletableEntityRepository<Request> requestRepo;
        private readonly IDeletableEntityRepository<Connection> connectionRepo;

        public SearchService(
            IDeletableEntityRepository<Town> townRepo,
            IDeletableEntityRepository<ApplicationUser> userRepo,
            IDeletableEntityRepository<Mountain> mountainRepo,
            IDeletableEntityRepository<Race> raceRepo,
            IDeletableEntityRepository<Ride> rideRepo,
            IDeletableEntityRepository<Team> teamRepo,
            IDeletableEntityRepository<Request> requestRepo,
            IDeletableEntityRepository<Connection> connectionRepo)
        {
            this.townRepo = townRepo;
            this.userRepo = userRepo;
            this.mountainRepo = mountainRepo;
            this.raceRepo = raceRepo;
            this.rideRepo = rideRepo;
            this.teamRepo = teamRepo;
            this.requestRepo = requestRepo;
            this.connectionRepo = connectionRepo;
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
               t.Name.ToLower().Contains(querySplitted[1].ToLower()))
               .To<T>()
              .ToList();
            }

            return this.teamRepo
              .AllAsNoTracking()
              .Where(r =>
              r.Name.ToLower().Contains(querySplitted[0].ToLower())).To<T>()
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
              m.Name.ToLower().Contains(querySplitted[0].ToLower()))
              .To<T>()
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
               r.Name.ToLower().Contains(querySplitted[1].ToLower()))
               .To<T>()
              .ToList();
            }

            return this.raceRepo
              .AllAsNoTracking()
              .Where(r =>
              r.Name.ToLower().Contains(querySplitted[0].ToLower()))
              .To<T>()
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
               r.Name.ToLower().Contains(querySplitted[1].ToLower()))
               .To<T>()
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

        public List<UserAllViewModel> GetUsers(string query, string currentUserId)
        {
            var querySplitted = query.Split(' ', ',', '.').Take(2).ToArray();

            if (querySplitted.Count() == 2)
            {
                var listUsers = this.userRepo
                     .AllAsNoTracking()
                     .Where(u => u.FirstName.ToLower().Contains(querySplitted[0].ToLower()) && u.Id != currentUserId ||
                     u.FirstName.ToLower().Contains(querySplitted[1].ToLower()) && u.Id != currentUserId ||
                     u.LastName.ToLower().Contains(querySplitted[1].ToLower()) && u.Id != currentUserId ||
                     u.LastName.ToLower().Contains(querySplitted[0].ToLower()) && u.Id != currentUserId)
                     .OrderBy(u => u.FirstName)
                     .ThenBy(u => u.LastName)
                     .To<UserAllViewModel>()
                     .ToList();

                foreach (var user in listUsers)
                {
                    user.IsConnected = this.AreConnected(user.Id, currentUserId);
                    user.RequestedConnection = this.RequestedConnection(user.Id, currentUserId);
                    user.CanMessageMe = this.AreConnected(user.Id, currentUserId);
                }

                return listUsers;
            }

            var usersList = this.userRepo
                     .AllAsNoTracking()
                     .Where(u => u.FirstName.ToLower().Contains(querySplitted[0].ToLower()) && u.Id != currentUserId ||
                      u.LastName.ToLower().Contains(querySplitted[0].ToLower()) && u.Id != currentUserId)
                     .OrderBy(u => u.FirstName)
                     .ThenBy(u => u.LastName)
                     .To<UserAllViewModel>().ToList();

            foreach (var user in usersList)
            {
                user.IsConnected = this.AreConnected(user.Id, currentUserId);
                user.RequestedConnection = this.RequestedConnection(user.Id, currentUserId);
                user.CanMessageMe = this.AreConnected(user.Id, currentUserId);
            }

            return usersList;
        }

        private bool RequestedConnection(string currentUserId, string targetUserId)
        {
            return this.requestRepo.AllAsNoTracking()
                 .Any(r => r.TargetUserId == targetUserId && r.RequesterId == currentUserId && r.Type == GlobalConstants.RequestTypeConnectUser ||
                      r.TargetUserId == currentUserId && r.RequesterId == targetUserId && r.Type == GlobalConstants.RequestTypeConnectUser);
        }

        private bool AreConnected(string currentUserId, string targetUserId)
        {
            return this.connectionRepo.AllAsNoTracking().Any(c => c.InterlocutorId == currentUserId && c.ApplicationUserId == targetUserId);
        }
    }
}
