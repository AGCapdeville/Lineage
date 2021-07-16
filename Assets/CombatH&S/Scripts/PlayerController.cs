using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private CubeInputActions cubeInputActions;
    public CharacterController controller;

    private InputAction movement;
    private InputAction run;
    private InputAction attack;
    // private Animator animator;
    public float movementSpeed = 2f;
    public float movementSpeedModifier = 1.5f;
    public float rotationSpeed = 175f;


    private Animator[] animators;


    private void Awake()
    {
        cubeInputActions = new CubeInputActions();
        controller = GetComponent<CharacterController>();
        animators = GetComponentsInChildren<Animator>();

//        animators = GetComponentsInChildren<Animator>();

        //animators = GetComponentsInChildren<Animator>();
    }

    private void OnEnable()
    {
        movement = cubeInputActions.Player.Movement;
        movement.Enable();

        run = cubeInputActions.Player.Run;
        run.Enable();

        attack = cubeInputActions.Player.Attack;
        attack.Enable();
//        cubeInputActions.Player.Run.performed += DoRun;
//        cubeInputActions.Player.Run.Enable();
    }

    private void OnDisable()
    {
        movement.Disable();
        run.Disable();
        attack.Disable();
//        cubeInputActions.Player.Run.Disable();
    }

/*    private void DoRun(InputAction.CallbackContext obj)
    {
        //movementSpeed = movementSpeed * movementSpeedModifier;
    }*/

    private void Update()
    {
        bool running = Mathf.Approximately(run.ReadValue<float>(), 1);
        bool attacking = Mathf.Approximately(attack.ReadValue<float>(), 1);

        if (attacking)
        {
            animators[1].Play("swingHammerAnimation");
        }

        Vector2 inputVector = movement.ReadValue<Vector2>();
        Vector3 movementVector = new Vector3();

        movementVector.x = inputVector.x;
        movementVector.z = inputVector.y;
        
        if (movementVector != Vector3.zero && !running)
        {
            controller.Move(movementVector * Time.deltaTime * 3.14f * movementSpeed);

            Quaternion toRotation = Quaternion.LookRotation(movementVector, Vector3.up);
            controller.transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
            //animator.SetFloat("Speed", 0.5f, 0.1f, Time.deltaTime);
            animators[0].SetFloat("Speed", 0.5f, 0.1f, Time.deltaTime);

        }
        else if (movementVector != Vector3.zero && running)
        {
            if (StaminaBar.instance.IsStaminaEmpty())
            {
                controller.Move(movementVector * Time.deltaTime * 3.14f * movementSpeed);

                Quaternion toRotation = Quaternion.LookRotation(movementVector, Vector3.up);
                controller.transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
                animators[0].SetFloat("Speed", 0.5f, 0.1f, Time.deltaTime);
            }
            else
            {
                StaminaBar.instance.UseStamina(0.1f);
                controller.Move(movementVector * Time.deltaTime * 3.14f * movementSpeed * 2f);

                Quaternion toRotation = Quaternion.LookRotation(movementVector, Vector3.up);
                controller.transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
                animators[0].SetFloat("Speed", 0.5f, 0.1f, Time.deltaTime);                
            }
        }
        else
        {
            controller.Move(movementVector * Time.deltaTime * 3.14f * movementSpeed);
            // animator.SetFloat("Speed", 0f, 0.1f, Time.deltaTime);
            animators[0].SetFloat("Speed", 0f, 0.1f, Time.deltaTime);

        }

    }

}
