using System;

namespace PBC_FDTD_2D
{
    public class Source
    {
        private readonly double deltaT;
        private readonly int timeSteps;
        private readonly Func<double, double> timeFunction;

        private readonly double deltaX = 1.0;
        private readonly double deltaY = 1.0;
        private Func<double, double, double> spaceFunction = DefaultSpaceFunction;

        public Source(double deltaT, int timeSteps, Func<double,double> timeFunction)
        {
            //initialisation of time details
            this.deltaT = deltaT;
            this.timeSteps = timeSteps;
            this.timeFunction = timeFunction;
        }

        public Source(
            double deltaT,
            int timeSteps,
            Func<double, double> timeFunction,
            double deltaX,
            double deltaY,
            Func<double,double,double> spaceFunction)
        {
            //initialisation of time details
            this.deltaT = deltaT;
            this.timeSteps = timeSteps;
            this.timeFunction = timeFunction;

            //initialisation of space details
            this.deltaX = deltaX;
            this.deltaY = deltaY;
            this.spaceFunction = spaceFunction;
        }

        public double GetTimeValue(int timeStep) 
            => timeFunction(timeStep * deltaT);

        public double GetTimeValue(double time) 
            => timeFunction(time);

        public double GetSpaceValue(int indexX, int indexY) 
            => spaceFunction(indexX * deltaX, indexY * deltaY);

        public double GetSpaceValue(double x, double y) 
            => spaceFunction(x, y);

        public double GetValue(double time, double x, double y)
            => GetTimeValue(time) * GetSpaceValue(x, y);

        public double GetValue(int timeStep, int indexX, int indexY)
            => GetTimeValue(timeStep) * GetSpaceValue(indexX, indexY);

        private static double DefaultSpaceFunction(double x, double y) 
            => 1.0;
    }
}
