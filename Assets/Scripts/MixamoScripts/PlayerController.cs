using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;          // Velocidade de movimento
    public float jumpForce = 10f;      // For�a do pulo
    public float gravity = -20f;    // Gravidade personalizada
    public float rotationSpeed;

    private Vector3 _moveVelocity;
    private CharacterController _characterController;
    public Animator animator;       // Refer�ncia ao Animator
    private float _verticalVelocity;  // Para aplicar a gravidade corretamente

    public Transform modelTransform;

    void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Movimenta��o no eixo horizontal (esquerda/direita e frente/tr�s)
        float moveX = Input.GetAxis("Horizontal") * speed;
        float moveZ = Input.GetAxis("Vertical") * speed;

        // Definir o vetor de movimenta��o baseado nos inputs
        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        // Controla as anima��es de idle e run
        bool isMoving = moveX != 0 || moveZ != 0; // Verifica se o personagem est� se movendo
        animator.SetBool("IsRunning", isMoving); // Ativa a anima��o de correr se houver movimento
        
        // precisa?
        animator.SetFloat("Speed", Mathf.Abs(moveX) + Mathf.Abs(moveZ)); // Ajusta a velocidade de movimento para anima��o


        if (_characterController.isGrounded)
        {
            // Reseta a velocidade vertical quando no ch�o
            _verticalVelocity = 0f;
            animator.SetBool("IsJumping", false); // Desativa a anima��o de pulo quando no ch�o

            // Verifica o pulo
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _verticalVelocity = jumpForce;
                animator.SetBool("IsJumping", true); // Ativa a anima��o de pulo
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
