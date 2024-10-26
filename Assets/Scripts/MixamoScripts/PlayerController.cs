using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;          // Velocidade de movimento
    public float jumpForce = 10f;      // Força do pulo
    public float gravity = -20f;    // Gravidade personalizada
    public float rotationSpeed;

    private Vector3 _moveVelocity;
    private CharacterController _characterController;
    public Animator animator;       // Referência ao Animator
    private float _verticalVelocity;  // Para aplicar a gravidade corretamente

    public Transform modelTransform;

    void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Movimentação no eixo horizontal (esquerda/direita e frente/trás)
        float moveX = Input.GetAxis("Horizontal") * speed;
        float moveZ = Input.GetAxis("Vertical") * speed;

        // Definir o vetor de movimentação baseado nos inputs
        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        // Controla as animações de idle e run
        bool isMoving = moveX != 0 || moveZ != 0; // Verifica se o personagem está se movendo
        animator.SetBool("IsRunning", isMoving); // Ativa a animação de correr se houver movimento
        
        // precisa?
        animator.SetFloat("Speed", Mathf.Abs(moveX) + Mathf.Abs(moveZ)); // Ajusta a velocidade de movimento para animação


        if (_characterController.isGrounded)
        {
            // Reseta a velocidade vertical quando no chão
            _verticalVelocity = 0f;
            animator.SetBool("IsJumping", false); // Desativa a animação de pulo quando no chão

            // Verifica o pulo
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _verticalVelocity = jumpForce;
                animator.SetBool("IsJumping", true); // Ativa a animação de pulo
            }
        }
        else
        {
            // Aplica a gravidade continuamente quando no ar
            _verticalVelocity += gravity * Time.deltaTime;
        }

        // Aplicar a velocidade vertical no vetor de movimento
        _moveVelocity = move + Vector3.up * _verticalVelocity;

        if (isMoving)
        {
            modelTransform.rotation = Quaternion.Slerp(modelTransform.rotation, Quaternion.LookRotation(move), rotationSpeed);
        }

        // Move o personagem
        _characterController.Move(_moveVelocity * Time.deltaTime);
    }
}
