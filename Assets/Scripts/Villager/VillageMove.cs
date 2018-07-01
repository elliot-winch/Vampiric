using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageMove : MonoBehaviour {

    public float moveSpeed;

    Coroutine pathCoroutine;
    Coroutine moveCoroutine;
    AStarPath currentPath;
    Tile currentTile;
    Tile nextTile;
    Tile destinationTile;

    public AStarPath CurrentPath
    {
        get
        {
            return currentPath;
        }
    }

    public Tile DestinationTile
    {
        get
        {
            return destinationTile;
        }
    }

    public Tile CurrentTile
    {
        get
        {
            return currentTile;
        }
    }

    public Tile NextTile
    {
        get
        {
            return nextTile;
        }
    }

    public void Init(Tile currentTile)
    {
        this.currentTile = currentTile;
        transform.position = new Vector3(currentTile.Position.x, currentTile.Position.y);
    }

    public void MoveTo(Tile dest, Action onPathEnd = null)
    {
        this.destinationTile = dest;

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
        currentPath = MapManager.Instance.GetPath(currentTile, dest);

        Tile next;

        while (currentPath.IsNextTile())
        {
            next = currentPath.GetNextTile();

            this.currentTile = this.nextTile;
            this.nextTile = next;

            moveCoroutine = StartCoroutine(MoveCoroutine());
            yield return moveCoroutine;
        }

        currentPath = null;

        if(onPathEnd != null)
        {
            onPathEnd();
        }
    }

    IEnumerator MoveCoroutine()
    {
        float dist = Vector3.Distance(transform.position, new Vector3(nextTile.Position.x, nextTile.Position.y, 0f));
        Vector3 direction = new Vector3(nextTile.Position.x - transform.position.x, nextTile.Position.y - transform.position.y, 0f);
        
        do {
            float d = Time.deltaTime * (moveSpeed / MapManager.Instance.MoveCosts[nextTile]);

            dist -= d;

            transform.position += d * direction.normalized;

            yield return null;
        } while (dist >= 0f);

        this.currentTile = nextTile;
    }
}
