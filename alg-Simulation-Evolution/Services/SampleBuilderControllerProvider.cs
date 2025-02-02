﻿using System;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using alg_Simulation_Evolution.Data;
using alg_Simulation_Evolution.Organisms;

namespace alg_Simulation_Evolution.Services
{
    /// <summary> Класс поставщик контроллера создания выборки </summary>
    public class SampleBuilderControllerProvider : IDisposable
    {
        /// <summary> Холст </summary>
        private readonly Canvas _canvas;

        /// <summary> Поставщик данных </summary>
        private readonly DataProvider _dataProvider;

        /// <summary> Кнопка случайной генерации выборки </summary>
        private readonly Button _btnRandomSampling;

        /// <summary> Поле ввода количество элементов для случайной генерации выборки </summary>
        private readonly TextBox _tbRandomSampling;

        /// <summary> Кнопка добавления обычных организмов в выборку </summary>
        private readonly Button _btnAddOrganisms;

        /// <summary> Поле ввода количества добавляемых обычных организмов в выборку </summary>
        private readonly TextBox _tbAddOrganisms;

        /// <summary> Кнопка добавления хищников в выборку </summary>
        private readonly Button _btnAddPredators;

        /// <summary> Поле ввода количества добавляемых хищников в выборку </summary>
        private readonly TextBox _tbAddPredators;

        /// <summary> Поле ввода количества добавляемых единиц пищи в выборку </summary>
        private readonly TextBox _tbAddFood;

        /// <summary> Поле ввода размера для добавляемых организмов </summary>
        private readonly TextBox _tbSizeOrganisms;

        /// <summary> Поле ввода скорости для добавляемых организмов </summary>
        private readonly TextBox _tbSpeedOrganisms;

        /// <summary> Поле ввода лимита размера для деления добавляемых организмов </summary>
        private readonly TextBox _tbDivSizeLimitOrganisms;

        /// <summary> Кнопка переключения режима автоматического пополнения </summary>
        private readonly Button _btnAutoAddSampling;

        /// <summary> Заголовок кнопки переключения режима автоматического пополнения </summary>
        private readonly TextBlock _tbBtnAutoAddSampling;

        /// <summary> Подзаголовок кнопки переключения режима автоматического пополнения </summary>
        private readonly TextBlock _tbBtnAutoAddSamplingSubtitle;

        /// <summary> Текстовое поле ввода промежутка между автоматическими пополнениями выборки </summary>
        private readonly TextBox _tbAutoAddSampling;

        /// <summary> Текущий режим пополнения </summary>
        private AutoSamplingMode _autoSamplingMode;

        /// <summary> Включён ли режим автоматического пополнения </summary>
        private enum AutoSamplingMode
        {
            Off,
            On
        }

        /// <summary> Регулярное выражение для проверки соответствия вводимого количества элементов </summary>
        private readonly Regex _regexCountElements = new (@"[0-9]+");

        /// <summary> Регулярное выражение для проверки соответствия вводимых параметров элементов </summary>
        private readonly Regex _regexParamsElements = new (@"[0-9]*[.,]?[0-9]+");

        /// <summary> Генератор случайных чисел </summary>
        private readonly Random _random = new();

