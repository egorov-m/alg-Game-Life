using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace alg_Simulation_Evolution.Organisms
{
    /// <summary> Базовый класс сущности </summary>
    public abstract class Essence : IBody
    {
        /// <summary> Сетка для элементов тела </summary>
        public Grid BodyGrid { get; init; }

        /// <summary> Эллипс (основа) тела </summary>
        public Ellipse BodyEllipse { get; init; }

        /// <summary> Размер тела </summary>
        protected double _bodySize;

        /// <summary> Размер тела </summary>
        public virtual double BodySize
        {
            get => _bodySize;
            set
            {
                if (value < 0) throw new ArgumentOutOfRangeException("Значение размера не может быть отрицательным.");
                BodyEllipse.Width = value;
                BodyEllipse.Height = value;
                _bodySize = value;
            }
        }

        /// <summary> Цвет тела </summary>
        private Color _bodyColor;

        /// <summary> Цвет тела </summary>
        public Color BodyColor
        {
            get => _bodyColor;
            set
            {
                BodyEllipse.Fill = new SolidColorBrush(value);
                _bodyColor = value;
            }
        }
    }
}
