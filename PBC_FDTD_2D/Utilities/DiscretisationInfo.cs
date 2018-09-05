namespace PBC_FDTD_2D.Utilities
{
    public struct DiscretisationInfo
    {
        public double Dx { get; }
        public double Dy { get; }
        public double Dt { get; }

        public DiscretisationInfo(double dx, double dy, double dt)
        {
            Dx = dx;
            Dy = dy;
            Dt = dt;
        }
    }
}
