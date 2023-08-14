using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesRepositoryLib
{
    public class MoviesRepositoryDB : IMoviesRepository
    {
        private readonly MoviesDbContext _context;

        public MoviesRepositoryDB(MoviesDbContext dbContext)
        {
            _context = dbContext;
        }

        public Movie Add(Movie movie)
        {
            movie.Validate();
            movie.Id = 0;
            _context.Movies.Add(movie);
            _context.SaveChanges();
            return movie;
        }

        public List<Movie> Get(int? yearAfter = null, string? titleIncludes = null, string? orderBy = null)
        {
            return _context.Movies.ToList();
        }

        public Movie? GetById(int id)
        {
            return _context.Movies.FirstOrDefault(m => m.Id == id);
        }

        public Movie? Remove(int id)
        {
            Movie? movie = GetById(id);
            if (movie is null)
            {
                return null;
            }
            _context.Movies.Remove(movie);
            _context.SaveChanges();
            return movie;
        }

        public Movie? Update(int id, Movie movie)
        // https://www.learnentityframeworkcore.com/dbcontext/modifying-data
        {
            movie.Validate();
            Movie? movieToUpdate = GetById(id);
            if (movieToUpdate == null) return null;
            movieToUpdate.Title = movie.Title;
            movieToUpdate.Year = movie.Year;
            _context.SaveChanges();
            return movieToUpdate;
        }
    }
}
