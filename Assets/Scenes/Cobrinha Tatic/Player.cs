using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public void MoveAlongPath(List<Node> path, System.Action onFinish = null)
    {
        StopAllCoroutines(); // evita múltiplos movimentos
        StartCoroutine(Move(path, onFinish));
    }

    public float speed = 3f;
    public float stepDuration = 0.2f; // tempo por node

    IEnumerator Move(List<Node> path, System.Action onFinish)
    {
        foreach (Node node in path)
        {
            Vector3 start = transform.position;
            Vector3 end = node.transform.position;

            float t = 0;

            while (t < 1)
            {
                t += Time.deltaTime / stepDuration;
                transform.position = Vector3.Lerp(start, end, t);
                yield return null;
            }
        }

        onFinish?.Invoke();
    }
}