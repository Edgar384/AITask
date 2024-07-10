using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollower : MonoBehaviour
{
    public List<Vector3> path;
    private int currentWaypointIndex = 0;

    public void SetPath(List<Vector3> newPath)
    {
        path = newPath;
        currentWaypointIndex = 0;
    }

    public Vector3 GetNextWaypoint()
    {
        if (currentWaypointIndex < path.Count)
        {
            return path[currentWaypointIndex];
        }
        return transform.position;
    }

    public void AdvanceToNextWaypoint()
    {
        currentWaypointIndex++;
    }

    public bool ReachedEndOfPath()
    {
        return currentWaypointIndex >= path.Count;
    }
}
