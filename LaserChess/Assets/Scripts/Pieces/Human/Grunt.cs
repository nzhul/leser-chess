public class Grunt : HumanPiece
{
    public override bool[,] PossibleMoves()
    {
        bool[,] r = new bool[PathfindingHelper.GRID_SIZE, PathfindingHelper.GRID_SIZE];
        PathfindingHelper.CheckRook(ref r, CurrentX, CurrentY, Speed);
        return r;
    }
}