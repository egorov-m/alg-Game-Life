using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using alg_Simulation_Evolution.Services;
using System.Windows.Controls;
using Color = System.Windows.Media.Color;
using Point = System.Windows.Point;

namespace alg_Simulation_Evolution.Organisms
{
    /// <summary> Класс организма </summary>
    public class Organism : Essence, IOrganism, IFood
    {
        /// <summary> Единицы насыщения </summary>
        public double SaturationUnit => BodySize * 0.25;

        /// <summary> Холст размещения организмов </summary>
        protected readonly Panel _canvas;

        /// <summary> Лимит размера для деления организма </summary>
        private double _divSizeLimit;

        /// <summary> Цвет обводки организма (в соответствии с типом) </summary>
        public virtual Color BodyStrokeColor { get; set; } = IOrganism.BodyStrokeColor;

        /// <summary> Лимит размера для деления организма </summary>
        public double DivSizeLimit
        {
            get => _divSizeLimit;
            set
            {
                if (value < IOrganism.DefaultSize) 
                    throw new ArgumentOutOfRangeException($"Лимит размера деления не должен быть меньше значения размера по умолчанию: {IOrganism.DefaultSize}.");
                _divSizeLimit = value;
            }
        }

        /// <summary> Событие деления клетки </summary>
        public event Action OnDivision;

        /// <summary> Дочерние организмы полученные делением </summary>
        public List<object?> Subsidiary { get; } = new();

        /// <summary> Скорость передвижения организма </summary>
        protected double _speed;

        /// <summary> Скорость передвижения организма </summary>
        public double Speed
        {
            get => _speed;
            set
            {
                if (value < 0) throw new ArgumentOutOfRangeException("Значение скорости не может быть меньше нуля.");
                var diffIn = _speed == 0 ? 1.05 : value / _speed;
                var newColor = Color.FromRgb(Convert.ToByte(BodyColor.R * diffIn), Convert.ToByte(BodyColor.G / diffIn), Convert.ToByte(BodyColor.B / diffIn));
                BodyColor = newColor;

                _speed = value;
            }
        }

        /// <summary> Размер тела </summary>
        public override double BodySize
        {
            get => _bodySize;
            set
            {
                if (value < 0) throw new ArgumentOutOfRangeException("Значение размера не может быть отрицательным.");
                var diffIn = _bodySize == 0 ? 1.05 : value / _bodySize;
                var newColor = Color.FromRgb(Convert.ToByte(BodyColor.R / diffIn), Convert.ToByte(BodyColor.G * diffIn), Convert.ToByte(BodyColor.B * diffIn));
                BodyColor = newColor;

                BodyEllipse.Width = value;
                BodyEllipse.Height = value;
                _bodySize = value;

                if (_bodySize > DivSizeLimit)
                {
                    Subsidiary.Add(Divide());
                    OnDivision.Invoke();
                }
            }
        }

        public Organism(Panel canvas)
        {
            (BodyGrid, BodyEllipse) = ConfiguratorViewElement.GetGridForBody(IOrganism.DefaultSize, IOrganism.DefaultBodyColor, BodyStrokeColor);
            _bodySize = IOrganism.DefaultSize;
            BodyColor = IOrganism.DefaultBodyColor;
            _speed = IOrganism.DefaultSpeed;
            canvas.Children.Add(BodyGrid);
            _canvas = canvas;
        }

        public Organism(Panel canvas, double speed)
        {
            (BodyGrid, BodyEllipse) = ConfiguratorViewElement.GetGridForBody(IOrganism.DefaultSize, IOrganism.DefaultBodyColor, BodyStrokeColor);
            BodyColor = IOrganism.DefaultBodyColor;
            _bodySize = IOrganism.DefaultSize;
            Speed = speed;
            canvas.Children.Add(BodyGrid);
            _canvas = canvas;
        }

        public Organism(Panel canvas, double size, double speed)
        {
            (BodyGrid, BodyEllipse) = ConfiguratorViewElement.GetGridForBody(IOrganism.DefaultSize, IOrganism.DefaultBodyColor, BodyStrokeColor);
            BodyColor = IOrganism.DefaultBodyColor;
            BodySize = size;
            Speed = speed;
            canvas.Children.Add(BodyGrid);
            _canvas = canvas;
        }

        /// <summary> Деление организма на два </summary>
        public object? Divide()
        {
            BodySize /= 2;
            Speed *= 2;

            const BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance;
            return Activator.CreateInstance(GetType(), flags, null, _canvas, BodySize, Speed);
        }

        /// <summary> Переместиться по холсту в точку </summary>
        /// <param name="x"> Позиция по X </param>
        /// <param name="y"> Позиция по Y </param>
        public void MoveOnCanvas(double x, double y)
        {
            BodyGrid.Margin = new Thickness(x - BodyEllipse.Width / 2, y - BodyEllipse.Height / 2, 0, 0);
        }

        /// <summary> Переместиться по холсту в точку </summary>
        /// <param name="position"> Новая позиция </param>
        public void MoveOnCanvas(Point position)
        {
            BodyGrid.Margin = new Thickness(position.X - BodyEllipse.Width / 2, position.Y - BodyEllipse.Height / 2, 0, 0);
        }

        /// <summary> Сместиться по холсту на указанное значение </summary>
        /// <param name="diffX"> Смещение по X </param>
        /// <param name="diffY"> Смещение по Y </param>
        public void OffsetOnCanvas(double diffX, double diffY)
        {
            MoveOnCanvas(BodyGrid.Margin.Left + diffX, BodyGrid.Margin.Top + diffY);
        }

        /// <summary> Поглотить еду </summary>
        /// <param name="food"> Еда </param>
        public bool AbsorbFood(Food food)
        {
            var tmp = BodySize;
            BodySize += food.SaturationUnit;
            Speed /= BodySize / tmp;

            return true;
        }
    }
}
