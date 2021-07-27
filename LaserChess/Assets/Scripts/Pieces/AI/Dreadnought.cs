using System.Collections.Generic;
using UnityEngine;

public class Dreadnought : EnemyPiece
{
    protected override void Awake()
    {
        base.Awake();

        this.OnAttackComplete += Drone_OnAttackComplete;
    }

    private void Drone_OnAttackComplete(Piece piece)
    {
        piece.InvokeOnTurnComplete();
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

        List<Coord> possibleDestinations = this.FindPossibleDestinations();

        if (possibleDestinations != null && possibleDestinations.Count > 0)
        {
            destination = this.PickClosestToEnemyPosition(possibleDestinations);
        }

        return destination;
    }

    private Coord PickClosestToEnemyPosition(List<Coord> possibleDestinations)
    {
        Coord position = null;

        float lowestDistance = float.MaxValue;

        for (int i = 0; i < possibleDestinations.Count; i++)
        {
            Coord newPosition = possibleDestinations[i];
            Vector3 targetPosition = BoardManager.Instance.GetTileCenter(newPosition.X, newPosition.Y);

            float distance = this.GetDistanceBetweenPositions(this.sensor.ClosestEnemyPiece.transform.position, targetPosition);

            if (distance < lowestDistance)
            {
                lowestDistance = distance;
                position = newPosition;
            }
        }

        return position;
    }

    public override bool[,] PossibleMoves()
    {
        bool[,] r = new bool[PathfindingHelper.GRID_SIZE, PathfindingHelper.GRID_SIZE];
        PathfindingHelper.CheckAll(ref r, CurrentX, CurrentY, Speed);
        return r;
    }
}