using PBC_FDTD_2D.Utilities;

namespace PBC_FDTD_2D.Structures
{
    public struct UnitCellDetails
    {
        public double XPeriod { get; }
        public double YPeriod { get; }
        public double DielectricBackground { get; }
        public double RodHoleDielectric { get; }
        public double RodHoleRadius { get; }
        public DiscretisationInfo DiscretisationInfo { get; }

        public UnitCellDetails(
            double xPeriod,
            double yPeriod,
            double dielectricBackground,
            double rodholeDielectric,
            double rodHoleRadius,
            DiscretisationInfo discretisationInfo)
        {
            XPeriod = xPeriod;
            YPeriod = yPeriod;
            DielectricBackground = dielectricBackground;
            RodHoleDielectric = rodholeDielectric;
            RodHoleRadius = rodHoleRadius;
            DiscretisationInfo = discretisationInfo;
        }
    }
}
