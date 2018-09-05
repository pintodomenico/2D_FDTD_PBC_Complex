using PBC_FDTD_2D.Utilities;
using MathsAndPhysics;

namespace PBC_FDTD_2D.Structures
{
    public class Structure : IStructure
    {
        //these need to become ReadOnly (somehow)
        public double[,] Permittivity { get; private set; }
        public double[,] Permeability { get; private set; }
        public double[,] ElecCond { get; private set; }
        public double[,] MagnCond { get; private set; }

        public int XCells { get; }
        public int YCells { get; }

        private readonly UnitCellDetails unitCellDetails;
        private readonly DiscretisationInfo discretisationInfo;

        public Structure(UnitCellDetails unitCellDetails, DiscretisationInfo discretisationInfo)
        {
            this.unitCellDetails = unitCellDetails;
            this.discretisationInfo = discretisationInfo;
            XCells = SpaceTimeDiscretiser.CalculateDiscreteQuantity(unitCellDetails.XPeriod, discretisationInfo.Dx);
            YCells = SpaceTimeDiscretiser.CalculateDiscreteQuantity(unitCellDetails.YPeriod, discretisationInfo.Dy);
            InitialiseStructure();
        }

        private void InitialiseStructure()
        {
            InstantiateMaterialProperties();
            //at the moment i'm only considering circular rod/hole placed at the centre of the unit cell
            double xCentre = unitCellDetails.XPeriod / 2.0;
            double yCentre = unitCellDetails.YPeriod / 2.0;
            for (int indexX = 0; indexX < XCells; indexX++)
            {
                double x = indexX * discretisationInfo.Dx;
                for (int indexY = 0; indexY < YCells; indexY++)
                {
                    double y = indexY * discretisationInfo.Dy;
                    //at the moment i'm only considering circular rod/hole in the unit cell
                    Permittivity[indexX, indexY] = IsPointInsideCircle(x, xCentre, y, yCentre)
                        ? unitCellDetails.RodHoleDielectric
                        : unitCellDetails.DielectricBackground;

                    Permeability[indexX, indexY] = PhysicalConstants.Mi0;
                    ElecCond[indexX, indexY] = 0.0;
                    MagnCond[indexX, indexY] = 0.0;
                }
            }
        }

        private void InstantiateMaterialProperties()
        {
            Permittivity = new double[XCells, YCells];
            Permeability = new double[XCells, YCells];
            ElecCond = new double[XCells, YCells];
            MagnCond = new double[XCells, YCells];
        }

        private bool IsPointInsideCircle(double x, double xCentre, double y, double yCentre) 
            => ((x - xCentre) * (x - xCentre) + (y - yCentre) * (y - yCentre)) < unitCellDetails.RodHoleRadius * unitCellDetails.RodHoleRadius;
    }
}
