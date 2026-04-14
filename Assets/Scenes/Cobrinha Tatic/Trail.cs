using System.Collections.Generic;
using UnityEngine;

public class Trail : MonoBehaviour
{
    [Header("Tags válidas")]
    public List<string> validTags = new List<string> { "Node" };

    [Header("Config")]
    public int maxSize = 25;

    private List<Transform> trail = new List<Transform>();

    public List<Transform> GetTrail()
    {
        return trail;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!validTags.Contains(other.tag)) return;

        if (trail.Count > 0 && trail[trail.Count - 1] == other.transform)
            return;

        trail.Add(other.transform);

        if (trail.Count > maxSize)
            trail.RemoveAt(0);
    }
}