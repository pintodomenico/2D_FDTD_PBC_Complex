using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace PBC_FDTD_2D.Tests
{
    [TestClass()]
    public class SourceTests
    {
        private double tolerance = 1.0e-9;

        [TestMethod()]
        public void SourceWithFuncTest()
        {
            double timeDelay = 1.0;
            double timeWidth = 2.0;
            double centralFrequency = 1.0;
            double deltaT = 0.05;
            int timeSteps = 5000;

            Func<double, double> timeFunction = (time) =>
            {
                double sinTerm = Math.Sin(2.0 * Math.PI * centralFrequency * time);
                double exponentialTerm = Math.Exp(-Math.Pow((time - timeDelay) / timeWidth, 2.0));
                return sinTerm * exponentialTerm;
            };
            var sourceFunc = new Source(deltaT, timeSteps, timeFunction);

            for (int timeStep = 0; timeStep < timeSteps; timeStep++)
            {
                Assert.AreEqual(timeFunction(timeStep*deltaT), sourceFunc.GetTimeValue(timeStep), tolerance, "Test array with Func value");
            }
        }
    }
}