        public SampleBuilderControllerProvider(Canvas  canvas,
                                               DataProvider dataProvider,
                                               Button  btnRandomSampling, 
                                               TextBox tbRandomSampling, 
                                               Button  btnAddOrganisms, 
                                               TextBox tbAddOrganisms,
                                               Button  btnAddPredators,
                                               TextBox tbAddPredators,
                                               Button  btnAddFood,
                                               TextBox tbAddFood,
                                               Button  btnResetSelection,
                                               TextBox tbSizeOrganisms,
                                               TextBox tbSpeedOrganisms,
                                               TextBox tbDivSizeLimitOrganisms,
                                               Button btnAutoAddSampling,
                                               TextBlock tbBtnAutoAddSampling,
                                               TextBlock tbBtnAutoAddSamplingSubtitle,
                                               TextBox tbAutoAddSampling)
        {
            _canvas = canvas;
            _dataProvider = dataProvider;

            // Добавление случайной выборки
            _btnRandomSampling = btnRandomSampling;
            _btnRandomSampling.Click += BtnRandomSamplingOnClick;
            _tbRandomSampling = tbRandomSampling;
            _tbRandomSampling.PreviewTextInput += TextBoxCountElementsOnPreviewTextInput;
            _tbRandomSampling.KeyDown += TextBoxOnKeyDown;

            // Добавление обычных организмов
            _btnAddOrganisms = btnAddOrganisms;
            _btnAddOrganisms.Click += BtnAddOrganismsOnClick;
            _tbAddOrganisms = tbAddOrganisms;
            _tbAddOrganisms.PreviewTextInput += TextBoxCountElementsOnPreviewTextInput;
            _tbAddOrganisms.KeyDown += TextBoxOnKeyDown;

            // Добавление хищников
            _btnAddPredators = btnAddPredators;
            _btnAddPredators.Click += BtnAddPredatorsOnClick;
            _tbAddPredators = tbAddPredators;
            _tbAddPredators.PreviewTextInput += TextBoxCountElementsOnPreviewTextInput;
            _tbAddPredators.KeyDown += TextBoxOnKeyDown;

            // Добавление пищи
            btnAddFood.Click += BtnAddFoodOnClick;
            _tbAddFood = tbAddFood;
            _tbAddFood.PreviewTextInput += TextBoxCountElementsOnPreviewTextInput;
            _tbAddFood.KeyDown += TextBoxOnKeyDown;

            // Сбрасывание выборки
            btnResetSelection.Click += BtnResetSelectionOnClick;

            // Размер добавляемых организмов
            _tbSizeOrganisms = tbSizeOrganisms;
            _tbSizeOrganisms.PreviewTextInput += TextBoxParamsElementsOnPreviewTextInput;
            _tbSizeOrganisms.KeyDown += TextBoxOnKeyDown;

            // Скорость добавляемых организмов
            _tbSpeedOrganisms = tbSpeedOrganisms;
            _tbSpeedOrganisms.PreviewTextInput += TextBoxParamsElementsOnPreviewTextInput;
            _tbSpeedOrganisms.KeyDown += TextBoxOnKeyDown;

            // Лимит размера деления организмов
            _tbDivSizeLimitOrganisms = tbDivSizeLimitOrganisms;
            _tbDivSizeLimitOrganisms.PreviewTextInput += TextBoxParamsElementsOnPreviewTextInput;
            _tbDivSizeLimitOrganisms.KeyDown += TextBoxOnKeyDown;

            // Автоматическое добавление элементов в выборку
            _btnAutoAddSampling = btnAutoAddSampling;
            _btnAutoAddSampling.Click += BtnAutoAddSamplingOnClick;
            _tbBtnAutoAddSamplingSubtitle = tbBtnAutoAddSamplingSubtitle;
            _tbBtnAutoAddSampling = tbBtnAutoAddSampling;

            // Промежуток добавления элементов в выборку
            _tbAutoAddSampling = tbAutoAddSampling;
            _tbAutoAddSampling.PreviewTextInput += TextBoxCountElementsOnPreviewTextInput;
            _tbAutoAddSampling.KeyDown += TextBoxAutoSamplingTimeOnKeyDown;

            // Автоматическое пополнение по умолчанию отключено
            SetAutoSamplingMode(AutoSamplingMode.Off);
            _tbAutoAddSampling.Text = "1000";
            AutoAddSampling();
        }

        /// <summary> Обработка нажатия кнопки переключения режима пополнения выборки </summary>
        /// <param name="sender"> Кнопка </param>
        /// <param name="e"> Событие клика </param>
        private void BtnAutoAddSamplingOnClick(object sender, RoutedEventArgs e)
        {
            if (_autoSamplingMode == AutoSamplingMode.On) SetAutoSamplingMode(AutoSamplingMode.Off);
            else if (_autoSamplingMode == AutoSamplingMode.Off) SetAutoSamplingMode(AutoSamplingMode.On);
        }

