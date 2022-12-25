using System.Windows.Controls;
using alg_Simulation_Evolution.Services;

namespace alg_Simulation_Evolution.Organisms
{
    /// <summary> Класс представляющий еду </summary>
    public class Food : Essence, IFood
    {
        /// <summary> Единицы насыщения </summary>
        public double SaturationUnit => BodySize * 0.25;

        public Food(Panel canvas)
        {
            (BodyGrid, BodyEllipse) = ConfiguratorViewElement.GetGridForBody(IFood.DefaultSize, IFood.DefaultBodyColor);
            _bodySize = IFood.DefaultSize;
            BodyColor = IFood.DefaultBodyColor;
            canvas.Children.Add(BodyGrid);
        }

        public Food(Panel canvas, double size)
        {
            (BodyGrid, BodyEllipse) = ConfiguratorViewElement.GetGridForBody(IFood.DefaultSize, IFood.DefaultBodyColor);
            BodySize = size;
            BodyColor = IFood.DefaultBodyColor;
            canvas.Children.Add(BodyGrid);
        }
    }
}
