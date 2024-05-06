﻿using UnityEngine;
using UnityEngine.UI;

public class striker : MonoBehaviour
{
    // Input handling
    Vector3 currentStrikerPosition, currentDragDirection, forceDirection, touchPosition;
    float dragAmount;

    // Serialized fields
    [Header("Striker Settings")]
    public Vector2 strikerStartPosition = new Vector2(0, -1.47f); // Make it random, based on player turn
    [SerializeField] float forceMultiplier = 230f;
    [SerializeField] float minRequiredForce = 0.8f;
    [SerializeField] float maxForce = 4f;
    [SerializeField] float resetThresholdVelocity = 0.07f;
    [SerializeField] Slider strikerSlider;
    [SerializeField] GameObject focusCircle;
    [SerializeField] GameObject powerControl;
    [SerializeField] AnimationCurve ac;

    // Audio
    public AudioSource[] breakshots;
    public AudioClip[] hitsound, breaksound, hits, pocketfillsound, movesound;

    // Unity components
    Camera mainCamera;
    LineRenderer lineRenderer;
    Rigidbody2D rb;
    SpriteRenderer spriteRenderer;

    // State flags
    bool isBeingDragged = false;
    Vector2 previousVelocity;

    GameObject gameBoard;
    GameObject gameManager; // Reference to GameManager

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        gameManager = GameObject.Find("Game Manager");
        gameBoard = GameObject.Find("carrom_board");
    }

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        lineRenderer = GetComponent<LineRenderer>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Subscribe to the OnTurnChanged event
        GameManager.Instance.OnTurnChanged += HandleTurnChanged;

        previousVelocity = rb.velocity;

        powerControl.SetActive(false); // Hide arrow initially
        focusCircle.SetActive(false);

        this.transform.position = new Vector3(strikerStartPosition.x, strikerStartPosition.y, 0);
        rb.velocity = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        control();
    }

    void FixedUpdate()
    {
        // Calculate change in velocity
        Vector2 deltaVelocity = rb.velocity - previousVelocity;

        // Check if object is decelerating
        bool isDecelerating = deltaVelocity.magnitude > 0 && Vector2.Dot(deltaVelocity, rb.velocity) < 0;

        // Update previous velocity
        previousVelocity = rb.velocity;

        if (rb.velocity.magnitude <= resetThresholdVelocity && isDecelerating) // && rb.velocity.magnitude != 0f
        {
            // breakshots[3].Stop();

            this.transform.position = new Vector3(strikerStartPosition.x, strikerStartPosition.y, 0);
            rb.velocity = Vector2.zero;

            GameManager.Instance.SwitchToNextPlayer();
        }
    }

    public void control()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            touchPosition = mainCamera.ScreenToWorldPoint(touch.position);
            touchPosition.z = 0;

            currentStrikerPosition = this.transform.position;

            // Raycast to check if the touch position is hitting the striker collider
            RaycastHit2D hit = Physics2D.Raycast(touchPosition, Vector3.forward);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    // Show arrow
                    if (hit.transform.CompareTag("striker") && hit.transform.name == this.name)
                    {
                        isBeingDragged = true; // Set the flag to indicate this striker is being dragged
                        powerControl.transform.localScale = Vector3.one * 0.75f; // Reset Gizmos Scale
                        powerControl.SetActive(true);
                    }
                    break;
                case TouchPhase.Moved:
                    if (isBeingDragged) // Process drag motion only if this striker is being dragged
                    {
                        // Calculate directions
                        currentDragDirection = (touchPosition - currentStrikerPosition).normalized;
                        forceDirection = (currentDragDirection * -1).normalized;

                        dragAmount = 4f * Vector2.Distance(currentStrikerPosition, touchPosition);
                        powerControl.transform.localScale = Vector3.one * Mathf.Clamp(dragAmount, 0.75f, maxForce);

                        float angle = Mathf.Atan2(forceDirection.y, forceDirection.x) * Mathf.Rad2Deg;
                        powerControl.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
                    }

                    break;
                case TouchPhase.Ended:
                    if (isBeingDragged) // Process end touch only if this striker is being dragged
                    {
                        isBeingDragged = false; // Reset the flag

                        // Hide arrow
                        powerControl.SetActive(false);
                        powerControl.transform.localScale = Vector3.one * 0.75f; // Reset Gizmos Scale

                        float magnitude = Mathf.Clamp(dragAmount, 0f, maxForce);

                        // Apply force in the direction of drag
                        if (magnitude > minRequiredForce)
                        {
                            breakshots[2].clip = hitsound[Random.Range(0, hitsound.Length)];
                            breakshots[2].Play();

                            rb.AddForce(forceDirection.normalized * magnitude * forceMultiplier);
                            // breakshots[3].Play();

                            strikerStartPosition = GameManager.Instance.GetNextPlayerResetPosition().position;
                        }
                    }
                    break;
            }
        }
    }
    void HandleTurnChanged(int currentPlayerIndex)
    {
        spriteRenderer.sprite = GameManager.Instance.players[GameManager.Instance.currentPlayerIndex].StrikerImage;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "black" || other.gameObject.tag == "white" || other.gameObject.tag == "red")
        {
            if (gameBoard.GetComponent<Collider2D>().enabled == false)
            {
                breakshots[1].clip = hits[Random.Range(0, hits.Length)];
                breakshots[1].volume = Mathf.Clamp01(other.relativeVelocity.magnitude / 12);
                breakshots[1].Play();
            }
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "board")
        {
            breakshots[0].volume = Mathf.Clamp01(rb.velocity.sqrMagnitude / 80);
            gameBoard.GetComponent<Collider2D>().enabled = false;
            breakshots[0].Play();
        }
    }
}