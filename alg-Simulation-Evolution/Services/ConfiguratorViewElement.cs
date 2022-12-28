using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace alg_Simulation_Evolution.Services
{
    /// <summary> Класс для получения настроенных визуальных элементов </summary>
    public static class ConfiguratorViewElement
    {
        public static double StrokeThicknessElement = 1;

        public static (Grid, Ellipse) GetGridForBody(double size, Color bodyMainColor, Color bodyStrokeColor)
        {
            var element = new Grid();
            var ellipse = new Ellipse()
            {
                Width = size,
                Height = size,
                StrokeThickness = StrokeThicknessElement,
                Stroke = new SolidColorBrush(bodyStrokeColor),
                Fill = new SolidColorBrush(bodyMainColor)
            };

            element.Children.Add(ellipse);

            return (element, ellipse);
        }

        /// <summary> Получение цвета для окраски организма в соответствие с его скоростью </summary>
        /// <param name="speed"> Скорость </param>
        public static Color GetColorAccordingSpeed(double speed)
        {
            return speed switch
            {
                <= 5  => Color.FromRgb(93, 229, 218),
                <= 15 => Color.FromRgb(113, 96, 232),
                <= 20 => Color.FromRgb(200, 164, 232),
                <= 30 => Color.FromRgb(200, 149, 109),
                <= 40 => Color.FromRgb(223, 118, 58),
                _     => Color.FromRgb(255, 53, 53)
            };
        }
    }
}
