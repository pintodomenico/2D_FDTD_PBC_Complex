using System;

namespace MathsAndPhysics
{
    public static class SpaceTimeDiscretiser
    {
        public static int CalculateDiscreteQuantity(double continueVariable, double discretisationStep)
        {
            return (int)(continueVariable / discretisationStep);
        }

        #region Courant Limit Methods
        public static double CalculateTimeStepUsingCourantLimit(double deltaX)
        {
            return CalculateTimeStepUsingCourantLimit3D(deltaX, deltaX, deltaX, 0.0, 0.0);
        }

        public static double CalculateTimeStepUsingCourantLimit(double deltaX, double deltaY)
        {
            return CalculateTimeStepUsingCourantLimit3D(deltaX, deltaY, deltaX, 1.0, 0.0);
        }

        public static double CalculateTimeStepUsingCourantLimit(double deltaX, double deltaY, double deltaZ)
        {
            return CalculateTimeStepUsingCourantLimit3D(deltaX, deltaY, deltaZ, 1.0, 1.0);
        }

        private static double CalculateTimeStepUsingCourantLimit3D(double deltaX, double deltaY, double deltaZ, double coeffDeltaY, double coeffDeltaZ)
        {
            return (1.0 / (PhysicalConstants.C0 * Math.Sqrt(1.0 / (deltaX * deltaX) + coeffDeltaY / (deltaY * deltaY) + coeffDeltaZ / (deltaZ * deltaZ))));
        }
        #endregion
    }
}
