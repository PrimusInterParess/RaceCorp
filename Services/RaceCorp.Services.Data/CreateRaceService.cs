namespace RaceCorp.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using RaceCorp.Data.Common.Repositories;
    using RaceCorp.Data.Models;
    using RaceCorp.Services.Data.Contracts;
    using RaceCorp.Web.ViewModels.RaceViewModels;

    public class CreateRaceService : ICreateRaceService
    {
        private readonly IDeletableEntityRepository<Race> raceRepo;
        private readonly IDeletableEntityRepository<Mountain> mountainRepo;
        private readonly IDeletableEntityRepository<Difficulty> difficultyRepo;
        private readonly IRepository<RaceDifficulty> traceRepo;

        public CreateRaceService(IDeletableEntityRepository<Race> raceRepo, IDeletableEntityRepository<Mountain> mountainRepo, IDeletableEntityRepository<Difficulty> difficultyRepo, IRepository<RaceDifficulty> traceRepo)
        {
            this.raceRepo = raceRepo;
            this.mountainRepo = mountainRepo;
            this.difficultyRepo = difficultyRepo;
            this.traceRepo = traceRepo;
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
            }

            race.Mountain = mountainData;

            foreach (var trace in model.Difficulties)
            {
                var dificultyData = this.difficultyRepo.All().FirstOrDefault(d => d.Id == int.Parse(trace.DifficultyId));
                var raceTrace = new RaceDifficulty()
                {
                    ControlTime = trace.ControlTime,
                    Difficulty = dificultyData,
                    Length = trace.Length,
                    Race = race,
                    StartTime = trace.StartTime,
                    TrackUrl = trace.TrackUrl,
                };

                race.Difficulties.Add(raceTrace);
            }

            await this.raceRepo.SaveChangesAsync();
            await this.mountainRepo.SaveChangesAsync();
            await this.traceRepo.SaveChangesAsync();
        }
    }
}
