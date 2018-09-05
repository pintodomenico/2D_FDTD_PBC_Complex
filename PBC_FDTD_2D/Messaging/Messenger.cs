using System;
using System.Diagnostics;

namespace PBC_FDTD_2D.Messaging
{
    public static class Messenger
    {
        public static void PrintSimulationRunDetails(int numberOfTimeSteps, double totalTimeSimulated)
        {
            Console.WriteLine("Simulation started");
            Console.WriteLine("Total number of time steps to be executed: {0}", numberOfTimeSteps);
            Console.WriteLine("Equivalent to {0} s of simulated time", totalTimeSimulated.ToString("E4"));
        }

        public static void ElapsedTime(Stopwatch watch)
        {
            var elapsedTime = watch.ElapsedMilliseconds;
            Console.WriteLine();
            Console.WriteLine("Elapsed time: {0} ms", elapsedTime);

            int hours = (int)(elapsedTime / (1000 * 60 * 60));
            elapsedTime = elapsedTime - (hours * 1000 * 60 * 60);
            int minutes = (int)(elapsedTime / (1000 * 60));
            elapsedTime = elapsedTime - (minutes * 1000 * 60);
            int seconds = (int)(elapsedTime / 1000);
            Console.WriteLine("Elapsed time hours: {0} minutes: {1} seconds: {2}", hours, minutes, seconds);
        }

        public static void SimulationTimeInfos(int numberOfTimeSteps, int t)
        {
            long reminder;
            long bigStepForVisualisation = 10000;
            long smallStepForVisualisation = 1000;
            double tiny = 1.0e-9;
            long result = Math.DivRem(t, bigStepForVisualisation, out reminder);
            if (Math.Abs(reminder) < tiny)
                Console.Write("\n Step number: {0} of {1}", t, numberOfTimeSteps);
            result = Math.DivRem(t, smallStepForVisualisation, out reminder);
            if (Math.Abs(reminder) < tiny)
            {
                Console.Write(".");
            }
        }
    }
}
