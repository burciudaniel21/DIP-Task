using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioClip))]
[RequireComponent(typeof(AudioSource))]
public class PlayerController : MonoBehaviour
{
    // Movement variables
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    private bool isGrounded;

    // Health system
    private int health = 100;

    // Input system (directly tied to Unity's Input class)
    private float horizontalInput;
    private bool jumpInput;

    // Enemy interaction
    private bool isTakingDamage;

    // UI update (directly controlling TextMeshPro for UI display)
    public TMPro.TextMeshProUGUI healthText;

    // Audio (manages its own audio sources directly)
    public AudioSource audioSource;
    public AudioClip jumpSound;
    public AudioClip damageSound;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Update UI on start
        UpdateHealthUI();
    }

    private void Update()
    {
        // Input handling
        horizontalInput = Input.GetAxis("Horizontal");
        jumpInput = Input.GetButtonDown("Jump");

        // Movement
        Move();

        // Jumping
        if (jumpInput && isGrounded)
        {
            Jump();
        }

        // Damage handling
        if (isTakingDamage)
        {
            TakeDamage(10);
            isTakingDamage = false;
        }
    }

    private void Move()
    {
        Vector3 movement = new Vector3(horizontalInput * moveSpeed, rb.velocity.y, 0);
        rb.velocity = movement;
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
        audioSource.PlayOneShot(jumpSound);
        isGrounded = false;
    }

    private void TakeDamage(int damage)
    {
        health -= damage;
        audioSource.PlayOneShot(damageSound);
        UpdateHealthUI();

        if (health <= 0)
        {
            Die();
        }
    }

    private void UpdateHealthUI()
    {
        healthText.text = $"Health: {health}";
    }

    private void Die()
    {
        Debug.Log("Character has died.");
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            isTakingDamage = true;
        }
    }
}
