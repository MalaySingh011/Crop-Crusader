using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigidbody;
    public GameObject waypoint1;
    public GameObject waypoint2;

    void Start()
    {
        WalkNeg();
    }

    void Update()
    {
        float xpos = Mathf.Round(transform.position.x * 100.0f) * .01f;
        float xpos1 = Mathf.Round(waypoint1.transform.position.x * 100.0f) * .01f;
        float xpos2 = Mathf.Round(waypoint2.transform.position.x * 100.0f) * .01f;

        if (Mathf.Approximately(xpos, xpos1))
        {
            rigidbody.linearVelocity = new Vector2(0, 0);
            Invoke("WalkPos", 2f);
        }
        if (Mathf.Approximately(xpos, xpos2))
        {
            rigidbody.linearVelocity = new Vector2(0, 0);
            Invoke("WalkNeg", 2f);
        }
    }

    void WalkPos()
    {
        rigidbody.linearVelocity = new Vector2(1, 0);
    }

    void WalkNeg()
    {
        rigidbody.linearVelocity = new Vector2(-1, 0);
    }
}
