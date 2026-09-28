using UnityEngine;

public class CameraController : MonoBehaviour
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void MoveCameraY(float dist)
    {
        transform.position += new Vector3(0, dist, 0);
    }
}
