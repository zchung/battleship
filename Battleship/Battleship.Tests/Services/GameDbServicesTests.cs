
using Battleship.Data.Context.Interfaces;
using Battleship.Data.Entities;
using Battleship.Logic.Services;
using Battleship.Logic.Services.Interfaces;
using BrandManager.Common.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Battleship.Data.Enums;

namespace Battleship.Tests.Services
{
    [TestClass]
    public class GameDbServicesTests : TestBase
    {
        private Mock<IBattleshipDbContext> _battleshipDbContext;
        private IGameDbService _gameDbServices;

        public GameDbServicesTests()
        {
        }

        [TestInitialize]
        public void Initalise()
        {
            _battleshipDbContext = new Mock<IBattleshipDbContext>();
            _gameDbServices = new GameDbService(_battleshipDbContext.Object);
        }

        [TestMethod]
        public async Task Create_With_No_Exceptions_Should_Call_Correct_Context_Methods()
        {
            var result = await _gameDbServices.Create(new Game());

            _battleshipDbContext.Verify(v => v.AddAsync(It.IsAny<object>(), It.IsAny<CancellationToken>()), Times.Once);
            _battleshipDbContext.Verify(v => v.SaveChangesAsync(), Times.Once);

            Assert.IsTrue(result.Success);
        }
        [TestMethod]
        public async Task Create_With_Exception_Should_Return_Failed_Result()
        {
            _battleshipDbContext.Setup(s => s.SaveChangesAsync()).Throws(new Exception());

            var result = await _gameDbServices.Create(new Game());

            Assert.IsNotNull(result);
            Assert.IsFalse(result.Success);
        }

        [TestMethod]
        public async Task GetActiveGames_Should_Return_The_List_Of_Games_With_No_Error()
        {
            _battleshipDbContext.Setup(s => s.Games).Returns(SetupDbContextList(new TestAsyncEnumerable<Game>(new List<Game> { new Game { GameId = 1, GameStatus= GameStatus.Active} }.AsQueryable())));

            var result = await _gameDbServices.GetActiveGames();

            Assert.IsTrue(result.Success);
            Assert.IsNotNull(result.Data);
            Assert.IsTrue(result.Data.Any());
        }
    }
}
