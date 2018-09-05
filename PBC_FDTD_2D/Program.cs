using System.Collections.Generic;
using PBC_FDTD_2D.Fields;
using PBC_FDTD_2D.Structures;
using PBC_FDTD_2D.Utilities;
using MathsAndPhysics;

using static System.Console;

using static MathsAndPhysics.PhysicalConstants;
using static DataLayer.FileManager;
using static MathsAndPhysics.SpaceTimeDiscretiser;
using System;
using System.Diagnostics;
using PBC_FDTD_2D.Messaging;
using static MathsAndPhysics.MagnitudeOrders;

namespace PBC_FDTD_2D
{
    class Program
    {
        static void Main(string[] args)
        {
            var dx = 0.58652 * u / 40.0;
            var dy = 0.58652 * u / 40.0;
            //calculating the time step using Corant Limit formula
            var dt = 0.9 * CalculateTimeStepUsingCourantLimit(dx, dy);
            var discretisationInfo = new DiscretisationInfo(dx, dy, dt);

            var unitCellDetails = new UnitCellDetails(
                xPeriod: 0.58652 * u,
                yPeriod: 0.58652 * u,
                dielectricBackground: Eps0,
                rodholeDielectric: 3.4 * 3.4 * Eps0,
                rodHoleRadius: 0.2 * 0.58652 * u,
                discretisationInfo: discretisationInfo);

            
            //creating the structure to be simulated
            var structure = CreateAndSaveStructure(unitCellDetails, discretisationInfo);

            //creating the source to be used to excite the structure
            int timeSteps = 200000;
            Source source = CreateSource(discretisationInfo, timeSteps);

            var EField = new ComplexField(structure.XCells, structure.YCells);
            var HField = new ComplexField(structure.XCells, structure.YCells);
            var fdtdPropagator = new FDTDPropagator(discretisationInfo, EField, HField, structure);
            fdtdPropagator.OnMultipleReached += (t) => Messenger.SimulationTimeInfos(timeSteps, t);
            var detector1 = new Detector(20, 20);
            var detector2 = new Detector(unitCellDetails.XPeriod / 3.0, unitCellDetails.YPeriod * 0.8, discretisationInfo.Dx, discretisationInfo.Dy);
            var detectors = new List<Detector> { detector1, detector2 };

            WriteLine();
            Messenger.PrintSimulationRunDetails(timeSteps, timeSteps * discretisationInfo.Dt);
            var watch = Stopwatch.StartNew();
            fdtdPropagator.Propagate(timeSteps, source, detectors);
            Messenger.ElapsedTime(watch);
            WriteLine();

            SaveDetectors(detectors);

            WriteLine("Press Enter to terminate");
            ReadLine();
        }

        #region Helper Methods
        private static void SaveDetectors(List<Detector> detectors)
        {
            WriteLine("Saving the detectors");
            for (int index = 0; index < detectors.Count; index++)
            {
                string filename = @"D:\data\detector" + (index + 1) + ".dat";
                WriteLine("Detector" + (index + 1) + " in " + filename);
                Save(detectors[index].TimeVariation, filename);
            }
        }


        private static IStructure CreateAndSaveStructure(UnitCellDetails unitCellDetails, DiscretisationInfo discretisationInfo)
        {
            var structure = new Structure(unitCellDetails, discretisationInfo);
            string structureFilename = @"D:\data\structureCss.csv";
            WriteLine("Saving the structure in " + structureFilename);
            Save(structure.Permittivity, structure.XCells, structure.YCells, structureFilename);
            return structure;
        }

        private static Source CreateSource(DiscretisationInfo discretisationInfo, int timeSteps)
        {
            Func<double, double> timeFunction = GetTimeFunction();
            var sourceFunc = new Source(discretisationInfo.Dt, timeSteps, timeFunction);
            //filename = @"D:\data\sourceCss.dat";
            //WriteLine("Saving the source in " + filename);
            //Save(source.TimeVariation, filename);
            return sourceFunc;
        }

        private static Func<double, double> GetTimeFunction()
        {
            double timeWidth = 15 * f;
            double timeDelay = 3.0 * timeWidth;
            double centralFrequency = C0 / 1.5 * u;

            return (time) =>
            {
                //double sinTerm = Math.Sin(2.0 * Math.PI * centralFrequency * time);
                var sinCarrier = MathFunctions.SinusoidalCarrier(centralFrequency);
                var gaussFunction = MathFunctions.GaussianBell(timeDelay, timeWidth);
                return sinCarrier(time) * gaussFunction(time);
            };
        }
        #endregion
    }
}
