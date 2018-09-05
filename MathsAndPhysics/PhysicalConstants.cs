using System;

namespace MathsAndPhysics
{
    public static class PhysicalConstants
    {
        /// <summary>
        /// Vacuum permeability
        /// </summary>
        public static double Mi0 { get; } = 1.256637061e-6;

        /// <summary>
        /// Vacuum permittivity
        /// </summary>
        public static double Eps0 { get; } = 8.8541878176e-12;

        /// <summary>
        /// Speed of light in vacuum
        /// </summary>
        public static double C0 { get; } = Math.Sqrt(1.0 / (Mi0 * Eps0));
    }
}
