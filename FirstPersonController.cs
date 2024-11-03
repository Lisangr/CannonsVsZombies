using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;          // �������� ������������
    public float jumpForce = 5f;          // ���� ������
    public float gravityMultiplier = 2f;  // ��������� ����������

    [Header("Camera Settings")]
    public Transform playerCamera;        // ������ ������
    public float lookSensitivity = 2f;    // ���������������� ����
    public float maxLookX = 80f;          // ������������ ���� �� ��� X
    public float minLookX = -80f;         // ����������� ���� �� ��� X

    private Rigidbody rb;
    private Vector3 moveInput;
    private bool isGrounded = true;       // �������� �� ������� � ������
    private float rotationX = 0f;         // �������� ���� �������� ������ �� ��� X

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;  // ��������� �������� ���� ��� ����� ������� ����������
        Cursor.lockState = CursorLockMode.Locked;  // ������ ������ � ������������� � ������
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
        // �������� ���� ��� �����������
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // ��������� ������ ��������
        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        moveInput = move * moveSpeed;
    }

    private void Rotate()
    {
        // �������� ������ �� ��� X (�����-����)
        float mouseY = Input.GetAxis("Mouse Y") * lookSensitivity;
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, minLookX, maxLookX);
        playerCamera.localRotation = Quaternion.Euler(rotationX, 0f, 0f);

        // �������� ���� ������ �� ��� Y (�����-������)
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
        // ��������� �������� ����� Rigidbody
        Vector3 velocity = moveInput;
        velocity.y = rb.velocity.y;  // ��������� ������������ �������� (����������)
        rb.velocity = velocity;

        // ����������� ������� ����������
        rb.AddForce(Physics.gravity * gravityMultiplier - Physics.gravity);
    }

    private void OnCollisionStay(Collision collision)
    {
        // ���������, �� ����� �� �����
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