        /// <summary> Автоматическое добавление выборки </summary>
        private async void AutoAddSampling()
        {
            await Task.Run(() => { while (_autoSamplingMode != AutoSamplingMode.On) {} }); // Ожидание того, пока не включат автоматическое пополнение
            BtnAddOrganismsOnClick(null, null);
            BtnAddPredatorsOnClick(null, null);
            BtnAddFoodOnClick(null, null);
            var sleepTime = ParseInt(_tbAutoAddSampling.Text);
            await Task.Run(() => Thread.Sleep(sleepTime));
            
            AutoAddSampling();
        }

        /// <summary> Установка режима наполнения выборки </summary>
        /// <param name="autoSamplingMode"> Включён ли автоматический </param>
        private void SetAutoSamplingMode(AutoSamplingMode autoSamplingMode)
        {
            _autoSamplingMode = autoSamplingMode;

            _tbBtnAutoAddSampling.Text = autoSamplingMode == AutoSamplingMode.On ? "Автоматическое пополнение" : "Пополнение вручную";
            if (autoSamplingMode == AutoSamplingMode.On)
            {
                _tbBtnAutoAddSamplingSubtitle.Visibility = Visibility.Visible;
                _btnAutoAddSampling.BorderBrush = new SolidColorBrush(IOrganism.DefaultBodyColor);
                _tbBtnAutoAddSamplingSubtitle.Text = $"{_tbAutoAddSampling.Text} ms";
                _tbAutoAddSampling.IsEnabled = false;
            }
            else
            {
                _btnAutoAddSampling.BorderBrush = new SolidColorBrush(IOrganism.BodyStrokeColor);
                _tbBtnAutoAddSamplingSubtitle.Visibility = Visibility.Collapsed;
                _tbAutoAddSampling.IsEnabled = true;
            }
        }

