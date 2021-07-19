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
    private InputAction aim;
    // private Animator animator;

    private float movementSpeed = 4f;
    private float movementSpeedModifier = 2f;
    private float rotationSpeed = 215f;

    private Animator[] animators;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Awake()
    {
        cubeInputActions = new CubeInputActions();
        controller = GetComponent<CharacterController>();
        animators = GetComponentsInChildren<Animator>();
    }

    private void OnEnable()
    {
        movement = cubeInputActions.Player.Movement;
        movement.Enable();

        run = cubeInputActions.Player.Run;
        run.Enable();

        attack = cubeInputActions.Player.Attack;
        attack.Enable();

        aim = cubeInputActions.Player.Aim;
        aim.Enable();
//        cubeInputActions.Player.Run.performed += DoRun;
//        cubeInputActions.Player.Run.Enable();
    }

    private void OnDisable()
    {
        movement.Disable();
        run.Disable();
        attack.Disable();
        aim.Disable();
//        cubeInputActions.Player.Run.Disable();
    }

    /*    private void DoRun(InputAction.CallbackContext obj)
        {
            //movementSpeed = movementSpeed * movementSpeedModifier;
        }*/

    private void Update()
    {
        bool attacking = Mathf.Approximately(attack.ReadValue<float>(), 1);

        if (attacking)
        {
            animators[1].Play("swingHammerAnimation");
        }

        movePlayerTo();
    }

    // Old player position x & z transform movement:
    private void oldMovePlayerH(Vector3 movementVector)
    {
        bool running = Mathf.Approximately(run.ReadValue<float>(), 1);

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

    private void movePlayer()
    {

        var cameraForwardDirection = Camera.main.transform.forward;
        Debug.DrawRay(Camera.main.transform.position, cameraForwardDirection * 10, Color.red);
        var cameraVector = Vector3.Scale(cameraForwardDirection, (Vector3.right + Vector3.forward));
        Debug.DrawRay(Camera.main.transform.position, cameraVector * 10, Color.blue);

        Vector2 inputVector = movement.ReadValue<Vector2>();
        Vector3 movementVector = new Vector3();

        movementVector.x = inputVector.x;
        movementVector.z = inputVector.y;

        bool aiming = Mathf.Approximately(aim.ReadValue<float>(), 1);
        bool running = Mathf.Approximately(run.ReadValue<float>(), 1);

        Vector3 camF = Camera.main.transform.forward;
        Vector3 camR = Camera.main.transform.right;

        camF.y = 0;
        camR.y = 0;

        camF = camF.normalized;
        camR = camR.normalized;

        Vector3 newPlayerMovementVector = camF * movementVector.z + camR * movementVector.x;

        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.right * 10, Color.yellow);


        if (movementVector != Vector3.zero && !running) // Walking
        {

            controller.Move(newPlayerMovementVector * 0.5f * Time.deltaTime * 3.14f * movementSpeed);
            Quaternion toRotation = Quaternion.LookRotation(newPlayerMovementVector, Vector3.up);
            controller.transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);

        }
        else if (movementVector != Vector3.zero && running) // Running
        {
            if (StaminaBar.instance.IsStaminaEmpty())
            {
                controller.Move(newPlayerMovementVector * 0.5f * Time.deltaTime * 3.14f * movementSpeed);
                Quaternion toRotation = Quaternion.LookRotation(newPlayerMovementVector, Vector3.up);
                controller.transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
                animators[0].SetFloat("Speed", 0.5f, 0.1f, Time.deltaTime);
            }
            else
            {
                StaminaBar.instance.UseStamina(0.05f);
                controller.Move(newPlayerMovementVector * 0.5f * Time.deltaTime * 3.14f * movementSpeed * movementSpeedModifier);
                Quaternion toRotation = Quaternion.LookRotation(newPlayerMovementVector, Vector3.up);
                controller.transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
                animators[0].SetFloat("Speed", 1f, 0.1f, Time.deltaTime);
            }
        }
        else // Idle
        {
            if (aiming)
            {
                Quaternion toRotation = Quaternion.LookRotation(cameraVector, Vector3.up);
                controller.transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
            }

            animators[0].SetFloat("Speed", 0f, 0.1f, Time.deltaTime);
        }

    }

    private void movePlayerTo()
    {

        var cameraForwardDirection = Camera.main.transform.forward;
        var cameraVector = Vector3.Scale(cameraForwardDirection, (Vector3.right + Vector3.forward));

        bool canRun = StaminaBar.instance.IsStaminaEmpty();
        bool aiming = Mathf.Approximately(aim.ReadValue<float>(), 1);

        // Player Input
        bool running = Mathf.Approximately(run.ReadValue<float>(), 1);
        Vector2 inputVector = movement.ReadValue<Vector2>();
        
        Vector3 movementVector = new Vector3();
        movementVector.x = inputVector.x;
        movementVector.z = inputVector.y;
        bool moving = movementVector != Vector3.zero;

        Vector3 camF = Camera.main.transform.forward;
        Vector3 camR = Camera.main.transform.right;
        camF.y = 0;
        camR.y = 0;
        camF = camF.normalized;
        camR = camR.normalized;

        Vector3 newPlayerMovementVector = camF * movementVector.z + camR * movementVector.x;

        if (moving && !running) // Walking
        {
            playerRotation(aiming, cameraVector, newPlayerMovementVector);
            controller.Move(newPlayerMovementVector * 0.5f * Time.deltaTime * 3.14f * movementSpeed);
            animators[0].SetFloat("Speed", 0.5f, 0.1f, Time.deltaTime);
        }
        else if (moving && running && canRun) // Running
        {
            playerRotation(aiming, cameraVector, newPlayerMovementVector);
            StaminaBar.instance.UseStamina(0.05f);
            controller.Move(newPlayerMovementVector * 0.5f * Time.deltaTime * 3.14f * movementSpeed * movementSpeedModifier);
            animators[0].SetFloat("Speed", 1f, 0.1f, Time.deltaTime);
        }
        else if (moving && running && !canRun) // Attempting to run
        {
            playerRotation(aiming, cameraVector, newPlayerMovementVector);
            controller.Move(newPlayerMovementVector * 0.5f * Time.deltaTime * 3.14f * movementSpeed * movementSpeedModifier);
            animators[0].SetFloat("Speed", 0.5f, 0.1f, Time.deltaTime);
        }
        else // Idle
        {
            if (aiming)
                playerRotation(aiming, cameraVector, newPlayerMovementVector);
            animators[0].SetFloat("Speed", 0f, 0.1f, Time.deltaTime);
        }
    }

    private void playerRotation(bool aiming, Vector3 cameraVector, Vector3 movementVector)
    {
        Quaternion toRotation = aiming ? Quaternion.LookRotation(cameraVector, Vector3.up) : Quaternion.LookRotation(movementVector, Vector3.up); 
        controller.transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
    }
}
