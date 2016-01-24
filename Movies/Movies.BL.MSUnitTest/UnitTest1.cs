﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Movies.Entities;
using NSubstitute;
using System.Collections.Generic;
using Movies.DAC;

namespace Movies.BL.MSUnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void CreateReviewTest()
        {
            var model = new Review();
            model.Id = 0;
            model.Movie = new Movie() { Id = 0, Title = "Test", Description = "test" };
            model.Score = 5;
            model.Comment = "this is a great movie";

            var repository = Substitute.For<IMovieRepositorio>();

            var grammar = Substitute.For<IGrammarService>();

            var badwords = new List<String>() { "sapos" };

            grammar.GetBadWords().Returns(badwords);

            var service = new MoviesService(repository, grammar);

            service.CreateReview(model);

            //Assert
            repository.Received().CreateReview(model);

        }
        [TestMethod]
        public void CreateReviewTestWithSapos()
        {
            var model = new Review();
            model.Id = 0;
            model.Movie = new Movie() { Id = 0, Title = "Test", Description = "test" };
            model.Score = 5;
            model.Comment = "this is sapos movie";

            var repository = Substitute.For<IMovieRepositorio>();

            var grammar = Substitute.For<IGrammarService>();

            var badwords = new List<String>() { "sapos" };

            grammar.GetBadWords().Returns(badwords);

            var service = new MoviesService(repository, grammar);

            try
            {
                service.CreateReview(model);

                Assert.Fail("the validation is not complete");
            }
            catch (ApplicationException ex)
            {

            }
            //Assert
            repository.DidNotReceive().CreateReview(model);

        }

    }
}