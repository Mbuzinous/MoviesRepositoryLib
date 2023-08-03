using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoviesRepositoryLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesRepositoryLib.Tests
{
    [TestClass()]
    public class MovieTests
    {
        private readonly Movie _movie = new() { Id = 1, Title = "A", Year = 1895 };
        private readonly Movie _nullTitle = new() { Id = 2, Year = 1895 };
        private readonly Movie _emptyTitle = new Movie() { Id = 3, Title = "", Year = 1895 };
        private readonly Movie _yearLow = new Movie() { Id = 4, Title = "The Matrix", Year = 1894 };

        [TestMethod()]
        public void ToStringTest()
        {
            Assert.AreEqual("1 A 1895", _movie.ToString());
        }

        [TestMethod()]
        public void ValidateTitleTest()
        {
            _movie.ValidateTitle();
            Assert.ThrowsException<ArgumentNullException>(() => _nullTitle.ValidateTitle());
            Assert.ThrowsException<ArgumentException>(() => _emptyTitle.ValidateTitle());
        }

        [TestMethod()]
        public void ValidateYearTest()
        {
            _movie.ValidateYear();
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => _yearLow.ValidateYear());
        }

        [TestMethod()]
        public void ValidateTest()
        {
            _movie.Validate();
        }
    }
}