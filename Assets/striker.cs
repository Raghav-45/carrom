using UnityEngine;
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
    [SerializeField] float allowedTouchRadius = 0.36f;
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
    Transform myTransform;

    // State flags
    bool isBeingDragged = false;
    bool isDecelerating;
    Vector2 previousVelocity;
    bool startObserving;

    GameObject gameBoard;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        gameBoard = GameObject.Find("carrom_board");
        myTransform = transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        lineRenderer = GetComponent<LineRenderer>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Subscribe to events
        GameManager.Instance.OnTurnChanged += HandleTurnChanged;

        previousVelocity = rb.velocity;

        powerControl.SetActive(false); // Hide arrow initially
        focusCircle.SetActive(false);

        myTransform.position = new Vector3(strikerStartPosition.x, strikerStartPosition.y, 0);
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
        isDecelerating = deltaVelocity.magnitude > 0 && Vector2.Dot(deltaVelocity, rb.velocity) < 0;

        // Update previous velocity
        previousVelocity = rb.velocity;

        if (rb.velocity.magnitude <= resetThresholdVelocity && isDecelerating && startObserving) // && rb.velocity.magnitude != 0f
        {
            startObserving = false;
            // breakshots[3].Stop();

            // if (isCollectedAnyCoin)
            // {
            //     // Should Get Extra Turn
            //     myTransform.position = new Vector3(strikerStartPosition.x, strikerStartPosition.y, 0);
            //     rb.velocity = Vector2.zero;

            //     isCollectedAnyCoin = false;
            // }
            // else
            // {
            //     // Should not Get Turn
            //     strikerStartPosition = GameManager.Instance.GetNextPlayerResetPosition().position;
            //     myTransform.position = new Vector3(strikerStartPosition.x, strikerStartPosition.y, 0);
            //     rb.velocity = Vector2.zero;

            //     // foreach (var player in GameManager.Instance.players)
            //     // {
            //     //     player.isQueenCoveringMove = false;
            //     // }

            //     GameManager.Instance.SwitchToNextPlayer();
            // }
        }
    }

    public void control()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            touchPosition = mainCamera.ScreenToWorldPoint(touch.position);
            touchPosition.z = 0;

            currentStrikerPosition = myTransform.position;

            float touchErrorDistance = Vector2.Distance(currentStrikerPosition, touchPosition);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    // Show arrow
                    if (touchErrorDistance < allowedTouchRadius)
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
                            startObserving = true;
                            // breakshots[3].Play();
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

public class PlayerCoin
{
    public Player player;
    public CoinType coinType;

    public PlayerCoin(Player player, CoinType coinType)
    {
        this.player = player;
        this.coinType = coinType;
    }
}