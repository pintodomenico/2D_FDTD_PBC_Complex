using System;

namespace MathsAndPhysics
{
    public static class MathFunctions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="delay"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        public static Func<double, double> GaussianBell(double delay, double width)
            => (x) => Math.Exp(-Math.Pow((x - delay) / width, 2.0));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="centralFrequency"></param>
        /// <param name="phase"></param>
        /// <returns></returns>
        public static Func<double, double> SinusoidalCarrier(double centralFrequency, double phase = 0.0)
            => (x) => Math.Sin((2.0 * Math.PI * centralFrequency * x) + phase);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valueLeft"></param>
        /// <param name="valueRight"></param>
        /// <param name="delay"></param>
        /// <returns></returns>
        public static Func<double, double> StepFunction(double valueLeft, double valueRight, double delay)
            => (x) => { return x < delay ? valueLeft : valueRight; };

        /// <summary>
        /// 
        /// </summary>
        /// <param name="delay"></param>
        /// <returns></returns>
        public static Func<double, double> RaisedCosine(double delay) => (x) =>
        {
            return x < delay || x > (delay + 2.0 * Math.PI)
            ? 0.0
            : (1.0 + Math.Cos(x - delay + Math.PI)) / 2.0;
        };
    }
}
