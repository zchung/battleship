
using Battleship.Data.Models;
using Battleship.Logic.Services;
using Battleship.Logic.Services.Interfaces;
using Battleship.Logic.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Battleship.Tests.Services
{
    [TestClass]
    public class CoordinatesValidationServiceTests
    {
        private ICoordinatesValidationService _coordinatesValidationService;

        [TestInitialize]
        public void Initialise()
        {
            _coordinatesValidationService = new CoordinatesValidationService();
        }

        [TestMethod]
        public void ValidateCoordinatesInGameSpace_Should_Return_Success_If_All_Coordinates_Are_Within_Game_Board()
        {
            List<CoordinatesViewModel> coordinates = new List<CoordinatesViewModel>
            {
                new CoordinatesViewModel(1, 1),
                new CoordinatesViewModel(1, 2),
                new CoordinatesViewModel(1, 3)
            };

            Result result = _coordinatesValidationService.ValidateCoordinatesInGameSpace(coordinates);

            Assert.IsTrue(result.Success);
        }

        [TestMethod]
        public void ValidateCoordinatesInGameSpace_Should_Return_False_If_At_Least_One_Coordinates_Are_Not_Within_Game_Board()
        {
            List<CoordinatesViewModel> coordinates = new List<CoordinatesViewModel>
            {
                new CoordinatesViewModel(1, 1),
                new CoordinatesViewModel(11, 2),
                new CoordinatesViewModel(1, 3)
            };

            Result result = _coordinatesValidationService.ValidateCoordinatesInGameSpace(coordinates);

            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);

            coordinates = new List<CoordinatesViewModel>
            {
                new CoordinatesViewModel(1, 1),
                new CoordinatesViewModel(1, 2),
                new CoordinatesViewModel(1, 13)
            };

            result = _coordinatesValidationService.ValidateCoordinatesInGameSpace(coordinates);

            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
        }

        [TestMethod]
        public void ValidateCoordinatesForOverlapping_Should_Return_True_If_Coordinates_Dont_Overlap()
        {
            List<CoordinatesViewModel> newCoordinates = new List<CoordinatesViewModel>
            {
                new CoordinatesViewModel(1, 1),
                new CoordinatesViewModel(1, 2),
                new CoordinatesViewModel(1, 3)
            };

            List<CoordinatesViewModel> existingCoordinates = new List<CoordinatesViewModel>
            {
                new CoordinatesViewModel(2, 1),
                new CoordinatesViewModel(2, 2),
                new CoordinatesViewModel(2, 3)
            };

            Result result = _coordinatesValidationService.ValidateCoordinatesForOverlapping(newCoordinates, existingCoordinates);

            Assert.IsTrue(result.Success);
        }

        [TestMethod]
        public void ValidateCoordinatesForOverlapping_Should_Return_False_If_Coordinates_Overlap()
        {
            List<CoordinatesViewModel> newCoordinates = new List<CoordinatesViewModel>
            {
                new CoordinatesViewModel(1, 1),
                new CoordinatesViewModel(1, 2),
                new CoordinatesViewModel(1, 3)
            };

            List<CoordinatesViewModel> existingCoordinates = new List<CoordinatesViewModel>
            {
                new CoordinatesViewModel(1, 1),
                new CoordinatesViewModel(2, 2),
                new CoordinatesViewModel(2, 3)
            };

            Result result = _coordinatesValidationService.ValidateCoordinatesForOverlapping(newCoordinates, existingCoordinates);

            Assert.IsFalse(result.Success);
        }
    }
}
