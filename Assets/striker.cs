using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class striker : MonoBehaviour
{
  Vector3 startPos, endPos, targetDirection;

  // TODO: Instea of Array, do something else
  public AudioSource[] breakshots;
  public AudioClip[] hitsound, breaksound, hits, pocketfillsound, movesound;

  public float resetThresholdVelocity = 0.07f;
  public GameObject board;
  bool player = false;
  public int black, white, red;
  public bool coveringTheQueen = false;
  // public Slider move_slider;

  Camera cm;
  LineRenderer lr;
  GameObject start;

  Rigidbody2D rb;
  // bool hit = false;
  public bool st = false;
  public bool movestriker = false;
  public bool turn = false;
  Vector3 camoffset = new(0, 0, 0);
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
    // Debug.Log(rb.velocity.magnitude);
    // if (rb.velocity == Vector2.zero)
    if (rb.velocity.magnitude < resetThresholdVelocity)
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
    // if (rb.velocity == Vector2.zero)
    if (rb.velocity.magnitude < resetThresholdVelocity)
    {
      if (t == false)
      {
        // this.transform.position = new Vector3(move_slider.value, -1.575f, 0);
        this.transform.position = new Vector3(0, -1.575f, 0);
      }
      else
      {
        // this.transform.position = new Vector3(move_slider.value, 1.575f, 0);
        this.transform.position = new Vector3(0, 1.575f, 0);
      }
    }
    // player = t;
  }
  public bool GetStriker()
  {
    return player;
  }
  private void control()
  {
    if (Input.mousePosition.y > 150f)
    {
      startPos = this.transform.position + camoffset;

      lr.positionCount = 2;
      lr.widthCurve = ac;
      lr.numCapVertices = 10;
      lr.enabled = false;
      lr.SetPosition(0, startPos);

      if (Input.GetMouseButton(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
      // {
      //   endPos = cm.ScreenToWorldPoint(Input.mousePosition) + camoffset;
      //   lr.SetPosition(1, endPos);

      //   lr.enabled = true;
      // }
      {
        endPos = cm.ScreenToWorldPoint(Input.mousePosition) + camoffset;

        // Calculate the direction from start position to input position
        Vector3 hitDirection = startPos - endPos;

        // Calculate the end position
        Vector3 linePos = startPos + hitDirection;

        // Draw a debug line from startPos to endPos
        Debug.DrawLine(startPos, endPos, Color.blue);

        // Set the end position for the line renderer
        lr.SetPosition(1, linePos);

        // Enable the line renderer
        lr.enabled = true;
      }

      if (Input.GetMouseButtonUp(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended))
      {
        lr.enabled = false;
        // hit = true;
        st = true;
        targetDirection = endPos - startPos;
        Vector3 testtargetDirection = startPos - endPos;

        // Draw a debug line to visualize the target direction
        Debug.DrawLine(startPos, endPos, Color.green, 2f);

        if (targetDirection.magnitude > 10.007f)
        {
          lr.material.color = Color.red;

          breakshots[2].clip = hitsound[Random.Range(0, hitsound.Length)];
          breakshots[2].Play();

          rb.AddForce(testtargetDirection * 250);

          player = false; // Set this to False to Make Player One Always Take Turn

          // if (player == true && this.GetComponent<pocket>().get_st() == false)
          // {
          //   player = false;
          // }
          // else if (player == false && this.GetComponent<pocket>().get_st() == false)
          // {
          //   player = true;
          // }
        }
      }

      breakshots[3].clip = movesound[Random.Range(0, movesound.Length)];
      breakshots[3].volume = Mathf.Clamp01(rb.velocity.sqrMagnitude / 200);
      breakshots[3].Play();
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
      Debug.Log("Striked");
      breakshots[0].volume = Mathf.Clamp01(rb.velocity.sqrMagnitude / 200);
      board.GetComponent<Collider2D>().enabled = false;
      breakshots[0].Play();
    }
  }
}