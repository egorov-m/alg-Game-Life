using System.Windows.Controls;

namespace alg_Simulation_Evolution.Organisms
{
    /// <summary> Класс хищника </summary>
    public class Predator : Organism, IPredator
    {
        public Predator(Panel canvas) : base(canvas)
        {
        }

        public Predator(Panel canvas, double speed) : base(canvas, speed)
        {
        }

        public Predator(Panel canvas, double size, double speed) : base(canvas, size, speed)
        {
        }

        public bool AbsorbOrganism(Organism organism)
        {
            var tmp = BodySize;
            BodySize += organism.SaturationUnit;
            Speed /= BodySize / tmp;

            return true;
        }
    }
}
