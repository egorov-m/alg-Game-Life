using alg_Simulation_Evolution.Organisms;
using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace alg_Simulation_Evolution.Services
{
    /// <summary> Класс поставщик контроллера эволюции </summary>
    public class EvolutionControllerProvider : IDisposable
    {
        /// <summary> Режим демонстрации эволюции </summary>
        public static DemoMode DemonstrationMode { get; private set; }

        /// <summary> Задержка в миллисекундах демонстрации эволюции в автоматическом режиме </summary>
        public static int Delay { get; private set; }

        /// <summary> Выполнять ли шаг вперёд </summary>
        public static bool IsStepForward { get; set; }

        /// <summary> Выполнять ли сброс </summary>
        public static bool IsReset;

        public enum DemoMode
        {
            InProcess,
            OnPause
        }

        /// <summary> Кнопка выбора режима демонстрации </summary>
        private readonly Button _btnDemoMode;
        /// <summary> Текстовый блок заголовка кнопки смены режима демонстрации </summary>
        private readonly TextBlock _tbBtnDemoModeTitle;
        /// <summary> Текстовый блок подзаголовка кнопки смены режима демонстрации </summary>
        private readonly TextBlock _tbBtnDemoModeSubtitle;

        /// <summary> Текстовое поля задержки демонстрации </summary>
        private readonly TextBox _textBoxDelay;

        /// <summary> Регулярное выражения для проверки соответствия вводимой задержки </summary>
        private readonly Regex _regexDelay = new (@"[0-9]+");

        public EvolutionControllerProvider(Button btnDemoMode, TextBox textBoxDelay)
        {
            _btnDemoMode    = btnDemoMode;
            if (_btnDemoMode.Content is StackPanel sp)
            {
                if (sp.Children[0] is TextBlock tb1) _tbBtnDemoModeTitle = tb1;
                if (sp.Children[1] is TextBlock tb2) _tbBtnDemoModeSubtitle = tb2;
            }

            _btnDemoMode.Click             += BtnChangeDemoModeOnClick;
            _textBoxDelay                   = textBoxDelay;
            _textBoxDelay.PreviewTextInput += TextBoxDelayOnPreviewTextInput;
            _textBoxDelay.KeyDown          += TextBoxDelayOnKeyDown;

            SetDemoMode(DemoMode.OnPause);
            SetDelay(500);
        }

        /// <summary> Проверка того можно ли продолжать эволюцию или ждать </summary>
        public static bool Continue()
        {
            return DemonstrationMode == DemoMode.InProcess;
        }

        /// <summary> Установка режима демонстрации </summary>
        /// <param name="demoMode"> Режим демонстрации </param>
        private void SetDemoMode(DemoMode demoMode)
        {
            DemonstrationMode = demoMode;

            _tbBtnDemoModeTitle.Text = demoMode == DemoMode.InProcess ? "Эволюция в процессе" : "Эволюция приостановлена";
            if (demoMode == DemoMode.InProcess)
            {
                _tbBtnDemoModeSubtitle.Visibility = Visibility.Visible;
                _btnDemoMode.BorderBrush = new SolidColorBrush(IOrganism.DefaultBodyColor);
                _tbBtnDemoModeSubtitle.Text = $"{Delay}";
            }
            else
            {
                _tbBtnDemoModeSubtitle.Visibility = Visibility.Collapsed;
                _btnDemoMode.BorderBrush = new SolidColorBrush(IOrganism.BodyStrokeColor);
            }
        }

        /// <summary> Установить задержку демонстрации </summary>
        /// <param name="delay"> Задержка </param>
        private void SetDelay(int delay)
        {
            Delay = delay;
            var str = Delay.ToString(CultureInfo.InvariantCulture);
            _textBoxDelay.Text = str;
            _tbBtnDemoModeSubtitle.Text = $"{str} ms";
        }

        /// <summary> Обработка нажатия кнопки Enter и установка введённой задержки </summary>
        /// <param name="sender"> Текстовое поле для ввода задержки </param>
        /// <param name="e"> Событие нажатия клавиши </param>
        private void TextBoxDelayOnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                Keyboard.ClearFocus();
                if (_textBoxDelay.Text == "")
                {
                    SetDelay(0);
                }
                else
                {
                    if (!int.TryParse(_textBoxDelay.Text, out var delay)) throw new ArgumentException("ОШИБКА! Задержка должна быть указана в виде целого неотрицательного числа.");
                
                    SetDelay(delay);
                }
            }
        }

        /// <summary> Предварительная проверка вводимого текста на соответствие (целое неотрицательное число) </summary>
        /// <param name="sender"> Текстовое поле для ввода задержки </param>
        /// <param name="e"> Событие ввода текста </param>
        private void TextBoxDelayOnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !_regexDelay.IsMatch(e.Text);
        }

        /// <summary> Обработчик нажатия кнопки смены режима демонстрации </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnChangeDemoModeOnClick(object sender, RoutedEventArgs e)
        {
            if (DemonstrationMode == DemoMode.InProcess) SetDemoMode(DemoMode.OnPause);
            else  if (DemonstrationMode == DemoMode.OnPause) SetDemoMode(DemoMode.InProcess);
        }

        /// <summary> Выполнить сброс панели управления </summary>
        public void Dispose()
        {
            IsStepForward = false;
            IsReset = false;
        }
    }
}
