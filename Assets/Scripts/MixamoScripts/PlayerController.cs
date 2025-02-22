using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //public float speed = 5f;          // Velocidade de movimento
    //public float jumpForce = 10f;      // Força do pulo
    //public float gravity = -20f;    // Gravidade personalizada
    //public float rotationSpeed;

    //private Vector3 _moveVelocity;
    //private CharacterController _characterController;
    //public Animator animator;       // Referência ao Animator
    //private float _verticalVelocity;  // Para aplicar a gravidade corretamente

    //public Transform modelTransform;

    //void Start()
    //{
    //    _characterController = GetComponent<CharacterController>();
    //}

    //void Update()
    //{
    //    // Movimentação no eixo horizontal (esquerda/direita e frente/trás)
    //    float moveX = Input.GetAxis("Horizontal") * speed;
    //    float moveZ = Input.GetAxis("Vertical") * speed;

    //    // Definir o vetor de movimentação baseado nos inputs
    //    Vector3 move = transform.right * moveX + transform.forward * moveZ;

    //    // Controla as animações de idle e run
    //    bool isMoving = moveX != 0 || moveZ != 0; // Verifica se o personagem está se movendo
    //    animator.SetBool("IsRunning", isMoving); // Ativa a animação de correr se houver movimento

    //    // precisa?
    //    animator.SetFloat("Speed", Mathf.Abs(moveX) + Mathf.Abs(moveZ)); // Ajusta a velocidade de movimento para animação


    //    if (_characterController.isGrounded)
    //    {
    //        // Reseta a velocidade vertical quando no chão
    //        _verticalVelocity = 0f;
    //        animator.SetBool("IsJumping", false); // Desativa a animação de pulo quando no chão

    //        // Verifica o pulo
    //        if (Input.GetKeyDown(KeyCode.Space))
    //        {
    //            _verticalVelocity = jumpForce;
    //            animator.SetBool("IsJumping", true); // Ativa a animação de pulo
    //        }
    //    }
    //    else
    //    {
    //        // Aplica a gravidade continuamente quando no ar
    //        _verticalVelocity += gravity * Time.deltaTime;
    //    }

    //    // Aplicar a velocidade vertical no vetor de movimento
    //    _moveVelocity = move + Vector3.up * _verticalVelocity;

    //    if (isMoving)
    //    {
    //        modelTransform.rotation = Quaternion.Slerp(modelTransform.rotation, Quaternion.LookRotation(move), rotationSpeed);
    //    }

    //    // Move o personagem
    //    _characterController.Move(_moveVelocity * Time.deltaTime);
    //}
    public bool useCharacterForward = false;
    public bool lockToCameraForward = false;
    public float turnSpeed = 10f;
    public KeyCode sprintJoystick = KeyCode.JoystickButton2;
    public KeyCode sprintKeyboard = KeyCode.Space;

    private float turnSpeedMultiplier;
    private float speed = 0f;
    private float direction = 0f;
    private bool isSprinting = false;
    private Vector3 targetDirection;
    private Vector2 input;
    private Quaternion freeRotation;
    private Camera mainCamera;
    private float velocity;

    // Use this for initialization
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Vertical");

        // set speed to both vertical and horizontal inputs
        if (useCharacterForward)
            speed = Mathf.Abs(input.x) + input.y;
        else
            speed = Mathf.Abs(input.x) + Mathf.Abs(input.y);

        speed = Mathf.Clamp(speed, 0f, 1f);
      //speed = Mathf.SmoothDamp(anim.GetFloat("Speed"), speed, ref velocity, 0.1f);
        

        if (input.y < 0f && useCharacterForward)
            direction = input.y;
        else
            direction = 0f;

        

        // set sprinting
        isSprinting = ((Input.GetKey(sprintJoystick) || Input.GetKey(sprintKeyboard)) && input != Vector2.zero && direction >= 0f);
        

        // Update target direction relative to the camera view (or not if the Keep Direction option is checked)
        UpdateTargetDirection();
        if (input != Vector2.zero && targetDirection.magnitude > 0.1f)
        {
            Vector3 lookDirection = targetDirection.normalized;
            freeRotation = Quaternion.LookRotation(lookDirection, transform.up);
            var diferenceRotation = freeRotation.eulerAngles.y - transform.eulerAngles.y;
            var eulerY = transform.eulerAngles.y;

            if (diferenceRotation < 0 || diferenceRotation > 0) eulerY = freeRotation.eulerAngles.y;
            var euler = new Vector3(0, eulerY, 0);

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(euler), turnSpeed * turnSpeedMultiplier * Time.deltaTime);
        }
    }

    public virtual void UpdateTargetDirection()
    {
        if (!useCharacterForward)
        {
            turnSpeedMultiplier = 1f;
            var forward = mainCamera.transform.TransformDirection(Vector3.forward);
            forward.y = 0;

            //get the right-facing direction of the referenceTransform
            var right = mainCamera.transform.TransformDirection(Vector3.right);

            // determine the direction the player will face based on input and the referenceTransform's right and forward directions
            targetDirection = input.x * right + input.y * forward;
        }
        else
        {
            turnSpeedMultiplier = 0.2f;
            var forward = transform.TransformDirection(Vector3.forward);
            forward.y = 0;

            //get the right-facing direction of the referenceTransform
            var right = transform.TransformDirection(Vector3.right);
            targetDirection = input.x * right + Mathf.Abs(input.y) * forward;
        }
    }
}
