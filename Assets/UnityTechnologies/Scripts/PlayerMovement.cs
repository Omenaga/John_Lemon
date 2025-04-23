using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public float turnSpeed = 20f;
    public bool isFrozen = false;
    public float walkingSpeed; // Normal walking speed
    public float sprintSpeed;  // Sprinting speed
    public float maxSprintTime;   // Max sprinting time
    public float sprintRechargeDuration; // Recharge Rate

    private float sprintTimer = 0f;             // Current available sprint time
    private bool isSprinting = false;
    private bool canSprintAgain = true;

    Animator m_Animator;
    Rigidbody m_Rigidbody;
    AudioSource m_AudioSource;
    Vector3 m_Movement;
    Quaternion m_Rotation = Quaternion.identity;

    public Slider sprintBar;
    public Image sprintFill;

    // Start is called before the first frame update
    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_AudioSource = GetComponent<AudioSource>();
        
        canSprintAgain = true;
        isSprinting = false;
        isFrozen = false;

        // Get Difficulty
        string selectedDifficulty = DifficultySelection.difficultyLevel;

        // Difficulty Settings
        switch (selectedDifficulty)
        {
            case "Easy":
                sprintSpeed = 3f;
                maxSprintTime = 5f;
                sprintRechargeDuration = 5f;
                break;
            case "Normal":
                sprintSpeed = 2f;
                maxSprintTime = 3f;
                sprintRechargeDuration = 10f;
                break;
            case "Hard":
                sprintSpeed = 1.5f;
                maxSprintTime = 2f;
                sprintRechargeDuration = 15f;
                break;
            case "Diabolical":
                sprintSpeed = 1.25f;
                maxSprintTime = 1f;
                sprintRechargeDuration = 20f;
                break;
        }

        sprintTimer = maxSprintTime;

        if (sprintFill != null)
        {
            sprintFill.color = new Color(0.196f, 0.8039f, 0.196f); // Ectoplasm Green
        }

        if (sprintBar != null)
        {
            sprintBar.value = 1f; // Full bar
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isFrozen)
        {
            // Stop Movement
            m_Movement = Vector3.zero;
            m_Animator.SetBool("IsWalking", false);

            if (sprintTimer < maxSprintTime)
            {
                float rechargeRate = maxSprintTime / sprintRechargeDuration;
                sprintTimer += rechargeRate * Time.deltaTime;
                if (sprintTimer >= maxSprintTime)
                {
                    // Fully recharged
                    sprintTimer = maxSprintTime;
                    canSprintAgain = true;
                    sprintFill.color = new Color(0.196f, 0.8039f, 0.196f); // Ectoplasm Green
                    Debug.Log("Sprinting Enabled");
                }
            }

            if (sprintBar != null)
            {
                sprintBar.value = sprintTimer / maxSprintTime;
            }

            return;
        }

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        m_Movement.Set(horizontal, 0f, vertical);
        m_Movement.Normalize();

        if (Input.GetKey(KeyCode.LeftShift) && sprintTimer > 0f && canSprintAgain)
        {
            // Sprint if available
            isSprinting = true;
            sprintTimer -= Time.deltaTime;  // Decreases sprint timer
            if (sprintTimer < 0f)
            {
                // Lock sprinting
                sprintTimer = 0f;
                canSprintAgain = false;
                sprintFill.color = new Color(0.541f, 0.0f, 0.0f); // Blood Red
                Debug.Log("Sprinting Disabled");
            }
        }
        else
        {
            // Stop sprinting
            isSprinting = false;

            if (sprintTimer < maxSprintTime)
            {
                float rechargeRate = maxSprintTime / sprintRechargeDuration;
                sprintTimer += rechargeRate * Time.deltaTime;
                if (sprintTimer >= maxSprintTime)
                {
                    // Fully recharged
                    sprintTimer = maxSprintTime;
                    canSprintAgain = true;
                    sprintFill.color = new Color(0.196f, 0.8039f, 0.196f); // Ectoplasm Green
                    Debug.Log("Sprinting Enabled");
                }
            }
        }

        if (isSprinting)
        {
            // Sprint Speed
            m_Movement *= sprintSpeed;
        }
        else
        {
            // Regular Speed
            m_Movement *= walkingSpeed;
        }

        bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0f);
        bool hasVerticalInput = !Mathf.Approximately(vertical, 0f);
        bool isWalking = hasHorizontalInput || hasVerticalInput;
        m_Animator.SetBool("IsWalking", isWalking);

        if(isWalking)
        {
            if(!m_AudioSource.isPlaying)
            {
                m_AudioSource.Play();
            }
        }
        else
        {
            m_AudioSource.Stop();
        }

        Vector3 desiredForward = Vector3.RotateTowards(transform.forward, m_Movement, turnSpeed * Time.deltaTime, 0f);
        m_Rotation = Quaternion.LookRotation(desiredForward);

        if (sprintBar != null)
        {
            sprintBar.value = sprintTimer / maxSprintTime;
        }
    }

    void OnAnimatorMove()
    {
        m_Rigidbody.MovePosition (m_Rigidbody.position + m_Movement * m_Animator.deltaPosition.magnitude);
        m_Rigidbody.MoveRotation (m_Rotation);
    }
}