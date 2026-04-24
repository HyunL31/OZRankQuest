using UnityEngine;

public class PlayerMoving : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float rotateSpeed = 10f;
    public float jumpPower = 5f;
    public float doubleJumpPower = 3f;
    public AudioSource walkSound;
    public AudioSource runSound;

    private Rigidbody _rb;
    private Animator _anim;

    private int stamina = 100;
    private bool isGround = true;
    private bool isRunning = false;
    private bool isWalking = false;
    private int jumpCount = 0;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();
    }

    void Update()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputZ = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        if (isGround)
        {
            Move(inputX, inputZ);
        }

        AnimControl();
    }

    private void Move(float inputX, float inputZ)
    {
        if (inputX == 0 && inputZ == 0)
        {
            isWalking = false;
            isRunning = false;

            runSound.Pause();
            walkSound.Pause();

            return;
        }

        float moveSpeed = walkSpeed;
        isWalking = true;
        isRunning = false;

        runSound.Pause();
        
        if (!walkSound.isPlaying)
        {
            walkSound.Play();
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            isRunning = true;
            isWalking = false;
            moveSpeed = runSpeed;

            walkSound.Pause();
            runSound.Play();
        }

        _rb.linearVelocity = new Vector3(inputX, 0, inputZ) * moveSpeed;

        Vector3 dir = new Vector3(inputX, 0, inputZ);

        Quaternion targetRotation = Quaternion.LookRotation(dir);
        _rb.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotateSpeed);
    }

    private void Jump()
    {
        if (jumpCount >= 2)
        {
            return;
        }

        isGround = false;
        jumpCount++;

        stamina -= 10;

        float power = jumpCount == 1 ? jumpPower : doubleJumpPower;

        _rb.AddForce(Vector3.up * power, ForceMode.VelocityChange);

        if (jumpCount == 2)
        {
            stamina -= 5;
            _anim.SetTrigger("DoubleJump");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGround = true;
            jumpCount = 0;
        }
    }

    private void AnimControl()
    {
        _anim.SetBool("isJumping", !isGround);
        _anim.SetBool("isWalking", isGround && isWalking);
        _anim.SetBool("isRunning", isGround && isRunning);
    }
}
