﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Movies.Entities;

namespace Movies.DAC
{
    public class MovieRepository :  IMovieRepositorio
    {
        private string ConnectionString = null; 
        public MovieRepository()
        {
            this.ConnectionString = ConfigurationManager.ConnectionStrings["Movies"].ConnectionString;
        }

        
        public IEnumerable<Entities.Movie> GetAll()
        {
            IEnumerable<Entities.Movie> result = new List<Entities.Movie>(); 
            using ( var con  = new SqlConnection(this.ConnectionString))
            {
                 result = con.Query<Entities.Movie>("usp_movies_get", commandType: System.Data.CommandType.StoredProcedure); 
            }
            return result;                 
        }
        public Entities.Movie GetOne(int Id)
        {
            Entities.Movie result = null;

            var context = new MoviesDbContext();
            
            result = (from c in context.Movies.Include("Reviews")
                        where c.Id == Id
                        select c).FirstOrDefault(); 
            
            //using (var con = new SqlConnection(this.ConnectionString))
            //{
            //    result = con.Query<Entities.Movie>("usp_movies_get_one", 
            //        new {Id = Id },
            //        commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            //}
            return result;                 
        }

        public int Count()
        {
            int result = 0; 
            using (var con = new SqlConnection(this.ConnectionString))
            {
                var cmd = con.CreateCommand();
                cmd.CommandText = "usp_movies_count";
                cmd.CommandType = System.Data.CommandType.StoredProcedure; 
                con.Open();
                result = (int)cmd.ExecuteScalar();                
            }
            return result; 
        }


        public void Create(Entities.Movie Model)
        {
            var context = new MoviesDbContext();
            context.Movies.Add(Model);
            context.SaveChanges();              
        }



        public void CreateReview(Entities.Review Model)
        {
            var context = new MoviesDbContext();
            context.Reviews.Add(Model);
            context.SaveChanges();              
        }

        public IList<Movie> Search(string Filter)
        {
            var entities = new MoviesDbContext();

            var query = (from c in entities.Movies
                         where c.Title.Contains(Filter)
                         select c).ToList();

            return query;
        }
    }
}
