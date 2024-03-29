﻿using Xunit;
using System.Threading.Tasks;
using APIQuiz.Models;

namespace APIQuiz.Services.Tests
{
    public class GameServiceTests
    {

        private readonly GameService gameService;

        public GameServiceTests()
        {
            gameService = GameService.Singleton();
        }

        [Fact]
        [Trait("GameService", "ViewNew")]
        public async Task ViewNew_Simple_Test()
        {
            // TODO: This test should not use the network and must mock HTTP requests
            var question = await gameService.ViewNew(1, true);

            Assert.IsType<Question>(question);
        }

        [Fact]
        [Trait("GameService", "ViewNew")]
        public async Task ViewNew_ReUseQuestion_Test()
        {
            // TODO: This test should not use the network and must mock HTTP requests
            var newQuestion = await gameService.ViewNew(1, true);
            var existingQuestion = await gameService.ViewNew(1, false);

            Assert.Equal(existingQuestion, newQuestion);
        }

        [Fact]
        [Trait("GameService", "ComparePlayerAnswer")]
        public async Task ComparePlayerAnswerTest()
        {
            // TODO: This test should not use the network and must mock HTTP requests
            _ = await gameService.ViewNew(1, true);
            var question = await gameService.ViewNew(1, false);

            bool compareResult = gameService.ComparePlayerAnswer("True", 1);
            Assert.Equal(question.CorrectAnswer == "True", compareResult);
        }
    }
}