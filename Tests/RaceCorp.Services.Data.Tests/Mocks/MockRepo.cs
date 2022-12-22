namespace RaceCorp.Services.Data.Tests.Mocks
{
    using Moq;
    using RaceCorp.Data.Common.Models;
    using RaceCorp.Data.Common.Repositories;

    public static class MockRepo
    {
        public static Mock<IDeletableEntityRepository<TEntity>> MockDeletableRepository<TEntity>()
           where TEntity : class, IDeletableEntity
        {
            return new Mock<IDeletableEntityRepository<TEntity>>();
        }

        public static Mock<IRepository<TEntity>> MockRepository<TEntity>()
            where TEntity : class
        {
            return new Mock<IRepository<TEntity>>();
        }
    }
}
