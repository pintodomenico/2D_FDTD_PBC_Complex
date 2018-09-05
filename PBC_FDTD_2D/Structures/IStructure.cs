namespace PBC_FDTD_2D.Structures
{
    public interface IStructure
    {
        double[,] ElecCond { get; }
        double[,] MagnCond { get; }
        double[,] Permeability { get; }
        double[,] Permittivity { get; }

        int XCells { get; }
        int YCells { get; }
    }
}