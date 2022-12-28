using System.Collections.Generic;
using alg_Simulation_Evolution.Organisms;

namespace alg_Simulation_Evolution.Data
{
    public class DataProvider
    {
        /// <summary> Обычные организмы </summary>
        public List<IOrganism> Organisms { get; }

        /// <summary> Хищники </summary>
        public List<IPredator> Predators { get; }

        /// <summary> Пища </summary>
        public List<IFood> Food { get; }

        public DataProvider()
        {
            Organisms = new List<IOrganism>();
            Predators = new List<IPredator>();
            Food =      new List<IFood>();
        }
    }
}
