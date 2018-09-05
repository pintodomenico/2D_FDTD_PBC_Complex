namespace PBC_FDTD_2D.Fields
{
    public class ComplexField
    {
        public Field real;
        public Field imag;

        public ComplexField(int xNoOfCells, int yNoOfCells)
        {
            real = new Field(xNoOfCells, yNoOfCells);
            imag = new Field(xNoOfCells, yNoOfCells);
        }
    }
}
