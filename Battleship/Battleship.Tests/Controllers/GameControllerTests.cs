using Battleship.Controllers;
using Battleship.Controllers.Interfaces;
using Battleship.Data.Entities;
using Battleship.Data.Models;
using Battleship.Logic.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

namespace Battleship.Tests.Controllers
{    
    [TestClass]
    public class GameControllerTests
    {
        private Mock<IGameDbService> _gameDbService;
        private IGameController _gameController;

        [TestInitialize]
        public void Initialise()
        {
            _gameDbService = new Mock<IGameDbService>();
            _gameController = new GameController(_gameDbService.Object);
        }

        [TestMethod]
        public async Task Create_Should_Return_The_Result()
        {
            _gameDbService.Setup(s => s.Create(It.IsAny<Game>())).ReturnsAsync(new Result<int> { Data = 1, Success = true });

            var result = await _gameController.Create();

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okObject = result as OkObjectResult;
            Assert.IsInstanceOfType(okObject.Value, typeof(Result<int>));
            var castResult = okObject.Value as Result<int>;
            Assert.IsTrue(castResult.Success);
        }
    }
}
