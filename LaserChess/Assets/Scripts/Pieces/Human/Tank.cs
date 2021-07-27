public class Tank : HumanPiece
{
    public override bool[,] PossibleMoves()
    {
        bool[,] r = new bool[PathfindingHelper.GRID_SIZE, PathfindingHelper.GRID_SIZE];
        PathfindingHelper.CheckAll(ref r, CurrentX, CurrentY, Speed);
        return r;
    }
}
