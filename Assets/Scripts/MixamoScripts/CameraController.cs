using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float mouseSensitivity = 100f;  // Sensibilidade ajustável no editor
    public float limitVerticalRotationDown = 90f;  // Limite para rotação vertical
    public float limitVerticalRotationUp = 90f;  // Limite para rotação vertical
    public Transform playerTransform;  // Transform do jogador

    private float _xRotation = 0f;  // Para rastrear a rotação no eixo X

    void Start()
    {
        // Trava o cursor no centro da tela
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Obtém as entradas do mouse
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Controla a rotação vertical (eixo X)
        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, limitVerticalRotationDown, limitVerticalRotationUp);

        // Aplica a rotação na câmera no eixo X (olhar para cima/baixo)
        transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);

        // Aplica a rotação horizontal (eixo Y) no jogador
        playerTransform.Rotate(Vector3.up * mouseX);
    }
}
