using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TestSpherCast : MonoBehaviour
{
    [SerializeField] Transform SolidCollider;
    [SerializeField] Transform TriggerCollider;
    [SerializeField] float radius;
    protected RaycastHit _rayCastHit;
    // Start is called before the first frame update
    void Start()
    {
        if (Physics.SphereCast(SolidCollider.position + (Vector3.up * 5.0f), radius, Vector3.down, out _rayCastHit, 6.0f, LayerMask.GetMask("Test")))
        {
            Debug.Log("choque con el collider solido");
        }
        else
        {
            Debug.Log("No choque con el collider solido");
        }
        if (Physics.SphereCast(TriggerCollider.position + (Vector3.up * 5.0f), radius, Vector3.down, out _rayCastHit, 6.0f, LayerMask.GetMask("Test")))
        {
            Debug.Log("choque con el collider Trigger");
        }
        else
        {
            Debug.Log("No choque con el collider Trigger");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
