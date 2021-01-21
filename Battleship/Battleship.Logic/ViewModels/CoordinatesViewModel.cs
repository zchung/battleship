
namespace Battleship.Logic.ViewModels
{
    public class CoordinatesViewModel
    {
        public int XPosition { get; set; }
        public int YPosition { get; set; }
        public bool Hit { get; set; }

        public CoordinatesViewModel() { }
        public CoordinatesViewModel(int xPosition, int yPosition)
        {
            XPosition = xPosition;
            YPosition = yPosition;
        }
    }
}
