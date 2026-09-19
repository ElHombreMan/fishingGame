using UnityEngine;

public class FishingLine : MonoBehaviour
{
    public Transform rodTip;
    public Transform bobber;
    private LineRenderer line;

    void Start()
    {
        line = GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (rodTip != null && bobber != null)
        {
            line.SetPosition(0, rodTip.position);
            line.SetPosition(1, bobber.position);
        }
    }
}
