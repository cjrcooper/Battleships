namespace Battleships
{
    public class BoardBlock
    {
        private int Row { get; }
        private int Column { get; }
        public string MarkerType { get; set; }

        public BoardBlock(int row, int column)
        {
            Row = row;
            Column = column;
            MarkerType = "E";
        }
    }
}