using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace alg_Simulation_Evolution.Organisms
{
    /// <summary> Класс хищника </summary>
    public class Predator : Organism, IPredator
    {
        /// <summary> Цвет обводки организма (в соответствии с типом) </summary>
        public override Color BodyStrokeColor { get; set; } = IPredator.BodyStrokeColor;

        public Predator(Panel canvas) : base(canvas)
        {
        }

        public Predator(Panel canvas, double speed) : base(canvas, speed)
        {
        }

        public Predator(Panel canvas, double size, double speed) : base(canvas, size, speed)
        {
        }

        public Predator(Panel canvas, double size, double speed, double divSizeLimit) : base(canvas, size, speed, divSizeLimit)
        {
        }

        /// <summary> Деление организма на два </summary>
        public override IOrganism Divide(Point position)
        {
            BodySize /= 2;
            Speed *= 2;
            //var tmp = DivSizeLimit / 2;
            //DivSizeLimit = tmp > IOrganism.DefaultSize ? tmp : IOrganism.DefaultSize;

            var predator = new Predator(_canvas, BodySize, Speed, DivSizeLimit);
            predator.SetPosition(position);

            return predator;
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
