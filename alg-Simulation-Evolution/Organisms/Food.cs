using System.Windows.Controls;
using System.Windows.Media;
using alg_Simulation_Evolution.Services;

namespace alg_Simulation_Evolution.Organisms
{
    /// <summary> Класс представляющий еду </summary>
    public class Food : Essence, IFood
    {
        /// <summary> Единицы насыщения </summary>
        public double SaturationUnit => BodySize * 0.25;

        /// <summary> Цвет обводки организма (в соответствии с типом) </summary>
        public Color BodyStrokeColor { get; set; } = IFood.BodyStrokeColor;

        public Food(Panel canvas)
        {
            (BodyGrid, BodyEllipse) = ConfiguratorViewElement.GetGridForBody(IFood.DefaultSize, IFood.DefaultBodyColor, BodyStrokeColor);
            _bodySize = IFood.DefaultSize;
            BodyColor = IFood.DefaultBodyColor;
            canvas.Children.Add(BodyGrid);
        }

        public Food(Panel canvas, double size)
        {
            (BodyGrid, BodyEllipse) = ConfiguratorViewElement.GetGridForBody(IFood.DefaultSize, IFood.DefaultBodyColor, BodyStrokeColor);
            BodySize = size;
            BodyColor = IFood.DefaultBodyColor;
            canvas.Children.Add(BodyGrid);
        }
    }
}
