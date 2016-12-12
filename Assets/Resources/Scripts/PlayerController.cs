using UnityEngine;

public class PlayerController : MonoBehaviour {

    private float INTERACT_DISTANCE = 1f;
    private float MOVEMENT_SPEED = 4.5f; // per second
    private Vector3 faceDir;
    private Interactable currentActive;
    public GameManager gm;

    // Use this for initialization
    void Start () {
        faceDir = new Vector2(0, 1f);
    }
	
	// Update is called once per frame
	void Update () {
        if (!GameGlobalState.Get().IsPlaying)
        {
            return;
        }

        RaycastHit2D hits = Physics2D.Raycast(transform.position, faceDir, INTERACT_DISTANCE, 1 << LayerMask.NameToLayer("Objects"));
        //Debug.DrawLine(transform.position, transform.position + (faceDir * INTERACT_DISTANCE), Color.green);
        if (hits)
        {
            Interactable interactable = hits.transform.gameObject.GetComponent<Interactable>();
            if (interactable != null)
            {
                if (interactable is GameObjective) {
                    if (gm.isActiveObjective(interactable as GameObjective) && interactable != currentActive)
                    {
                        if (currentActive != null)
                        {
                            currentActive.DeactivateObj();
                        }
                        currentActive = interactable;
                        currentActive.ActivateObj();
                    }
                } else
                {
                    if (currentActive != null)
                    {
                        currentActive.DeactivateObj();
                    }
                    currentActive = interactable;
                    currentActive.ActivateObj();
                }
            }
            //Debug.Log("hit " + hits.transform.gameObject.name);
        } else
        {
            if (currentActive != null)
            {
                currentActive.DeactivateObj();
                currentActive = null;
            }
        }
    }

    void FixedUpdate()
    {
        if (!GameGlobalState.Get().IsPlaying)
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            return;
        }

        float dx = Input.GetAxis("Horizontal");
        float dy = Input.GetAxis("Vertical");
        GetComponent<Rigidbody2D>().velocity = new Vector2(dx * MOVEMENT_SPEED, dy * MOVEMENT_SPEED);
        if (Mathf.Abs(dx) > float.Epsilon || Mathf.Abs(dy) > float.Epsilon)
        {
            faceDir = new Vector2(dx, dy);
            transform.rotation = Quaternion.LookRotation(Vector3.forward, faceDir);
        }
    }
}
