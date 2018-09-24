using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathMovement : MonoBehaviour {

    public float moveSpeed;

    Coroutine pathCoroutine;
    Coroutine moveCoroutine;

    public AStarPath CurrentPath
    {
        get; private set;
    }

    public Tile DestinationTile
    {
        get; private set;
    }

    public Tile CurrentTile
    {
        get; private set;

    }

    public Tile NextTile
    {
        get; private set;
    }

    public void Init(Tile currentTile)
    {
        this.CurrentTile = currentTile;
        transform.position = new Vector3(currentTile.Position.x, currentTile.Position.y);
    }

    public void MoveTo(Tile dest, Action onPathEnd = null)
    {
        if(dest == null)
        {
            return;
        }

        this.DestinationTile = dest;

        if (pathCoroutine != null)
        {
            StopCoroutine(pathCoroutine);
        }

        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }

        pathCoroutine = StartCoroutine(PathCoroutine(dest, onPathEnd));
    }

    IEnumerator PathCoroutine(Tile dest, Action onPathEnd)
    {
        CurrentPath = MapFactory.Instance.MapGrid.GetPath(CurrentTile, dest);


        for(int i = 0; i < CurrentPath.ValidPath.Count - 1; i++)
        {
            Tile t1 = CurrentPath.ValidPath[i].Destination;
            Tile t2 = CurrentPath.ValidPath[i + 1].Destination;

            Debug.DrawLine(new Vector3(t1.Position.x, t1.Position.y), new Vector3(t2.Position.x, t2.Position.y), Color.green, 100f);
        }

        Debug.Log(CurrentTile.Position + " " + dest.Position);

        string path = "";

        foreach(TileConnection tc in CurrentPath.ValidPath)
        {
            path += tc.Destination.Position + " ";
        }

        Debug.Log(path);

        foreach(TileConnection tc in CurrentPath.ValidPath) {

            this.NextTile = tc.Destination;

            moveCoroutine = StartCoroutine(MoveCoroutine(tc.MoveCost));
            yield return moveCoroutine;

            CurrentTile = NextTile;
        }

        NextTile = null;

        CurrentPath = null;

        if(onPathEnd != null)
        {
            onPathEnd();
        }
    }

    IEnumerator MoveCoroutine(float moveCost)
    {
        float totalDist = Vector3.Distance(transform.position, new Vector3(NextTile.Position.x, NextTile.Position.y, 0f));
        Vector3 direction = new Vector3(NextTile.Position.x - transform.position.x, NextTile.Position.y - transform.position.y, 0f).normalized;
        
        while(totalDist > 0) {
            float frameDist = Time.deltaTime * (moveSpeed / moveCost);

            totalDist -= frameDist;

            transform.position += frameDist * direction;

            yield return null;
        }
    }
}
