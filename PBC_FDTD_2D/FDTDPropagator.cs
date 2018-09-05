using System.Collections.Generic;
using PBC_FDTD_2D.Utilities;
using PBC_FDTD_2D.Interfaces;
using PBC_FDTD_2D.Fields;
using PBC_FDTD_2D.Structures;
using MathsAndPhysics;

namespace PBC_FDTD_2D
{
    public class FDTDPropagator : IFDTDPropagator
    {
        public delegate void dgEventRaiser(int t);
        public event dgEventRaiser OnMultipleReached;

        private readonly DiscretisationInfo discretisationInfo;

        public ComplexField EField { get; }
        public ComplexField HField { get; }
        public IStructure Structure { get; }

        public FDTDPropagator(DiscretisationInfo discretisationInfo, ComplexField EField, ComplexField HField, IStructure structure)
        {
            this.discretisationInfo = discretisationInfo;
            this.EField = EField;
            this.HField = HField;
            Structure = structure;
        }

        public void Propagate(int numberOfTimeSteps, Source source, List<Detector> detectors)
        {
            Point3D shiftCos = new Point3D(0, 0, 0);
            Point3D shiftSin = new Point3D(0, 0, 0);

            for (int t = 0; t < numberOfTimeSteps; t++)
            {

                UpdatePeriodicBoundariesForHField(shiftCos, shiftSin);
                UpdateHField();

                UpdatePeriodicBoundariesForEField(shiftCos, shiftSin);
                UpdateEField();

                ApplySource(source, t);

                UpdateDetecors(detectors);


                //Ex-Hy-Hz POLARISATION	
                //------------------------------------------------------------------------------------------------------------------------------------------//
                //------------------------------------------------------------------------------------------------------------------------------------------//
                //------------------------------------------------------------------------------------------------------------------------------------------//
                //BLOCK STOP

                MultipleOf(t, 1000);

            }//for(t=0;t<nt;t++)
        }

        #region Private Methods for Magnetic field H update
        private void UpdatePeriodicBoundariesForHField(Point3D shiftCos, Point3D shiftSin)
        {
            int zCells = Structure.XCells;
            int yCells = Structure.YCells;
            double hty = (discretisationInfo.Dt / PhysicalConstants.Mi0) / discretisationInfo.Dy;
            double htz = (discretisationInfo.Dt / PhysicalConstants.Mi0) / discretisationInfo.Dx;
            //BLOCK START
            //------------------------------------------------------------------------------------------------------------------------------------------//
            //------------------------------------------------------------------------------------------------------------------------------------------//
            //------------------------------------------------------------------------------------------------------------------------------------------//
            //Ex-Hy-Hz POLARISATION	

            //modifico le equazioni con l'approccio con i numeri complessi

            HField.real.y[zCells - 1, yCells - 1] = HField.real.y[zCells - 1, yCells - 1] +
                (htz * (EField.real.x[zCells - 1, yCells - 1] - (EField.real.x[0, yCells - 1] * shiftCos.z - EField.imag.x[0, yCells - 1] * shiftSin.z)));

            HField.real.z[zCells - 1, yCells - 1] = HField.real.z[zCells - 1, yCells - 1] +
                (hty * ((EField.real.x[zCells - 1, 0] * shiftCos.y - EField.imag.x[zCells - 1, 0] * shiftSin.y) - EField.real.x[zCells - 1, yCells - 1]));

            HField.imag.y[zCells - 1, yCells - 1] = HField.imag.y[zCells - 1, yCells - 1] +
                (htz * (EField.imag.x[zCells - 1, yCells - 1] - (EField.real.x[0, yCells - 1] * shiftSin.z + EField.imag.x[0, yCells - 1] * shiftCos.z)));

            HField.imag.z[zCells - 1, yCells - 1] = HField.imag.z[zCells - 1, yCells - 1] +
                (hty * ((EField.real.x[zCells - 1, 0] * shiftSin.y + EField.imag.x[zCells - 1, 0] * shiftCos.y) - EField.imag.x[zCells - 1, yCells - 1]));

            for (int k = 0; k < (zCells - 1); k++)
            {
                HField.real.y[k, yCells - 1] = HField.real.y[k, yCells - 1] + (htz * (EField.real.x[k, yCells - 1] - EField.real.x[k + 1, yCells - 1]));

                HField.real.z[k, yCells - 1] = HField.real.z[k, yCells - 1] + (hty * ((EField.real.x[k, 0] * shiftCos.y - EField.imag.x[k, 0] * shiftSin.y) - EField.real.x[k, yCells - 1]));

                HField.imag.y[k, yCells - 1] = HField.imag.y[k, yCells - 1] + (htz * (EField.imag.x[k, yCells - 1] - EField.imag.x[k + 1, yCells - 1]));

                HField.imag.z[k, yCells - 1] = HField.imag.z[k, yCells - 1] + (hty * ((EField.real.x[k, 0] * shiftSin.y + EField.imag.x[k, 0] * shiftCos.y) - EField.imag.x[k, yCells - 1]));
            }//for(k=0;(int)k<(zCells-1);k++)

            for (int j = 0; j < (yCells - 1); j++)
            {
                HField.real.y[zCells - 1, j] = HField.real.y[zCells - 1, j] + (htz * (EField.real.x[zCells - 1, j] - (EField.real.x[0, j] * shiftCos.z - EField.imag.x[0, j] * shiftSin.z)));

                HField.real.z[zCells - 1, j] = HField.real.z[zCells - 1, j] + (hty * (EField.real.x[zCells - 1, j + 1] - EField.real.x[zCells - 1, j]));

                HField.imag.y[zCells - 1, j] = HField.imag.y[zCells - 1, j] + (htz * (EField.imag.x[zCells - 1, j] - (EField.real.x[0, j] * shiftSin.z + EField.imag.x[0, j] * shiftCos.z)));

                HField.imag.z[zCells - 1, j] = HField.imag.z[zCells - 1, j] + (hty * (EField.imag.x[zCells - 1, j + 1] - EField.imag.x[zCells - 1, j]));
            }//for(j=0;(int)j<(yCells-1);j++)
        }

