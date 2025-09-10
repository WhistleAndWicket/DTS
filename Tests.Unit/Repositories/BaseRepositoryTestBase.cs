using Application.Common.Specifications;
using Domain.Entities.Base;
using Infrastructure.Persistence.Repositories;
using Tests.Unit.Core;

namespace Tests.Unit.Repositories
{
    /// <summary>
    /// Unit tests for the BaseRepository class.
    /// </summary>
    /// <typeparam name="T">The database entity.</typeparam>
    public abstract class BaseRepositoryTestBase<T> : RepositoryTestBase where T : Entity, new()
    {
        /// <summary>
        /// The Base repository for the entity.
        /// </summary>
        protected BaseRepository<T> Repository { get; }

        /// <summary>
        /// Create a valid <typeparamref name="T"/> for testing.
        /// </summary>
        /// <param name="title">Optional. Name to set on the Entity.</param>
        /// <returns>The <typeparamref name="T"/>.</returns>
        protected abstract T CreateEntity(string title = "");

        /// <summary>
        /// Verifies that a new entity is successfully added.
        /// </summary>
        [Fact]
        public async Task AddAsync_ShouldAddEntity()
        {
            // Arrange
            var entity = CreateEntity();
            var added = await Repository.AddAsync(entity);
            await Context.SaveChangesAsync();

            Assert.NotNull(added);
            Assert.True(added.Id > 0);
        }

        /// <summary>
        /// Verifies that all the entities are successfully retrieved.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_ShouldReturnAllEntities()
        {
            for (int i = 0; i < 3; i++)
            {
                var entity = CreateEntity($"Entity{i + 1}");
                await Context.Set<T>().AddAsync(entity);
            }

            await Context.SaveChangesAsync();

            var spec = new DefaultSpecification<T>();
            var result = await Repository.GetAllAsync(spec);

            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
        }

        /// <summary>
        /// Verifies that when no entities exist, an empty list is returned.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_ShouldReturnEmpty_WhenNoEntitiesExist()
        {
            // Act
            var specification = new DefaultSpecification<T>();
            var result = await Repository.GetAllAsync(specification);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        /// <summary>
        /// Verifies that an entity is successfully retrieved by its id.
        /// </summary>
        [Fact]
        public async Task GetByIdAsync_ShouldReturnCorrectEntity()
        {
            var entity = CreateEntity();
            await Context.Set<T>().AddAsync(entity);
            await Context.SaveChangesAsync();

            var fromDb = await Repository.GetByIdAsync(entity.Id);
            Assert.NotNull(fromDb);
            Assert.Equal(entity.Id, fromDb.Id);
        }

        /// <summary>
        /// Verifies that when an entity does not exist, null is returned.
        /// </summary>
        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenEntityDoesNotExist()
        {
            var result = await Repository.GetByIdAsync(999);
            Assert.Null(result);
        }

        /// <summary>
        /// Verifies that the paginated entities are retrieved successfully.
        /// </summary>
        /// <param name="page">The page number to test.</param>
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task GetPageResultsAsync_ShouldReturnPaginatedResults(int page)
        {
            // Arrange.
            var entityItems = new List<T>();
            for (int i = 1; i <= 8; i++)
            {
                var entity = CreateEntity($"Entity{i + 1}");
                await Context.Set<T>().AddAsync(entity);
                entityItems.Add(entity);
            }

            await Context.SaveChangesAsync();

            // Act.
            var skip = (page - 1) * 5;
            var expectedItems = entityItems.Skip(skip).Take(5).ToList();
            var (items, total) = await Repository.GetPageResultsAsync(page, 5);

            // Assert.
            Assert.NotNull(items);
            Assert.NotEmpty(items);
            Assert.Equal(entityItems.Count, total);
            Assert.Equal(expectedItems.Count, items.Count);
        }

        /// <summary>
        ///  Initializes a new instance of the <see cref="BaseRepositoryTest"/> class.
        /// </summary>
        public BaseRepositoryTestBase()
        {
            Repository = new BaseRepository<T>(Context);
        }
    }
}
