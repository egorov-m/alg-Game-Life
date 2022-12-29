using System;
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

        /// <summary> Скорость передвижения организма </summary>
        protected double _speed;

        /// <summary> Скорость передвижения организма </summary>
        public double Speed
        {
            get => _speed;
            set
            {
                if (value < 0) throw new ArgumentOutOfRangeException("Значение скорости не может быть меньше нуля.");
                _speed = value;
                BodyColor = ConfiguratorViewElement.GetColorAccordingSpeed(_speed);
            }
        }

        /// <summary> Размер тела </summary>
        public override double BodySize
        {
            get => _bodySize;
            set
            {
                if (value < 0) throw new ArgumentOutOfRangeException("Значение размера не может быть отрицательным.");
                BodyEllipse.Width = value;
                BodyEllipse.Height = value;
                _bodySize = value;
                //BodyColor = ConfiguratorViewElement.GetColorAccordingSpeed(_speed);

                if (_bodySize > DivSizeLimit)
                {
                    //Subsidiary.Add(Divide(Position));
                    var organism = Divide(Position);
                    if (organism is IPredator predator)
                    {
                        MainWindow.DataProvider.Predators.Add(predator);
                    }
                    else
                    {
                        MainWindow.DataProvider.Organisms.Add(organism);
                    }
                }
            }
        }

        public Organism(Panel canvas)
        {
            _canvas = canvas;
            (BodyGrid, BodyEllipse) = ConfiguratorViewElement.GetGridForBody(IOrganism.DefaultSize, IOrganism.DefaultBodyColor, BodyStrokeColor);
            _bodySize = IOrganism.DefaultSize;
            BodyColor = IOrganism.DefaultBodyColor;
            DivSizeLimit = IOrganism.DefaultDivSizeLimit;
            _speed = IOrganism.DefaultSpeed;
            canvas.Children.Add(BodyGrid);
        }

        public Organism(Panel canvas, double speed)
        {
            _canvas = canvas;
            (BodyGrid, BodyEllipse) = ConfiguratorViewElement.GetGridForBody(IOrganism.DefaultSize, IOrganism.DefaultBodyColor, BodyStrokeColor);
            DivSizeLimit = IOrganism.DefaultDivSizeLimit;
            BodyColor = IOrganism.DefaultBodyColor;
            _bodySize = IOrganism.DefaultSize;
            Speed = speed;
            canvas.Children.Add(BodyGrid);
        }

        public Organism(Panel canvas, double size, double speed)
        {
            if (size > IOrganism.DefaultDivSizeLimit) 
                throw new ArgumentOutOfRangeException($"Размер организмов не должен превышать лимит деления по умолчанию ({IOrganism.DefaultDivSizeLimit}).");
            _canvas = canvas;
            (BodyGrid, BodyEllipse) = ConfiguratorViewElement.GetGridForBody(IOrganism.DefaultSize, IOrganism.DefaultBodyColor, BodyStrokeColor);
            DivSizeLimit = IOrganism.DefaultDivSizeLimit;
            BodyColor = IOrganism.DefaultBodyColor;

            // !!! порядок важен, так-как размер может вызвать деление
            Speed = speed;
            BodySize = size;
            canvas.Children.Add(BodyGrid);
        }

        public Organism(Panel canvas, double size, double speed, double divSizeLimit)
        {
            if (size > divSizeLimit) throw new ArgumentOutOfRangeException("Размер организмов не должен превышать указанный лимит деления.");
            _canvas = canvas;
            (BodyGrid, BodyEllipse) = ConfiguratorViewElement.GetGridForBody(size, IOrganism.DefaultBodyColor, BodyStrokeColor);
            DivSizeLimit = divSizeLimit;
            BodyColor = IOrganism.DefaultBodyColor;

            // !!! порядок важен, так-как размер может вызвать деление
            Speed = speed;
            BodySize = size;
            canvas.Children.Add(BodyGrid);
        }

        /// <summary> Деление организма на два </summary>
        public virtual IOrganism Divide(Point position)
        {
            Speed *= 1.5;
            BodySize /= 2;
            //var tmp = DivSizeLimit / 2;
            //DivSizeLimit = tmp > IOrganism.DefaultSize ? tmp : IOrganism.DefaultSize;

            var organism = new Organism(_canvas, BodySize, Speed, DivSizeLimit);
            organism.SetPosition(position);

            return organism;
        }

        /// <summary> Переместиться по холсту в точку </summary>
        /// <param name="x"> Позиция по X </param>
        /// <param name="y"> Позиция по Y </param>
        public void MoveOnCanvas(double x, double y)
        {
            if (!double.IsNaN(x) || !double.IsNaN(y))
            {
                Position = new Point(x, y);
                BodyGrid.Margin = new Thickness(x - BodyEllipse.Width / 2, y - BodyEllipse.Height / 2, 0, 0);
            }
        }

        /// <summary> Переместиться по холсту в точку </summary>
        /// <param name="position"> Новая позиция </param>
        public void MoveOnCanvas(Point position)
        {
            if (!double.IsNaN(position.X) || !double.IsNaN(position.Y))
            {
                Position = position;
                BodyGrid.Margin = new Thickness(position.X - BodyEllipse.Width / 2, position.Y - BodyEllipse.Height / 2, 0, 0);
            }
        }

        /// <summary> Сместиться по холсту на указанное значение </summary>
        /// <param name="diffX"> Смещение по X </param>
        /// <param name="diffY"> Смещение по Y </param>
        public void OffsetOnCanvas(double diffX, double diffY)
        {
            if (!double.IsNaN(diffX) || !double.IsNaN(diffY))
            {
                Position = new Point(BodyGrid.Margin.Left + diffX, BodyGrid.Margin.Top + diffY);
                MoveOnCanvas(Position);
            }
        }

        /// <summary> Поглотить еду </summary>
        /// <param name="food"> Еда </param>
        public bool AbsorbFood(Food food)
        {
            var tmp = BodySize;
            BodySize += food.SaturationUnit;
            Speed /= BodySize / tmp;
            _canvas.Children.Remove(food.BodyGrid);

            return true;
        }
    }
}
