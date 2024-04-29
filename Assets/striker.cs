using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class striker : MonoBehaviour
{
    // Vector3 strikerPosition, targetDirection, touchPosition;
    Vector3 strikerPosition, dragDirection, forceDirection, touchPosition;
    float dragAmount;

    GameObject gameManager;
    public Vector2 strikerStartPosition = new Vector2(0, -1.47f);
    public float forceMultiplier = 230f;
    public float minRequiredForce = 0.8f;
    public float maxForce = 4f;

    // TODO: Instea of Array, do something else
    public AudioSource[] breakshots;
    public AudioClip[] hitsound, breaksound, hits, pocketfillsound, movesound;

    [SerializeField] Slider StrikerSlider;

    public GameObject focusCircle;
    public GameObject powerControl;
    public float resetThresholdVelocity = 0.07f;
    public GameObject board;
    public int black, white, red;
    public bool coveringTheQueen = false;

    Camera cm;
    LineRenderer lr;
    GameObject start;

    Rigidbody2D rb;
    // bool hit = false;
    public bool st = false;
    public bool movestriker = false;
    public bool isPlayerTurn = false;
    private bool isBeingDragged = false;
    public byte currentTurnIndex = 0;
    bool showGizmos = false;
    [SerializeField] AnimationCurve ac;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        start = GameObject.Find("start");
        gameManager = GameObject.Find("Game Manager");
    }

    // Start is called before the first frame update
    void Start()
    {
        cm = Camera.main;
        lr = GetComponent<LineRenderer>();
        rb = GetComponent<Rigidbody2D>();

        powerControl.SetActive(false); // Hide arrow initially
        focusCircle.SetActive(false);
    }

    GameObject GetCurrentPlayingCharacter()
    {
        return gameManager.GetComponent<Game_Manager>().currentPlayingCharacter;
    }

    // Update is called once per frame
    void Update()
    {
        // if (rb.velocity.magnitude <= resetThresholdVelocity)
        // {
        //     // ReturnStriker(); // Reset Striker Position
        //     ResetStrikerPos();
        //     if (movestriker == false)
        //     {
        //         breakshots[3].Stop();
        if (isPlayerTurn)
        {
            control();
        }
        //     }
        // }
    }


    public void ResetStrikerPos()
    {
        // if (rb.velocity.magnitude <= resetThresholdVelocity)
        // {
        // // float yPosition = t ? 1.47f : -1.47f;
        // // float yPosition = GetCurrentPlayingCharacter().GetComponent<striker>().strikerStartPosition.y;
        float yPosition = strikerStartPosition.y;
        this.transform.position = new Vector3(StrikerSlider.value, yPosition, 0);
        // }
    }

    public void UpdateScore()
    {
        gameManager.GetComponent<Game_Manager>().white = (byte)white;
        gameManager.GetComponent<Game_Manager>().black = (byte)black;
        gameManager.GetComponent<Game_Manager>().UpdateScoreUI();
    }
    public void control()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
            touchPosition.z = 0;

            strikerPosition = this.transform.position;

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
                        dragDirection = (touchPosition - strikerPosition).normalized;
                        forceDirection = (dragDirection * -1).normalized;

                        dragAmount = 4f * Vector2.Distance(strikerPosition, touchPosition);
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
                            currentTurnIndex++;
                        }
                    }
                    break;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "black" || other.gameObject.tag == "white" || other.gameObject.tag == "red")
        {
            if (board.GetComponent<Collider2D>().enabled == false)
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
            board.GetComponent<Collider2D>().enabled = false;
            breakshots[0].Play();
        }
    }
}