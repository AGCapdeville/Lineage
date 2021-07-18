using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public InputAction wasd;
    public InputAction shift;
    public float rotationSpeed = 175f;
    public float movementSpeed = 1;


    // Referemces 
    public CharacterController controller;
    private Animator animator;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
    }

    void OnEnable()
    {
        wasd.Enable();
    }

    void OnDisable()
    {
        wasd.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 inputVector = wasd.ReadValue<Vector2>();

        Vector3 movementVector = new Vector3();

        movementVector.x = inputVector.x;
        movementVector.z = inputVector.y;

        controller.Move(movementVector * Time.deltaTime * 3.14f * movementSpeed);

        if (movementVector != Vector3.zero)
        {
            walk(movementVector);
        }
        else
        {
            idle();
        }
    }


    private void idle()
    {
        animator.SetFloat("Speed", 0, 0.1f, Time.deltaTime);     
    }
    private void walk(Vector3 movementVector)
    {
        Quaternion toRotation = Quaternion.LookRotation(movementVector, Vector3.up);
        controller.transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        animator.SetFloat("Speed", 0.5f, 0.1f, Time.deltaTime);
    }
    private void charge(Vector3 movementVector)
    {
        Quaternion toRotation = Quaternion.LookRotation(movementVector, Vector3.up);
        controller.transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        animator.SetFloat("Speed", 1f);
    }
}

