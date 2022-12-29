using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Media;
using RealTimeGraphX;
using RealTimeGraphX.DataPoints;
using RealTimeGraphX.WPF;

namespace alg_Simulation_Evolution.Graphs
{
    public class ViewModel
    {
        //Graph controller with timespan as X axis and double as Y.
        public WpfGraphController<TimeSpanDataPoint, DoubleDataPoint> Controller { get; set; }

        public ViewModel()
        {
            Controller = new WpfGraphController<TimeSpanDataPoint, DoubleDataPoint>();
            Controller.Range.MinimumY = 0;
            Controller.Range.MaximumY = int.MaxValue;
            Controller.Range.MaximumX = TimeSpan.FromSeconds(10);
            Controller.Range.AutoY = true;
            Controller.Range.AutoYFallbackMode = GraphRangeAutoYFallBackMode.None;

            Controller.DataSeriesCollection.Add(new WpfGraphDataSeries()
            {
                Name = "До 5.",
                Stroke = Color.FromRgb(93, 229, 218),
                StrokeThickness = 1
            });

            Controller.DataSeriesCollection.Add(new WpfGraphDataSeries()
            {
                Name = "От 6 до 15.",
                Stroke = Color.FromRgb(113, 96, 232),
                //Fill = new SolidColorBrush(Color.FromArgb(50, 113, 96, 232)),
                StrokeThickness = 1
            });

            Controller.DataSeriesCollection.Add(new WpfGraphDataSeries()
            {
                Name = "От 16 до 20.",
                Stroke = Color.FromRgb(200, 164, 232),
                StrokeThickness = 1
            });

            Controller.DataSeriesCollection.Add(new WpfGraphDataSeries()
            {
                Name = "От 21 до 30.",
                Stroke = Color.FromRgb(200, 149, 109),
                StrokeThickness = 1
            });

            Controller.DataSeriesCollection.Add(new WpfGraphDataSeries()
            {
                Name = "От 31 до 40.",
                Stroke = Color.FromRgb(223, 118, 58),
                StrokeThickness = 1
            });

            Controller.DataSeriesCollection.Add(new WpfGraphDataSeries()
            {
                Name = "Больше 40.",
                Stroke = Color.FromRgb(255, 53, 53),
                StrokeThickness = 1
            });

            Start();
        }

        /// <summary> Получить количество элементов на основе скорости </summary>
        /// <param name="speed1"> Верхний предел </param>
        /// <param name="speed2"> Нижний предел </param>
        private int GetCountBySpeed(double speed1, double speed2 = double.MinValue)
        {
            var count = 0;
            var dataProvider = MainWindow.DataProvider;
            if (dataProvider != null)
            {
                count += dataProvider.Organisms.ToList().Count(organism => organism.Speed <= speed1 && organism.Speed >= speed2);

                count += dataProvider.Predators.ToList().Count(organism => organism.Speed <= speed1 && organism.Speed >= speed2);
            }

            return count;
        }

        /// <summary> Запуск отрисовки графика </summary>
        private void Start()
        {
            var thread = new Thread(() =>
            {
                while (true)
                {
                    // Выделение групп на основе попадания скоростей в промежуток
                    var yy = new List<DoubleDataPoint>()
                    {
                        GetCountBySpeed(5),
                        GetCountBySpeed(15, 6),
                        GetCountBySpeed(20, 16),
                        GetCountBySpeed(30, 21),
                        GetCountBySpeed(40, 31),
                        GetCountBySpeed(double.MaxValue, 41)
                    };

                    var x = DateTime.Now.TimeOfDay;
                    var xx = new List<TimeSpanDataPoint>()
                    {
                        x,
                        x,
                        x,
                        x,
                        x,
                        x
                    };

                    Controller.PushData(xx, yy);

                    Thread.Sleep(100);
                }
            });

            thread.Start();
        }
    }
}
