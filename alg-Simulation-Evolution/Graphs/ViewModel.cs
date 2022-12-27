using System;
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
                Name = "Series Name",
                Stroke = Color.FromRgb(113, 96, 232),
                Fill = new SolidColorBrush(Color.FromArgb(50, 113, 96, 232)),
                StrokeThickness = 1
            });

            Start();
        }

        /// <summary> Запуск отрисовки графика </summary>
        private void Start()
        {
            var thread = new Thread(() =>
            {
                var index = 1;
                while (true)
                {
                    var y = index;
                    var x = DateTime.Now.TimeOfDay;

                    Controller.PushData(x, y);

                    Thread.Sleep(300);
                    index++;
                }
            });

            thread.Start();
        }
    }
}
