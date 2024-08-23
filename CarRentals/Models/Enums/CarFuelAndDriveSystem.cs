namespace CarRentals.Models.Enums
{
    /// <summary>
    /// Car fuel and air conditioning system types.
    /// </summary>
    public enum CarFuelAndAirConSystem
    {
        UnspecifiedFuelWithAir = 'R',
        UnspecifiedFuelWithoutAir = 'N',
        DieselWithAir = 'D',
        DieselNoAir = 'Q',
        Hybrid = 'H',
        HybridPlugIn = 'I',
        ElectricVehicle = 'E',
        ElectricVehicleSpecial = 'C',
        LPGWithAir = 'L',
        LPGNoAir = 'S',
        HydrogenWithAir = 'A',
        HydrogenNoAir = 'B',
        MultiFuelWithAir = 'M',
        MultiFuelNoAir = 'F',
        PetrolWithAir = 'V',
        PetrolNoAir = 'Z',
        EthanolWithAir = 'U',
        EthanolNoAir = 'X'
    }
}
