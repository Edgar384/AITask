using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollower : MonoBehaviour
{

    public float speed = 5f;
    public float turnSpeed = 5f;
    public float stoppingDistance = 1f;
    private Grid grid;
    private int targetIndex;
    private List<NodeGrid> path;

    void Start()
    {
        grid = GameObject.FindObjectOfType<Grid>();
        path = grid.path;
        if (path != null && path.Count > 0)
        {
            targetIndex = 0;
            StartCoroutine(FollowPath());
        }
    }

    IEnumerator FollowPath()
    {
        while (true)
        {
            if (path == null || path.Count == 0) yield break;

            Vector3 targetPosition = path[targetIndex].worldPosition;
            targetPosition.y = transform.position.y;

            while (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
            {
                Vector3 direction = (targetPosition - transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, turnSpeed * Time.deltaTime);
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
                yield return null;
            }

            targetIndex++;
            if (targetIndex >= path.Count)
            {
                yield break;
            }
        }
    }
}
