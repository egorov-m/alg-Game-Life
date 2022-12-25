namespace alg_Simulation_Evolution.Organisms
{
    /// <summary> Параметры хищника </summary>
    public interface IPredator : IOrganism
    {
        /// <summary> Поглотить организм </summary>
        /// <param name="organism"> Организм </param>
        bool AbsorbOrganism(Organism organism);
    }
}
