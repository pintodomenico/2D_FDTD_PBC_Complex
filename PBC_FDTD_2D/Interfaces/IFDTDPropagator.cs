using System.Collections.Generic;
using PBC_FDTD_2D.Structures;


using PBC_FDTD_2D.Fields;

namespace PBC_FDTD_2D.Interfaces
{
    public interface IFDTDPropagator
    {
        ComplexField EField { get; }
        ComplexField HField { get; }
        IStructure Structure { get; }

        void Propagate(int numberOfTimeSteps, Source source, List<Detector> detectors);
    }
}