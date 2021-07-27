public class Drone : EnemyPiece
{
    protected override void Awake()
    {
        base.Awake();
        this.OnAttackComplete += Drone_OnAttackComplete;
    }

    private void Drone_OnAttackComplete(Piece obj)
    {
        obj.InvokeOnTurnComplete();
    }

    protected override void ExecuteTurn()
    {
        this.sensor.DetectClosestTarget();
        this.TryMove();
        base.sensor.DetectPossibleAttackTargets();
        this.Attack();
    }

    protected override Coord TryFindDestination()
    {
        Coord destination = null;

        bool[,] allowedMoves = this.PossibleMoves();

        int destX = 0;
        int destY = int.MaxValue;

        for (int x = 0; x < allowedMoves.GetLength(0); x++)
        {
            for (int y = 0; y < allowedMoves.GetLength(1); y++)
            {
                if (allowedMoves[x, y])
                {
                    if (y < destY)
                    {
                        destX = x;
                        destY = y;
                    }
                }
            }
        }

        if (destY != int.MaxValue)
        {
            destination = new Coord(destX, destY);
        }

        return destination;
    }

    public override bool[,] PossibleMoves()
    {
        bool[,] r = new bool[PathfindingHelper.GRID_SIZE, PathfindingHelper.GRID_SIZE];
        PathfindingHelper.CheckDown(ref r, CurrentX, CurrentY, Speed);
        return r;
    }
}
