﻿using System.ComponentModel;
using System;
using System.Windows;
using System.Windows.Controls;
using alg_Simulation_Evolution.Data;
using alg_Simulation_Evolution.EngineOfEvolution;
using alg_Simulation_Evolution.Services;

namespace alg_Simulation_Evolution
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Canvas _canvas;
        public static DataProvider DataProvider;
        private SampleBuilderControllerProvider _builderControllerProvider;
        private EvolutionControllerProvider _evolutionControllerProvider;
        private Evolutioner _evolutioner;

        public MainWindow()
        {
            InitializeComponent();

            _canvas = Canvas;
            DataProvider = new DataProvider();
            _evolutionControllerProvider = new EvolutionControllerProvider(btnAlgDemoMode, tbDelayAlgStep);
            _builderControllerProvider = new SampleBuilderControllerProvider(_canvas,
                                                                             DataProvider,
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
                                                                             tbDivSizeLimitOrganisms,
                                                                             btnAutoAddSampling,
                                                                             tbBtnAutoAddSampling,
                                                                             tbBtnAutoAddSamplingSubtitle,
                                                                             tbAutoAddSampling);

            _evolutioner = new Evolutioner(DataProvider, _canvas);
        }

        /// <summary> Обработчик нажатия кнопки (крестик) закрытия главного окна </summary>
        /// <param name="sender"> Кнопка </param>
        /// <param name="e"> Событие клика </param>
        private void MainWindowOnClosing(object? sender, CancelEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
