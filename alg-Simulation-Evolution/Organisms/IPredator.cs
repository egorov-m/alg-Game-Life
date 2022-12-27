using System.Windows.Media;

namespace alg_Simulation_Evolution.Organisms
{
    /// <summary> Параметры хищника </summary>
    public interface IPredator : IOrganism
    {
        /// <summary> Цвет обводки организма (в соответствии с типом) </summary>
        new static Color BodyStrokeColor { get; set; } = Color.FromRgb(224, 27, 45);

        /// <summary> Поглотить организм </summary>
        /// <param name="organism"> Организм </param>
        bool AbsorbOrganism(Organism organism);
    }
}
