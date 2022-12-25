using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace alg_Simulation_Evolution.Organisms
{
    /// <summary> Параметры тела </summary>
    public interface IBody
    {
        /// <summary> Сетка для элементов тела </summary>
        Grid BodyGrid { get; init; }

        /// <summary> Эллипс (основа) тела </summary>
        Ellipse BodyEllipse { get; init; }

        /// <summary> Размер по умолчанию </summary>
        static double DefaultSize;

        /// <summary> Размер тела </summary>
        double BodySize { get; set; }

        /// <summary> Цвет тела по умолчанию </summary>
        static Color DefaultBodyColor;

        /// <summary> Цвет тела </summary>
        Color BodyColor { get; set; }
    }
}
