using UnityEngine;

//Dictates camera movement
public class CameraMovement : MonoBehaviour {

    public Transform player;
    [SerializeField] float leftLimit;
    [SerializeField] float rightLimit;
    [SerializeField] float bottomLimit;
    [SerializeField] float topLimit;

    //Updates the camera to follow the player
    void Update ()
    {
        transform.position = player.transform.position + new Vector3(0, 1, -5);

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, leftLimit, rightLimit), Mathf.Clamp(transform.position.y, bottomLimit, topLimit), -5);
    }
}
