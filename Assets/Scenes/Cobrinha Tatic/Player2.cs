using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2 : MonoBehaviour
{
    public float stepDuration = 0.2f;
    public int delayNodes = 1;

    public void FollowPath(List<Node> path)
    {
        StopAllCoroutines();
        StartCoroutine(Follow(path));
    }

    IEnumerator Follow(List<Node> path)
    {
        for (int i = 0; i < path.Count; i++)
        {
            int targetIndex = i - delayNodes;

            if (targetIndex < 0)
            {
                yield return new WaitForSeconds(stepDuration);
                continue;
            }

            Vector3 start = transform.position;
            Vector3 end = path[targetIndex].transform.position;

            float t = 0;

            while (t < 1)
            {
                t += Time.deltaTime / stepDuration;
                transform.position = Vector3.Lerp(start, end, t);
                yield return null;
            }
        }
    }
}