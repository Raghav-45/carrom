using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class striker : MonoBehaviour
{
    Vector3 strikerPosition, targetDirection, touchPosition;

    public GameObject gameManager;

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
    bool player = false;
    public int black, white, red;
    public bool coveringTheQueen = false;

    Camera cm;
    LineRenderer lr;
    GameObject start;

    Rigidbody2D rb;
    // bool hit = false;
    public bool st = false;
    public bool movestriker = false;
    public bool turn = false;
    bool showGizmos = false;
    [SerializeField] AnimationCurve ac;

    float angle = 0f;

    // Start is called before the first frame update
    void Start()
    {
        start = GameObject.Find("start");
        gameManager = GameObject.Find("Game Manager");

        cm = Camera.main;
        lr = GetComponent<LineRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rb.velocity.magnitude <= resetThresholdVelocity)
        {
            // ReturnStriker(); // Reset Striker Position
            MoveStriker(false);
            if (movestriker == false)
            {
                breakshots[3].Stop();
                focusCircle.SetActive(false); // set to true
                control();
            }
        }

        //if (focusCircle.active == true)
        //{
        //    angle += 75f * Time.deltaTime;
        //    focusCircle.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        //}
    }
    public void MoveStriker(bool t)
    {
        if (rb.velocity.magnitude <= resetThresholdVelocity)
        {
            float yPosition = t ? 1.47f : -1.47f;
            this.transform.position = new Vector3(StrikerSlider.value, yPosition, 0);
        }
        // player = t;
    }

    public void UpdateScore()
    {
        gameManager.GetComponent<Game_Manager>().white = (byte)white;
        gameManager.GetComponent<Game_Manager>().black = (byte)black;
        gameManager.GetComponent<Game_Manager>().UpdateScoreUI();
    }
    private void control()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            touchPosition = cm.ScreenToWorldPoint(touch.position);

            // Raycast to check if the touch position is hitting the striker collider
            RaycastHit2D hit = Physics2D.Raycast(touchPosition, Vector3.forward);

            powerControl.SetActive(false);

            if (hit.collider)
            {
                if (hit.transform.name == "striker")
                {
                    showGizmos = true;
                }

                if (showGizmos == true)
                {
                    powerControl.SetActive(true);
                    focusCircle.SetActive(false);

                    strikerPosition = this.transform.position;

                    float scaleValue = 4f * Vector2.Distance(strikerPosition, touchPosition);
                    powerControl.transform.localScale = Vector3.one * Mathf.Clamp(scaleValue, 0.75f, maxForce);

                    targetDirection = strikerPosition - touchPosition;
                    float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
                    powerControl.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);

                    // Draw Debug line for Force Vector
                    Debug.DrawLine(strikerPosition, touchPosition, Color.blue);

                    // Calculate the end position
                    // Vector3 linePos = strikerPosition + targetDirection;

                    // lr.positionCount = 2;
                    // lr.widthCurve = ac;
                    // lr.numCapVertices = 10;
                    // lr.SetPosition(0, strikerPosition);
                    // lr.SetPosition(1, linePos);
                    // lr.enabled = true;
                }
            }
        }
        else if (showGizmos)
        {
            // lr.enabled = false;
            showGizmos = false;
            powerControl.SetActive(false);
            focusCircle.SetActive(false);
            powerControl.transform.localScale = Vector3.one * 0.75f;

            float dragAmount = 4f * Vector2.Distance(strikerPosition, touchPosition);
            float magnitudeClamped = Mathf.Clamp(dragAmount, 0f, maxForce);
            Vector2 hitDirectionNormalized = targetDirection.normalized;
            Vector3 forceVector = hitDirectionNormalized.normalized * magnitudeClamped;

            if (magnitudeClamped > minRequiredForce)
            {
                breakshots[2].clip = hitsound[Random.Range(0, hitsound.Length)];
                breakshots[2].Play();

                rb.AddForce(forceVector.normalized * magnitudeClamped * forceMultiplier);

                player = false; // Set this to False to Make Player One Always Take Turn
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