        private void UpdateHField()
        {
            int zCells = Structure.XCells;
            int yCells = Structure.YCells;
            double deltaY = discretisationInfo.Dy;
            double deltaZ = discretisationInfo.Dx;
            //componenti del campo H : Hx, Hy, Hz

            for (int k = 0; k < (zCells - 1); k++)
            {
                for (int j = 0; j < (yCells - 1); j++)
                {
                    double hCoeff1 = (2.0 * Structure.Permeability[k, j] - discretisationInfo.Dt * Structure.MagnCond[k, j]) / (2.0 * Structure.Permeability[k, j] + discretisationInfo.Dt * Structure.MagnCond[k, j]);
                    double hCoeff2 = (2.0 * discretisationInfo.Dt) / (2.0 * Structure.Permeability[k, j] + discretisationInfo.Dt * Structure.MagnCond[k, j]);

                    HField.real.y[k, j] = hCoeff1 * HField.real.y[k, j] + hCoeff2 * (EField.real.x[k, j] - EField.real.x[k + 1, j]) / deltaZ;

                    HField.real.z[k, j] = hCoeff1 * HField.real.z[k, j] + hCoeff2 * (EField.real.x[k, j + 1] - EField.real.x[k, j]) / deltaY;

                    HField.imag.y[k, j] = hCoeff1 * HField.imag.y[k, j] + hCoeff2 * (EField.imag.x[k, j] - EField.imag.x[k + 1, j]) / deltaZ;

                    HField.imag.z[k, j] = hCoeff1 * HField.imag.z[k, j] + hCoeff2 * (EField.imag.x[k, j + 1] - EField.imag.x[k, j]) / deltaY;
                }//for(j=0;(int)j<(yCells-1);j++)
            }//for(k=0;(int)k<(zCells-1);k++)		
        } 
        #endregion

