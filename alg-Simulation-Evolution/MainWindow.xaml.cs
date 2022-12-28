using System;
using System.Drawing;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using alg_Simulation_Evolution.Organisms;
using alg_Simulation_Evolution.Services;
using Color = System.Windows.Media.Color;
using Image = System.Drawing.Image;
using Rectangle = System.Drawing.Rectangle;

namespace alg_Simulation_Evolution
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Canvas _canvas;
        private SampleBuilderControllerProvider _builderControllerProvider;

        public MainWindow()
        {
            InitializeComponent();

            _canvas = Canvas;
            _builderControllerProvider = new SampleBuilderControllerProvider(_canvas,
                                                                             btnRandomSampling,
                                                                             tbRandomSampling,
                                                                             btnAddOrganisms,
                                                                             tbAddOrganisms,
                                                                             btnAddPredators,
                                                                             tbAddPredators,
                                                                             btnAddFood,
                                                                             tbAddFood,
                                                                             btnResetSelection,
                                                                             tbSizeOrganisms,
                                                                             tbSpeedOrganisms,
                                                                             tbDivSizeLimitOrganisms);

        }
    }
}
