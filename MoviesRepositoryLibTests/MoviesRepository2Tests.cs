using Microsoft.EntityFrameworkCore;
// NuGet  Microsoft.EntityFrameworkCore
using Microsoft.Extensions.Options;
// NuGet Microsoft.EntityFrameworkCore.SqlServer
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
    [TestClass]
    [DoNotParallelize]
    public class MoviesRepository2Tests
    {
        private const bool useDatabase = true;
        private static IMoviesRepository _repo;
        // https://learn.microsoft.com/en-us/dotnet/core/testing/order-unit-tests?pivots=mstest

        //[ClassInitialize]
        //public static void InitOnce(TestContext context)
        [TestInitialize]
        public void Init()
        {
           
            if (useDatabase)
            {
                var optionsBuilder = new DbContextOptionsBuilder<MoviesDbContext>();
                optionsBuilder.UseSqlServer(Secrets.ConnectionStringSimply);
                // connection string structure
                //   "Data Source=mssql7.unoeuro.com;Initial Catalog=FROM simply.com;Persist Security Info=True;User ID=FROM simply.com;Password=DB PASSWORD FROM simply.com;TrustServerCertificate=True"
                MoviesDbContext _dbContext = new(optionsBuilder.Options);
                // clean database table: remove all rows
                _dbContext.Database.ExecuteSqlRaw("TRUNCATE TABLE dbo.Movies");
                _repo = new MoviesRepositoryDB(_dbContext);
            }
            else
            {
                _repo = new MoviesRepositoryList();
            }
        }

        [TestMethod, Priority(1)]
        [DoNotParallelize]
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

        [TestMethod, Priority(2)]
        [DoNotParallelize]
        public void GetATest()
        {
            _repo.Add(new Movie { Title = "Z", Year = 1895 });
            _repo.Add(new Movie { Title = "Snehvide", Year = 1937 });
            _repo.Add(new Movie { Title = "Den Store Mand", Year = 1941 });

            IEnumerable<Movie> movies = _repo.Get(orderBy: "Title");

            Assert.AreEqual(movies.First().Title, "Den Store Mand");

            movies = _repo.Get(orderBy: "Year");
            Assert.AreEqual(movies.First().Title, "Z");

            movies = _repo.Get(titleIncludes: "vide");
            Assert.AreEqual(1, movies.Count());
            Assert.AreEqual(movies.First().Title, "Snehvide");
        }

        [TestMethod, Priority(3)]
        public void GetByIdTest()
        {
            Movie m = _repo.Add(new Movie { Title = "Tarzan", Year = 1932 });
            Movie? movie = _repo.GetById(m.Id);
            Assert.IsNotNull(movie);
            Assert.AreEqual("Tarzan", movie.Title);
            Assert.AreEqual(1932, movie.Year);

            Assert.IsNull(_repo.GetById(-1));
        }

        [TestMethod, Priority(7)]
        [DoNotParallelize]
        public void RemoveTest()
        {
            Movie m = _repo.Add(new Movie { Title = "Olsenbanden", Year = 1968 });
            Movie? movie = _repo.Remove(m.Id);
            Assert.IsNotNull(movie);
            Assert.AreEqual("Olsenbanden", movie.Title);

            Movie? movie2 = _repo.Remove(m.Id);
            Assert.IsNull(movie2);
        }

        [TestMethod, Priority(10)]
        [DoNotParallelize]
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