using UnityEngine;

public class ToggleRagdoll : MonoBehaviour
{
    [SerializeField] BoxCollider bcollider;
    [SerializeField] Rigidbody rb;
    [SerializeField] Animator animator;
    [SerializeField] Event onDeathEvent;
    Rigidbody[] rbs;
    Collider[] colliders;
    public bool isRagdoll;

    void Start()
    {
        rbs = GetComponentsInChildren<Rigidbody>();
        colliders = GetComponentsInChildren<Collider>();
        onDeathEvent?.Subscribe(EnableRagdoll);
        //onGameStartEvent?.Subscribe(OnGameStart);
        DisableRagdoll();
    }

    void Update()
    {
        //if (isRagdoll)
        //{
        //    EnableRagdoll();
        //}
        //else
        //{
        //    DisableRagdoll();
        //}
       
    }

    private void EnableRagdoll()
    {
        animator.enabled = false;

        foreach (Collider c in colliders)
        {
            c.enabled = true;
        }
        foreach (var r in rbs)
        {
            r.isKinematic = false;
        }

        bcollider.enabled = false;
        rb.isKinematic = true;
        Debug.Log($"{isRagdoll}, Ragdoll on");
    }
    private void DisableRagdoll()
    {
        //animator.enabled = true;
        //bcollider.enabled = true;
        //rb.isKinematic = false;


        foreach (Collider c in colliders)
        {
            c.enabled = false;
        }
        foreach (var r in rbs)
        {
            r.isKinematic = true;
        }

        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<BoxCollider>().enabled = true;
        GetComponent<Animator>().enabled = true;

        Debug.Log($"{isRagdoll}, Ragdoll off");
    }
}
