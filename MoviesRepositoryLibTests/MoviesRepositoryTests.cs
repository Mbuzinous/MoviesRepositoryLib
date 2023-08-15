using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoviesRepositoryLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// NuGet Microsoft.EntityFrameworkCore.SqlServer

namespace MoviesRepositoryLib.Tests
{
    //[TestClass()]
    public class MoviesRepositoryTests
    {
        private const bool useDatabase = false;
        private static MoviesDbContext _dbContext;
        private IMoviesRepository _repo;
        // https://learn.microsoft.com/en-us/dotnet/core/testing/order-unit-tests?pivots=mstest
        private readonly Movie _badMovie = new() { Title = "The Matrix", Year = 1894 };

        [ClassInitialize]
        public static void InitOnce(TestContext context)
        {
            if (useDatabase)
            {
                var optionsBuilder = new DbContextOptionsBuilder<MoviesDbContext>();
                optionsBuilder.UseSqlServer(Secrets.ConnectionString);
                _dbContext = new MoviesDbContext(optionsBuilder.Options);
                //List<Movie> all = _dbContext.Movies.ToList();
                //_dbContext.RemoveRange(all);

                //_dbContext.SaveChanges();
            }
        }


        [TestInitialize]
        public void Init()
        {
            if (useDatabase)
            {
                _repo = new MoviesRepositoryDB(_dbContext);
                _dbContext.Database.ExecuteSqlRaw("TRUNCATE TABLE dbo.Movies");
            }
            else { _repo = new MoviesRepositoryList(); }

            _repo.Add(new Movie() { Title = "The Matrix", Year = 1999 });
            _repo.Add(new Movie() { Title = "Snehvide", Year = 1937 });
            _repo.Add(new Movie() { Title = "Løvejagten", Year = 1907 });
            _repo.Add(new Movie() { Title = "Abekongen", Year = 1961 });
        }


        [TestMethod()]
        public void GetTest()
        {
            IEnumerable<Movie> movies = _repo.Get();
            Assert.AreEqual(4, movies.Count());
            Assert.AreEqual(movies.First().Title, "The Matrix");

            IEnumerable<Movie> sortedMovies = _repo.Get(orderBy: "title");
            Assert.AreEqual(sortedMovies.First().Title, "Abekongen");

            IEnumerable<Movie> sortedMovies2 = _repo.Get(orderBy: "year");
            Assert.AreEqual(sortedMovies2.First().Title, "Løvejagten");
        }

        [TestMethod()]
        public void GetByIdTest()
        {
            Assert.IsNotNull(_repo.GetById(1));
            Assert.IsNull(_repo.GetById(100));
        }

        [TestMethod()]
        public void AddTest()
        {
            Movie m = new() { Title = "Test", Year = 2021 };
            Assert.AreEqual(5, _repo.Add(m).Id);
            Assert.AreEqual(5, _repo.Get().Count());

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => _repo.Add(_badMovie));
        }

        [TestMethod()]
        public void RemoveTest()
        {
            Assert.IsNull(_repo.Remove(100));
            Assert.AreEqual(1, _repo.Remove(1)?.Id);
            Assert.AreEqual(3, _repo.Get().Count());
        }

        [TestMethod()]
        public void UpdateTest()
        {
            Assert.AreEqual(4, _repo.Get().Count());
            Movie m = new() { Title = "Test", Year = 2021 };
            Assert.IsNull(_repo.Update(100, m));
            Assert.AreEqual(1, _repo.Update(1, m)?.Id);
            Assert.AreEqual(4, _repo.Get().Count());

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => _repo.Update(1, _badMovie));
        }
    }
}