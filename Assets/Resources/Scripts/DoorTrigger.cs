using UnityEngine;
using System.Collections;

public class DoorTrigger : MonoBehaviour {

    public GameObject door;

    void OnTriggerEnter2D()
    {
        door.transform.rotation = Quaternion.AngleAxis(90, Vector3.forward);
    }

    void OnTriggerExit2D()
    {
        door.transform.rotation = Quaternion.AngleAxis(0, Vector3.forward);
    }
}
