using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesRepositoryLib
{
    public class Movie
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public int Year { get; set; }

        public override string ToString()
        {
            return $"{Id} {Title} {Year}";
        }

        public void ValidateTitle()
        {

            if (Title == null)
            {
                throw new ArgumentNullException(nameof(Title), "Title cannot be null");
            }
            if (Title.Length < 1)
            {
                throw new ArgumentException("Title must be at least 1 character", nameof(Title));
            }
        }

        public void ValidateYear()
        {
            if (Year < 1895)
            {
                throw new ArgumentOutOfRangeException(nameof(Year), "Year must be at least 1895");
            }
        }

        public void Validate()
        {
            ValidateTitle();
            ValidateYear();
        }
    }
}
