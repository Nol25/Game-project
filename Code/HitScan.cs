using UnityEngine;


public class HitScan : MonoBehaviour
{
    public Transform raycastFrom;
    Ray ray;
    RaycastHit hitInfo;

    void Update()
    {
        ray.origin = raycastFrom.position;
        ray.direction = raycastFrom.forward;

        if (Physics.Raycast(ray, out hitInfo))
        {
            Vector3 pos = hitInfo.point;
            transform.position = hitInfo.point;
        }
        else
        {
            transform.localPosition = new Vector3(0, 0, 20);
        }
    }
}

