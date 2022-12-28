using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows;
using Color = System.Windows.Media.Color;

namespace alg_Simulation_Evolution.Organisms
{
    /// <summary> Параметры тела </summary>
    public interface IBody
    {
        /// <summary> Позиция тела на холсте </summary>
        Point Position { get; }

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

        /// <summary> Установить позицию на холсте </summary>
        /// /// <param name="position"> Позиция на холсте </param>
        void SetPosition(Point position);
    }
}
