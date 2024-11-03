using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;          // Скорость передвижения
    public float jumpForce = 5f;          // Сила прыжка
    public float gravityMultiplier = 2f;  // Ускорение гравитации

    [Header("Camera Settings")]
    public Transform playerCamera;        // Камера игрока
    public float lookSensitivity = 2f;    // Чувствительность мыши
    public float maxLookX = 80f;          // Максимальный угол по оси X
    public float minLookX = -80f;         // Минимальный угол по оси X

    private Rigidbody rb;
    private Vector3 moveInput;
    private bool isGrounded = true;       // Проверка на контакт с землей
    private float rotationX = 0f;         // Хранение угла вращения камеры по оси X

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;  // Отключаем вращение тела для более точного управления
        Cursor.lockState = CursorLockMode.Locked;  // Скрыть курсор и зафиксировать в центре
    }

    private void Update()
    {
        Move();
        Rotate();
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }
    }

    private void Move()
    {
        // Получаем вход для перемещения
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // Формируем вектор движения
        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        moveInput = move * moveSpeed;
    }

    private void Rotate()
    {
        // Вращение камеры по оси X (вверх-вниз)
        float mouseY = Input.GetAxis("Mouse Y") * lookSensitivity;
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, minLookX, maxLookX);
        playerCamera.localRotation = Quaternion.Euler(rotationX, 0f, 0f);

        // Вращение тела игрока по оси Y (влево-вправо)
        float mouseX = Input.GetAxis("Mouse X") * lookSensitivity;
        transform.Rotate(Vector3.up * mouseX);
    }

    private void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isGrounded = false;
    }

    private void FixedUpdate()
    {
        // Применяем движение через Rigidbody
        Vector3 velocity = moveInput;
        velocity.y = rb.velocity.y;  // Сохраняем вертикальную скорость (гравитация)
        rb.velocity = velocity;

        // Увеличиваем влияние гравитации
        rb.AddForce(Physics.gravity * gravityMultiplier - Physics.gravity);
    }

    private void OnCollisionStay(Collision collision)
    {
        // Проверяем, на земле ли игрок
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
