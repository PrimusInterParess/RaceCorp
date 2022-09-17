namespace RaceCorp.Services.Data
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Net.Security;
    using System.Threading.Tasks;
    using System.Web;

    using Microsoft.AspNetCore.Hosting;
    using RaceCorp.Data.Common.Repositories;
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Data.Contracts;
    using RaceCorp.Web.ViewModels.RaceViewModels;

    public class CreateRaceService : ICreateRaceService
    {
        private readonly IDeletableEntityRepository<Logo> logoRepo;
        private readonly IDeletableEntityRepository<Race> raceRepo;
        private readonly IDeletableEntityRepository<Mountain> mountainRepo;
        private readonly IDeletableEntityRepository<Difficulty> difficultyRepo;
        private readonly IRepository<RaceDifficulty> traceRepo;
        private readonly IDeletableEntityRepository<Town> townRepo;

        public CreateRaceService(IDeletableEntityRepository<Logo> imageRepo, IDeletableEntityRepository<Race> raceRepo, IDeletableEntityRepository<Mountain> mountainRepo, IDeletableEntityRepository<Difficulty> difficultyRepo, IRepository<RaceDifficulty> traceRepo, IDeletableEntityRepository<Town> townRepo)
        {
            this.logoRepo = imageRepo;
            this.raceRepo = raceRepo;
            this.mountainRepo = mountainRepo;
            this.difficultyRepo = difficultyRepo;
            this.traceRepo = traceRepo;
            this.townRepo = townRepo;
        }

        public async Task CreateAsync(AddRaceInputViewModel model)
        {
            var race = new Race();
            race.Name = model.Name;
            race.Date = model.Date;
            race.Description = model.Description;
            race.FormatId = int.Parse(model.FormatId);

            var mountainData = this.mountainRepo.All().FirstOrDefault(m => m.Name.ToLower() == model.Mountain.ToLower());

            if (mountainData == null)
            {
                mountainData = new Mountain()
                {
                    Name = model.Mountain,
                };

                await this.mountainRepo.AddAsync(mountainData);
            }

            race.Mountain = mountainData;

            var townData = this.townRepo.All().FirstOrDefault(t => t.Name.ToLower() == model.Town.ToLower());

            if (townData == null)
            {
                townData = new Town()
                {
                    Name = model.Town,
                };

                await this.townRepo.AddAsync(townData);
            }

            race.Town = townData;

            foreach (var trace in model.Difficulties)
            {
                var dificultyData = this.difficultyRepo.All().FirstOrDefault(d => d.Id == int.Parse(trace.DifficultyId));
                var raceTrace = new RaceDifficulty()
                {
                    ControlTime = trace.ControlTime,
                    DifficultyId = int.Parse(trace.DifficultyId),
                    Length = trace.Length,
                    Race = race,
                    StartTime = trace.StartTime,
                    TrackUrl = trace.TrackUrl,
                };

                race.Traces.Add(raceTrace);
                await this.traceRepo.AddAsync(raceTrace);
            }

            if (!model.RaceLogo.FileName.EndsWith(".png") && !model.RaceLogo.FileName.EndsWith(".jpg") && !model.RaceLogo.FileName.EndsWith(".jpg"))
            {
                throw new Exception("invalid file");
            }

            if (model.RaceLogo.Length > 10 * 1024 * 1024)
            {
                throw new Exception("invalid file size. It needs to be max 10mb.");
            }

            var path = Path.GetFullPath("wwwroot\\Images\\");

            using (FileStream fs = new FileStream(path + model.RaceLogo.FileName, FileMode.Create))
            {
                await model.RaceLogo.CopyToAsync(fs);

                path = fs.Name;
            }

            var logo = new Logo()
            {
                Path = path,
            };

            await this.logoRepo.AddAsync(logo);

            race.Logo = logo;

            await this.raceRepo.AddAsync(race);

            try
            {
                await this.townRepo.SaveChangesAsync();
                await this.mountainRepo.SaveChangesAsync();
                var result = await this.raceRepo.SaveChangesAsync();
                await this.traceRepo.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