        #region Private Methods for Electric field E update
        private void UpdatePeriodicBoundariesForEField(Point3D ShiftCos, Point3D ShiftSin)
        {
            int zCells = Structure.XCells; //this should disappear after refactoring
            int yCells = Structure.YCells; //this should disappear after refactoring
            double tety = discretisationInfo.Dt / discretisationInfo.Dy; //this should disappear after refactoring
            double tetz = discretisationInfo.Dt / discretisationInfo.Dx; //this should disappear after refactoring
            //componenti del campo E : Ex, Ey, Ez

            EField.real.x[0, 0] = EField.real.x[0, 0] + (tety / (Structure.Permittivity[0, 0])) *
                (HField.real.z[0, 0] - (HField.real.z[0, yCells - 1] * ShiftCos.y + HField.imag.z[0, yCells - 1] * ShiftSin.y)) +
                (tetz / (Structure.Permittivity[0, 0])) * ((HField.real.y[zCells - 1, 0] * ShiftCos.z + HField.imag.y[zCells - 1, 0] * ShiftSin.z) - HField.real.y[0, 0]);

            EField.imag.x[0, 0] = EField.imag.x[0, 0] + (tety / (Structure.Permittivity[0, 0])) *
                (HField.imag.z[0, 0] - (-HField.real.z[0, yCells - 1] * ShiftSin.y + HField.imag.z[0, yCells - 1] * ShiftCos.y)) +
                (tetz / (Structure.Permittivity[0, 0])) * ((-HField.real.y[zCells - 1, 0] * ShiftSin.z + HField.imag.y[zCells - 1, 0] * ShiftCos.z) - HField.imag.y[0, 0]);

            for (int k = 1; k < zCells; k++)
            {
                EField.real.x[k, 0] = EField.real.x[k, 0] + (tety / (Structure.Permittivity[k, 0])) *
                    (HField.real.z[k, 0] - (HField.real.z[k, yCells - 1] * ShiftCos.y + HField.imag.z[k, yCells - 1] * ShiftSin.y)) +
                    (tetz / (Structure.Permittivity[k, 0])) * (HField.real.y[k - 1, 0] - HField.real.y[k, 0]);

                EField.imag.x[k, 0] = EField.imag.x[k, 0] + (tety / (Structure.Permittivity[k, 0])) *
                    (HField.imag.z[k, 0] - (-HField.real.z[k, yCells - 1] * ShiftSin.y + HField.imag.z[k, yCells - 1] * ShiftCos.y)) +
                    (tetz / (Structure.Permittivity[k, 0])) * (HField.imag.y[k - 1, 0] - HField.imag.y[k, 0]);

            }//for(k=1;(int)k<zCells;k++)

            for (int j = 1; j < yCells; j++)
            {
                EField.real.x[0, j] = EField.real.x[0, j] + (tety / (Structure.Permittivity[0, j])) *
                    (HField.real.z[0, j] - HField.real.z[0, j - 1]) + (tetz / (Structure.Permittivity[0, j])) *
                    ((HField.real.y[zCells - 1, j] * ShiftCos.z + HField.imag.y[zCells - 1, j] * ShiftSin.z) - HField.real.y[0, j]);

                EField.imag.x[0, j] = EField.imag.x[0, j] + (tety / (Structure.Permittivity[0, j])) *
                    (HField.imag.z[0, j] - HField.imag.z[0, j - 1]) + (tetz / (Structure.Permittivity[0, j])) *
                    ((-HField.real.y[zCells - 1, j] * ShiftSin.z + HField.imag.y[zCells - 1, j] * ShiftCos.z) - HField.imag.y[0, j]);

            }//for(j=1;(int)j<yCells;j++)
        }

        private void UpdateEField()
        {
            int zCells = Structure.XCells; //this should disappear after refactoring
            int yCells = Structure.YCells; //this should disappear after refactoring
            double Deltay = discretisationInfo.Dy;
            double Deltaz = discretisationInfo.Dx;

            for (int k = 1; k < zCells; k++)
            {
                for (int j = 1; j < yCells; j++)
                {
                    double ECoeff1 = (2.0 * Structure.Permittivity[k, j] - discretisationInfo.Dt * Structure.ElecCond[k, j]) / (2.0 * Structure.Permittivity[k, j] + discretisationInfo.Dt * Structure.ElecCond[k, j]);
                    double ECoeff2 = (2.0 * discretisationInfo.Dt) / (2.0 * Structure.Permittivity[k, j] + discretisationInfo.Dt * Structure.ElecCond[k, j]);

                    //ety=tety/(structure.eps[k,j]);
                    //etz=tetz/(structure.eps[k,j]);
                    //jet=Deltat/(structure.eps[k,j]);

                    EField.real.x[k, j] = ECoeff1 * EField.real.x[k, j] +
                        ECoeff2 * ((HField.real.z[k, j] - HField.real.z[k, j - 1]) / Deltay + (HField.real.y[k - 1, j] - HField.real.y[k, j]) / Deltaz);

                    EField.imag.x[k, j] = ECoeff1 * EField.imag.x[k, j] +
                        ECoeff2 * ((HField.imag.z[k, j] - HField.imag.z[k, j - 1]) / Deltay + (HField.imag.y[k - 1, j] - HField.imag.y[k, j]) / Deltaz);

                }//for(j=1;(int)j<yCells;j++)
            }//for(k=1;(int)k<zCells;k++)  	
        } 
        #endregion

        private void ApplySource(Source source, int t)
        {
            //point sources (soft source technique)
            //EField.real.x[10,21] = EField.real.x[10,21] + source.TimeVariation[t];
            //EField.real.x[15,11] = EField.real.x[15,11] + source.TimeVariation[t];
            //EField.real.x[33,23] = EField.real.x[33,23] + source.TimeVariation[t];
            EField.real.x[10, 21] = EField.real.x[10, 21] + source.GetTimeValue(t);
            EField.real.x[15, 11] = EField.real.x[15, 11] + source.GetTimeValue(t);
            EField.real.x[33, 23] = EField.real.x[33, 23] + source.GetTimeValue(t);

            //source with space profile (soft source technique)
            //InsertSpaceProfileSource(EField, source, zCells, yCells, t);
        }

        private void UpdateDetecors(List<Detector> detectors)
        {
            //updating the detectors
            foreach (var detector in detectors)
            {
                detector.AddData(EField.real.x[detector.Index1, detector.Index2]);
            }
        }

        private void MultipleOf(int timeStep, int multiple)
        {
            if ((timeStep % multiple == 0) && OnMultipleReached != null)
                OnMultipleReached(timeStep);
        }
    }
}
