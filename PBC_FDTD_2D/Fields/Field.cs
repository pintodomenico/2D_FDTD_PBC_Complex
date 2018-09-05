namespace PBC_FDTD_2D.Fields
{
    public class Field
    {
        public double[,] x;
        public double[,] y;
        public double[,] z;

        public int XCells { get; }
        public int YCells { get;  }

        public Field(int xCells, int yCells)
        {
            XCells = xCells;
            YCells = yCells;

            x = new double[XCells, YCells];
            y = new double[XCells, YCells];
            z = new double[XCells, YCells];

            InitialiseFieldWithZeros();
        }

        private void InitialiseFieldWithZeros()
        {
            for (int xIndex = 0; xIndex < XCells; xIndex++)
            {
                for (int yIndex = 0; yIndex < YCells; yIndex++)
                {
                    x[xIndex, yIndex] = 0.0;
                    y[xIndex, yIndex] = 0.0;
                    z[xIndex, yIndex] = 0.0;
                }
            }
        }
    }
}
