using RealTimeGraphX;
using RealTimeGraphX.DataPoints;
using RealTimeGraphX.WPF;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Media;
using alg_Simulation_Evolution.Organisms;

namespace alg_Simulation_Evolution.Graphs
{
    public class ViewModelPredatorGraphs
    {
        //Graph controller with timespan as X axis and double as Y.
        public WpfGraphController<TimeSpanDataPoint, DoubleDataPoint> Controller { get; set; }

        public ViewModelPredatorGraphs()
        {
            Controller = new WpfGraphController<TimeSpanDataPoint, DoubleDataPoint>();
            Controller.Range.MinimumY = 0;
            Controller.Range.MaximumY = int.MaxValue;
            Controller.Range.MaximumX = TimeSpan.FromSeconds(10);
            Controller.Range.AutoY = true;
            Controller.Range.AutoYFallbackMode = GraphRangeAutoYFallBackMode.None;

            Controller.DataSeriesCollection.Add(new WpfGraphDataSeries()
            {
                Name = "Not Predators",
                Stroke = Color.FromRgb(214, 214, 214),
                StrokeThickness = 1
            });

            Controller.DataSeriesCollection.Add(new WpfGraphDataSeries()
            {
                Name = "Predators",
                Stroke = Color.FromRgb(224, 27, 45),
                StrokeThickness = 1
            });

            Start();
        }

        /// <summary> Получить количество организмов по категории хищники / не хищники </summary>
        /// <param name="organismType"> Тип организма </param>
        private int GetCountByPredators(OrganismType organismType)
        {
            var count = 0;
            var dataProvider = MainWindow.DataProvider;
            if (dataProvider != null)
            {
                if (organismType == OrganismType.Predator) count = MainWindow.DataProvider.Predators.Count;
                else count = MainWindow.DataProvider.Organisms.Count;
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
                        GetCountByPredators(OrganismType.Usual),
                        GetCountByPredators(OrganismType.Predator),
                    };

                    var x = DateTime.Now.TimeOfDay;
                    var xx = new List<TimeSpanDataPoint>()
                    {
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
