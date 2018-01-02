using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class CharController : MonoBehaviour
{
    Rigidbody rb;
    public float speed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        
    }
    void Update()
    {

    }

    private void FixedUpdate()
    {
        float horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
        float vertical = CrossPlatformInputManager.GetAxis("Vertical");

        rb.velocity = new Vector3(vertical, 0f, -horizontal);
    }
}
