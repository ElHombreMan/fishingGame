using UnityEngine;

public class FollowTargetUI : MonoBehaviour
{
    public RectTransform target;

    void Update()
    {
        if (target != null)
        {
            transform.position = target.position;
        }
    }
}
