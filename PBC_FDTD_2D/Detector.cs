using System.Collections.Generic;
using MathsAndPhysics;

namespace PBC_FDTD_2D
{
    public class Detector
    {
        private List<double> timeVariation = new List<double>();
        public IReadOnlyList<double> TimeVariation => timeVariation;


        public double X { get; }
        public double Y { get; }
        public int Index1 { get; }
        public int Index2 { get; }

        #region Constructors
        public Detector(double xPosition, double yPosition, double deltaX, double deltaY)
            : this(SpaceTimeDiscretiser.CalculateDiscreteQuantity(xPosition, deltaX), SpaceTimeDiscretiser.CalculateDiscreteQuantity(yPosition, deltaY))
        {
            X = xPosition;
            Y = yPosition;
        }

        public Detector(int index1, int index2)
        {
            Index1 = index1;
            Index2 = index2;
        } 

        public void AddData(double data)
        {
            timeVariation.Add(data);
        }
        #endregion
    }
}
