using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float stepDuration = 0.2f;

    public void MoveAlongPath(List<Node> path, System.Action onFinish = null)
    {
        StopAllCoroutines();
        StartCoroutine(Move(path, onFinish));
    }

    IEnumerator Move(List<Node> path, System.Action onFinish)
    {
        foreach (Node node in path)
        {
            Vector3 start = transform.position;
            Vector3 end = node.transform.position;
            float t = 0;

            while (t < 1f)
            {
                t += Time.deltaTime / stepDuration;
                transform.position = Vector3.Lerp(start, end, Mathf.Clamp01(t));
                yield return null;
            }
        }

        onFinish?.Invoke();
    }
}
