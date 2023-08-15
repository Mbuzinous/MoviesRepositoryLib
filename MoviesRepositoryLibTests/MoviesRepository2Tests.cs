using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoviesRepositoryLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MoviesRepositoryLib.Tests
{
    [TestClass()]
    public class MoviesRepository2Tests
    {
        private const bool useDatabase = false;
        private static MoviesDbContext? _dbContext;
        private static IMoviesRepository _repo;
        // https://learn.microsoft.com/en-us/dotnet/core/testing/order-unit-tests?pivots=mstest

        [ClassInitialize]
        public static void InitOnce(TestContext context)
        {
            if (useDatabase)
            {
                var optionsBuilder = new DbContextOptionsBuilder<MoviesDbContext>();
                optionsBuilder.UseSqlServer(Secrets.ConnectionString);
                _dbContext = new MoviesDbContext(optionsBuilder.Options);
                // clean database table: remove all rows
                _dbContext.Database.ExecuteSqlRaw("TRUNCATE TABLE dbo.Movies");
                _repo = new MoviesRepositoryDB(_dbContext);
            }
            else
            {
                _repo = new MoviesRepositoryList();
            }
        }

        [TestMethod()]
        public void AddTest()
        {
            _repo.Add(new Movie { Title = "Z", Year = 1895 });
            Movie snowWhite = _repo.Add(new Movie { Title = "Snehvide", Year = 1937 });
            Assert.IsTrue(snowWhite.Id >= 0);
            IEnumerable<Movie> all = _repo.Get();
            Assert.AreEqual(2, all.Count());

            Assert.ThrowsException<ArgumentNullException>(
                () => _repo.Add(new Movie { Title = null, Year = 1895 }));
            Assert.ThrowsException<ArgumentException>(
                () => _repo.Add(new Movie { Title = "", Year = 1895 }));
            Assert.ThrowsException<ArgumentOutOfRangeException>(
                () => _repo.Add(new Movie { Title = "B", Year = 1894 }));
        }

        [TestMethod()]
        public void GetTest()
        {
            IEnumerable<Movie> movies = _repo.Get(orderBy: "Title");

            Assert.AreEqual(movies.First().Title, "Snehvide");

            movies = _repo.Get(orderBy: "Year");
            Assert.AreEqual(movies.First().Title, "Z");

            movies = _repo.Get(titleIncludes: "vide");
            Assert.AreEqual(1, movies.Count());
            Assert.AreEqual(movies.First().Title, "Snehvide");
        }

        [TestMethod()]
        public void GetByIdTest()
        {
            Movie m = _repo.Add(new Movie { Title = "Tarzan", Year = 1932 });
            Movie? movie = _repo.GetById(m.Id);
            Assert.IsNotNull(movie);
            Assert.AreEqual("Tarzan", movie.Title);
            Assert.AreEqual(1932, movie.Year);

            Assert.IsNull(_repo.GetById(-1));
        }

        [TestMethod()]
        public void RemoveTest()
        {
            Movie m = _repo.Add(new Movie { Title = "Olsenbanden", Year = 1968 });
            Movie? movie = _repo.Remove(m.Id);
            Assert.IsNotNull(movie);
            Assert.AreEqual("Olsenbanden", movie.Title);

            Movie? movie2 = _repo.Remove(m.Id);
            Assert.IsNull(movie2);
        }

        [TestMethod()]
        public void UpdateTest()
        {
            Movie m = _repo.Add(new Movie { Title = "Citizen Kane", Year = 1941 });
            Movie? movie = _repo.Update(m.Id, new Movie { Title = "Den Store Mand", Year = 1941 });
            Assert.IsNotNull(movie);
            Movie? movie2 = _repo.GetById(m.Id);
            Assert.AreEqual("Den Store Mand", movie.Title);

            Assert.IsNull(
                _repo.Update(-1, new Movie { Title = "Buh", Year = 1967 }));
            Assert.ThrowsException<ArgumentException>(
                () => _repo.Update(m.Id, new Movie { Title = "", Year = 1941 }));
        }
    }
}