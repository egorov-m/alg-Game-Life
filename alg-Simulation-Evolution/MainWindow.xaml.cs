using System.Windows;
using System.Windows.Controls;
using alg_Simulation_Evolution.Data;
using alg_Simulation_Evolution.Services;

namespace alg_Simulation_Evolution
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Canvas _canvas;
        private DataProvider _dataProvider;
        private SampleBuilderControllerProvider _builderControllerProvider;

        public MainWindow()
        {
            InitializeComponent();

            _canvas = Canvas;
            _dataProvider = new DataProvider();
            _builderControllerProvider = new SampleBuilderControllerProvider(_canvas,
                                                                             _dataProvider,
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
