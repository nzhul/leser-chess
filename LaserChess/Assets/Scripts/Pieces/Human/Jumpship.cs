public class Jumpship : HumanPiece
{
    public override bool[,] PossibleMoves()
    {
        bool[,] r = new bool[PathfindingHelper.GRID_SIZE, PathfindingHelper.GRID_SIZE];
        PathfindingHelper.CheckKnight(ref r, CurrentX, CurrentY, Speed);
        return r;
    }
}