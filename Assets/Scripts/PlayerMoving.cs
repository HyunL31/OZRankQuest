using UnityEngine;
using UnityEngine.UI;

public class PlayerMoving : MonoBehaviour
{
    PlayerState playerState = new PlayerState();

    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float rotateSpeed = 10f;
    public float jumpPower = 5f;
    public float doubleJumpPower = 3f;
    public AudioSource walkSound;
    public AudioSource runSound;
    public AudioClip jumpSound;
    public AudioClip doubleJump;

    public Image staminaGage;

    private Rigidbody _rb;
    private Animator _anim;

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

        if (playerState.isGround)
        {
            Move(inputX, inputZ);
        }

        playerState.Stamina();
        AnimControl();

        CheckStamina();
    }

    private void Move(float inputX, float inputZ)
    {
        if ((inputX == 0 && inputZ == 0) || playerState.stamina <= 0)
        {
            playerState.isWalking = false;
            playerState.isRunning = false;

            runSound.Pause();
            walkSound.Pause();

            return;
        }

        float moveSpeed = walkSpeed;
        playerState.isWalking = true;
        playerState.isRunning = false;

        runSound.Pause();

        if (!walkSound.isPlaying)
        {
            walkSound.Play();
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            playerState.isRunning = true;
            playerState.isWalking = false;
            moveSpeed = runSpeed;

            walkSound.Pause();

            if (!runSound.isPlaying)
            {
                runSound.Play();
            }
        }

        _rb.linearVelocity = new Vector3(inputX, 0, inputZ) * moveSpeed;

        Vector3 dir = new Vector3(inputX, 0, inputZ);

        Quaternion targetRotation = Quaternion.LookRotation(dir);
        _rb.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotateSpeed);
    }

    private void Jump()
    {
        if (jumpCount >= 2 || playerState.stamina <= 8)
        {
            return;
        }

        playerState.isGround = false;
        jumpCount++;

        walkSound.Pause();
        runSound.Pause();
        walkSound.PlayOneShot(jumpSound);

        float power = jumpCount == 1 ? jumpPower : doubleJumpPower;

        _rb.AddForce(Vector3.up * power, ForceMode.VelocityChange);

        if (jumpCount == 2)
        {
            playerState.stamina -= 5;
            _anim.SetTrigger("DoubleJump");
            walkSound.PlayOneShot(doubleJump);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            playerState.isGround = true;
            jumpCount = 0;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            playerState.isGround = false;
        }
    }

    private void AnimControl()
    {
        _anim.SetBool("isJumping", !playerState.isGround);
        _anim.SetBool("isWalking", playerState.isGround && playerState.isWalking);
        _anim.SetBool("isRunning", playerState.isGround && playerState.isRunning);
    }

    private void Die()
    {
        _anim.SetTrigger("isDie");
    }

    private void CheckStamina()
    {
        playerState.ChangeState();

        staminaGage.fillAmount = playerState.stamina / 100f;

        Color gageColor = Color.white;

        if (playerState.state == State.Good)
        {
            gageColor = Color.green;
        }
        else if (playerState.state == State.Normal)
        {
            gageColor = Color.yellow;
        }
        else if (playerState.state == State.Bad)
        {
            gageColor = Color.red;
        }

        staminaGage.color = gageColor;

        if (playerState.stamina <= 0)
        {
            playerState.isWalking = false;
            playerState.isRunning = false;

            Die();
        }
    }
}
