using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;

namespace alg_Simulation_Evolution.Organisms
{
    /// <summary> Параметры базового организма </summary>
    public interface IOrganism : IBody
    {
        /// <summary> Лимит по умолчанию размера для деления организма </summary>
        static double DefaultDivSizeLimit => 50;

        /// <summary> Лимит размера для деления организма </summary>
        double DivSizeLimit { get; set; }

        /// <summary> Дочерние организмы полученные делением </summary>
        ObservableCollection<IOrganism> Subsidiary { get; }

        /// <summary> Размер по умолчанию </summary>
        new static double DefaultSize => 30;

        /// <summary> Цвет тела по умолчанию </summary>
        new static Color DefaultBodyColor => Color.FromRgb(113, 96, 232);

        /// <summary> Цвет обводки организма (в соответствии с типом) </summary>
        static Color BodyStrokeColor { get; set; } = Color.FromRgb(214, 214, 214);

        /// <summary> Скорость передвижения по умолчанию </summary>
        static double DefaultSpeed => 10;

        /// <summary> Скорость передвижения организма </summary>
        double Speed { get; }

        /// <summary> Деление организма на два </summary>
        /// /// <param name="position"> Позиция для установки нового организма </param>
        IOrganism Divide(Point position);

        /// <summary> Переместиться по холсту в точку </summary>
        /// <param name="x"> Позиция по X </param>
        /// <param name="y"> Позиция по Y </param>
        void MoveOnCanvas(double x, double y);

        /// <summary> Переместиться по холсту в точку </summary>
        /// <param name="position"> Новая позиция </param>
        void MoveOnCanvas(Point position);

        /// <summary> Сместиться по холсту на указанное значение </summary>
        /// <param name="diffX"> Смещение по X </param>
        /// <param name="diffY"> Смещение по Y </param>
        void OffsetOnCanvas(double diffX, double diffY);

        /// <summary> Поглотить еду </summary>
        /// <param name="food"> Еда </param>
        bool AbsorbFood(Food food);
    }
}
