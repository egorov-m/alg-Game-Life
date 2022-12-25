using System.Windows.Media;

namespace alg_Simulation_Evolution.Organisms
{
    /// <summary> Параметры еды </summary>
    public interface IFood : IBody
    {
        /// <summary> Размер по умолчанию </summary>
        new static double DefaultSize => 8;

        /// <summary> Цвет тела по умолчанию </summary>
        new static Color DefaultBodyColor => Color.FromRgb(154, 230, 154);

        /// <summary> Единицы насыщения </summary>
        double SaturationUnit => BodySize * 0.25;
    }
}