        /// <summary> Обработка нажатия кнопки Enter и установка введённой задержки </summary>
        /// <param name="sender"> Текстовое поле для ввода задержки </param>
        /// <param name="e"> Событие нажатия клавиши </param>
        private void TextBoxAutoSamplingTimeOnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return || e.Key == Key.Enter)
            {
                Keyboard.ClearFocus();
                if (_tbAutoAddSampling.Text == "")
                {
                    _tbAutoAddSampling.Text = "1000";
                }

                _tbBtnAutoAddSamplingSubtitle.Text = $"{_tbAutoAddSampling.Text} ms";
            }
        }

        /// <summary> Кнопка сброса выборки </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnResetSelectionOnClick(object sender, RoutedEventArgs e)
        {
            _canvas.Children       .Clear();
            _dataProvider.Organisms.Clear();
            _dataProvider.Predators.Clear();
            _dataProvider.Food     .Clear();
        }

        /// <summary> Кнопка добавления Хищников </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAddPredatorsOnClick(object sender, RoutedEventArgs e)
        {
            AddEssence((canvas, size, speed, divSizeLimit) => new Predator(canvas, size, speed, divSizeLimit), _tbAddPredators.Text, OrganismType.Predator);
        }

        /// <summary> Кнопка добавления пищи </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAddFoodOnClick(object sender, RoutedEventArgs e)
        {
            AddEssence((canvas, size, speed, divSizeLimit) => new Food(canvas, size / 6), _tbAddFood.Text, OrganismType.Food);
        }

        /// <summary> Добавлять сущности </summary>
        /// <param name="essence"> Функция задающая сущность </param>
        /// <param name="strCount"> Количество элементов в строковом формате </param>
        private void AddEssence(Func<Canvas, double, double, double, Essence> essence, string strCount, OrganismType organismType)
        {
            var count = ParseInt(strCount);
            var positions = new Point[count];
            var size = ParseDouble(_tbSizeOrganisms.Text);
            var speed = ParseDouble(_tbSpeedOrganisms.Text);
            var divSizeLimit = ParseDouble(_tbDivSizeLimitOrganisms.Text);

            for (var i = 0; i < count; i++)
            {
                positions[i] = GetPoint(_random, _canvas.ActualWidth, _canvas.ActualHeight);
            }

            switch (organismType)
            {
                case OrganismType.Usual:
                    foreach (var position in positions)
                    {
                        var organism = (IOrganism) essence(_canvas, size, speed, divSizeLimit);
                        _dataProvider.Organisms.Add(organism);
                        organism.SetPosition(position);
                    }
                    break;
                case OrganismType.Predator:
                    foreach (var position in positions)
                    {
                        var organism = (IPredator) essence(_canvas, size, speed, divSizeLimit);
                        _dataProvider.Predators.Add(organism);
                        organism.SetPosition(position);
                    }
                    break;
                case OrganismType.Food:
                    foreach (var position in positions)
                    {
                        var organism = (IFood) essence(_canvas, size, speed, divSizeLimit);
                        _dataProvider.Food.Add(organism);
                        organism.SetPosition(position);
                    }
                    break;
            }
        }

        /// <summary> Кнопка добавления организмов </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAddOrganismsOnClick(object sender, RoutedEventArgs e)
        {
            AddEssence((canvas, size, speed, divSizeLimit) => new Organism(canvas, size, speed, divSizeLimit), _tbAddOrganisms.Text, OrganismType.Usual);
        }

        /// <summary> Получить точку на холсте </summary>
        public static Point GetPoint(Random random, double width, double height)
        {
            var x = width * 0.03 + random.NextDouble() * width * 0.87;
            x = x < 10 ? 10 : x;
            var y = width * 0.03 + random.NextDouble() * height * 0.87;
            y = y < 10 ? 10 : y;
            return new Point(x, y);
        }

        /// <summary> Парсинг целого числа из поля ввода </summary>
        /// <param name="str"> Строка для парсинга </param>
        public static int ParseInt(string str)
        {
            return int.Parse(str == "" ? "10" : str);
        }

        /// <summary> Парсинг вещественного числа из поля ввода </summary>
        /// <param name="str"> Строка для парсинга </param>
        public static double ParseDouble(string str)
        {
            return double.Parse(str == "" ? "10" : str);
        }

        /// <summary> Обработчик нажатия кнопки случайной генерации выборки </summary>
        /// <param name="sender"> Кнопка </param>
        /// <param name="e"> Событие клика </param>
        private async void BtnRandomSamplingOnClick(object sender, RoutedEventArgs e)
        {
            if (_random.NextDouble() >= 0.5)
            {
                AddEssence((canvas, size, speed, divSizeLimit) => new Organism(canvas, size, speed, divSizeLimit), _tbRandomSampling.Text, OrganismType.Usual);
            }
            else
            {
                AddEssence((canvas, size, speed, divSizeLimit) => new Predator(canvas, size, speed, divSizeLimit), _tbRandomSampling.Text, OrganismType.Predator);
            }

            AddEssence((canvas, size, speed, divSizeLimit) => new Food(canvas, size / 6), _tbAddFood.Text, OrganismType.Food);
        }

        /// <summary> Обработка нажатия кнопки Enter </summary>
        /// <param name="sender"> Текстовое поле для ввода задержки </param>
        /// <param name="e"> Событие нажатия клавиши </param>
        private void TextBoxOnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key is Key.Return or Key.Enter)
            {
                Keyboard.ClearFocus();
                if (sender is TextBox {Text: ""} tb)
                {
                    tb.Text = "10";
                }
            }
        }

        /// <summary> Предварительная проверка вводимого текста на соответствие (целое неотрицательное число) </summary>
        /// <param name="sender"> Текстовое поле для ввода задержки </param>
        /// <param name="e"> Событие ввода текста </param>
        private void TextBoxCountElementsOnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !_regexCountElements.IsMatch(e.Text);
        }

        /// <summary> Предварительная проверка вводимого текста на соответствие (вещественное неотрицательное число) </summary>
        /// <param name="sender"> Текстовое поле для ввода задержки </param>
        /// <param name="e"> Событие ввода текста </param>
        private void TextBoxParamsElementsOnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !_regexParamsElements.IsMatch(e.Text);
        }

        public void Dispose()
        {
        }
    }
}
