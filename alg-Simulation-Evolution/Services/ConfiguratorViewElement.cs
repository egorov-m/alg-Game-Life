using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace alg_Simulation_Evolution.Services
{
    /// <summary> Класс для получения настроенных визуальных элементов </summary>
    public static class ConfiguratorViewElement
    {
        public static double StrokeThicknessElement = 0.5;
        public static Color StrokeColorElement = Color.FromRgb(214, 214, 214);

        public static (Grid, Ellipse) GetGridForBody(double size, Color bodyMainColor)
        {
            var element = new Grid();
            var ellipse = new Ellipse()
            {
                Width = size,
                Height = size,
                StrokeThickness = StrokeThicknessElement,
                Stroke = new SolidColorBrush(StrokeColorElement),
                Fill = new SolidColorBrush(bodyMainColor)
            };

            element.Children.Add(ellipse);

            return (element, ellipse);
        }
    }
}
