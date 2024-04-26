using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class striker : MonoBehaviour
{
  Vector3 strikerPosition, endPos, targetDirection, touchPosition;

  // public float forceMultiplier = 350f;
  public float forceMultiplier = 1500f;
  public float minRequiredForce = 3f;
  public float maxForce = 5f;

  // TODO: Instea of Array, do something else
  public AudioSource[] breakshots;
  public AudioClip[] hitsound, breaksound, hits, pocketfillsound, movesound;

  [SerializeField] Slider StrikerSlider;

  public GameObject powerCircle;
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

  // Start is called before the first frame update
  void Start()
  {
    start = GameObject.Find("start");

    cm = Camera.main;
    lr = GetComponent<LineRenderer>();
    rb = GetComponent<Rigidbody2D>();
  }

  // Update is called once per frame
  void Update()
  {
    if (rb.velocity.magnitude <= resetThresholdVelocity)
    {
      ReturnStriker(); // Reset Striker Position
      if (movestriker == false)
      {
        breakshots[3].Stop();
        control();
      }
    }

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
  private void control()
  // {
  //   if (Input.touchCount > 0)
  //   {
  //     startPos = this.transform.position;

  //     lr.positionCount = 2;
  //     lr.widthCurve = ac;
  //     lr.numCapVertices = 10;
  //     lr.enabled = false;
  //     lr.SetPosition(0, startPos);

  //     if (Input.GetMouseButton(0) || (Input.GetTouch(0).phase == TouchPhase.Began))
  //     // {
  //     //   endPos = cm.ScreenToWorldPoint(Input.mousePosition);
  //     //   lr.SetPosition(1, endPos);

  //     //   lr.enabled = true;
  //     // }
  //     {
  //       endPos = cm.ScreenToWorldPoint(Input.mousePosition);

  //       // Calculate the direction from start position to input position
  //       Vector3 hitDirection = startPos - endPos;

  //       // Calculate the end position
  //       Vector3 linePos = startPos + hitDirection;

  //       // Draw a debug line from startPos to endPos
  //       Debug.DrawLine(startPos, endPos, Color.blue);

  //       // Set the end position for the line renderer
  //       lr.SetPosition(1, linePos);

  //       // Enable the line renderer
  //       lr.enabled = true;
  //     }

  //     if (Input.GetMouseButtonUp(0) || (Input.GetTouch(0).phase == TouchPhase.Ended))
  //     {
  //       lr.enabled = false;
  //       // hit = true;
  //       st = true;
  //       targetDirection = endPos - startPos;
  //       Vector3 testtargetDirection = startPos - endPos;

  //       // Draw a debug line to visualize the target direction
  //       Debug.DrawLine(startPos, endPos, Color.green, 2f);

  //       if (targetDirection.magnitude > 10.007f)
  //       {
  //         lr.material.color = Color.red;

  //         breakshots[2].clip = hitsound[Random.Range(0, hitsound.Length)];
  //         breakshots[2].Play();

  //         Debug.Log(testtargetDirection.magnitude);

  //         // 10 > is good
  //         rb.AddForce(testtargetDirection * 350);

  //         player = false; // Set this to False to Make Player One Always Take Turn

  //         // if (player == true && this.GetComponent<pocket>().get_st() == false)
  //         // {
  //         //   player = false;
  //         // }
  //         // else if (player == false && this.GetComponent<pocket>().get_st() == false)
  //         // {
  //         //   player = true;
  //         // }
  //       }
  //     }

  //     breakshots[3].clip = movesound[Random.Range(0, movesound.Length)];
  //     breakshots[3].volume = Mathf.Clamp01(rb.velocity.sqrMagnitude / 200);
  //     breakshots[3].Play();
  //   }
  // }
  {
    // Vector3 touchPosition;
    if (Input.touchCount > 0)
    {
      Touch touch = Input.GetTouch(0);
      touchPosition = cm.ScreenToWorldPoint(Input.mousePosition);

      // Raycast to check if the touch position is hitting the board collider
      RaycastHit2D hit = Physics2D.Raycast(touchPosition, Vector3.forward);

      if (hit.collider)
      {
        if (hit.transform.name == "striker")
        {
          // Debug.Log("Touch on striker");
          showGizmos = true;
        }

        if (showGizmos == true)
        {
          strikerPosition = this.transform.position;
          // powerCircle.transform.LookAt(hit.point);
          float scaleValue = 4f * Vector2.Distance(strikerPosition, touchPosition);
          // powerCircle.transform.localScale = Vector3.one * Mathf.Clamp(scaleValue, 0f, maxForce);
          powerCircle.transform.localScale = Vector3.one * scaleValue;

          // Draw Debug line for Force Vector
          Debug.DrawLine(strikerPosition, touchPosition, Color.blue);

          // Calculate the direction from start position to input position
          // Vector3 hitDirection = strikerPosition - touchPosition;

          // // Calculate the end position
          // Vector3 linePos = strikerPosition + hitDirection;
          // Calculate the direction from start position to input position
          targetDirection = strikerPosition - touchPosition;

          // Calculate the end position
          Vector3 linePos = strikerPosition + targetDirection;

          lr.positionCount = 2;
          lr.widthCurve = ac;
          lr.numCapVertices = 10;
          lr.SetPosition(0, strikerPosition);
          lr.SetPosition(1, linePos);
          lr.enabled = true;
        }
      }
    }
    else if (Input.touchCount == 0 && showGizmos == true)
    {
      showGizmos = false;
      powerCircle.transform.localScale = Vector3.zero;

      lr.enabled = false;

      if (targetDirection.magnitude > 10f)
      {
        breakshots[2].clip = hitsound[Random.Range(0, hitsound.Length)];
        breakshots[2].Play();


        float magnitude = 4f * Vector2.Distance(strikerPosition, touchPosition);
        // Vector3 forceVector = targetDirection.normalized * Mathf.Clamp(magnitude, 0f, maxForce);
        Vector3 forceVector = targetDirection.normalized * magnitude;
        Debug.Log(forceVector.magnitude);

        // 10 > is good
        rb.AddForce(forceVector * forceMultiplier);

        player = false; // Set this to False to Make Player One Always Take Turn
      }
    }
  }
  public void ReturnStriker()
  {
    if (player == false)
    {
      MoveStriker(false);
    }
    else
    {
      MoveStriker(true);
    }
  }

  private void OnCollisionEnter2D(Collision2D other)
  {
    if (other.gameObject.tag == "black" || other.gameObject.tag == "white" || other.gameObject.tag == "red")
    {
      if (board.GetComponent<Collider2D>().enabled == false)
      {
        breakshots[1].clip = hits[Random.Range(0, hits.Length)];
        breakshots[1].volume = Mathf.Clamp01(other.relativeVelocity.magnitude / 30);
        breakshots[1].Play();
      }
    }
  }
  void OnTriggerEnter2D(Collider2D other)
  {
    if (other.gameObject.tag == "board")
    {
      breakshots[0].volume = Mathf.Clamp01(rb.velocity.sqrMagnitude / 200);
      board.GetComponent<Collider2D>().enabled = false;
      breakshots[0].Play();
    }
  }
}