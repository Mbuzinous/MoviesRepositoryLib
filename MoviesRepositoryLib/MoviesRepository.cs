using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesRepositoryLib
{
    public class MoviesRepository
    {
        private int _nextId = 1;
        private readonly List<Movie> _movies = new();

        public MoviesRepository()
        {
            //_movies.Add(new Movie() { Id = _nextId++, Title = "The Matrix", Year = 1999 });
            //_movies.Add(new Movie() { Id = _nextId++, Title = "Snehvide", Year = 1937 });
        }

        public List<Movie> Get(int? yearAfter = null, string? titleIncludes = null, string? orderBy = null)
        {

            List<Movie> result = new(_movies);
            // Filtering
            if (yearAfter != null)
            {
                result = result.Where(m => m.Year > yearAfter).ToList();
            }
            if (titleIncludes != null)
            {
                result = result.Where(m => m.Title?.ToLower().Contains(titleIncludes.ToLower()) ?? false).ToList();
            }

            // Ordering aka sorting
            switch (orderBy)
            {
                case null: break;
                case "title":
                    result.Sort((m1, m2) => m1.Title.CompareTo(m2.Title));
                    break;
                case "titleDesc":
                    result.Sort((m1, m2) => m2.Title.CompareTo(m1.Title));
                    break;
                case "year":
                    result.Sort((m1, m2) => m1.Year.CompareTo(m2.Year));
                    break;
                case "yearDesc":
                    result.Sort((m1, m2) => m2.Year.CompareTo(m1.Year));
                    break;
                default:
                    throw new ArgumentException("Unknown sort order: " + orderBy);
            }
            return result;
        }

        public Movie? GetById(int id)
        {
            return _movies.Find(movie => movie.Id == id);
        }

        public Movie Add(Movie movie)
        {
            movie.Validate();
            movie.Id = _nextId++;
            _movies.Add(movie);
            return movie;
        }

        public Movie? Remove(int id)
        {
            Movie? movie = GetById(id);
            if (movie == null)
            {
                return null;
            }
            _movies.Remove(movie);
            return movie;
        }

        public Movie? Update(int id, Movie movie)
        {
            movie.Validate();
            Movie? existingMovie = GetById(id);
            if (existingMovie == null)
            {
                return null;
            }
            existingMovie.Title = movie.Title;
            existingMovie.Year = movie.Year;
            return existingMovie;
        }
    }
}
