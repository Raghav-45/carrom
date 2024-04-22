using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class striker : MonoBehaviour
{
  Vector3 startPos, endPos, targetDirection;

  public AudioSource[] breakshots;
  public AudioClip[] hitsound, breaksound, hits, pocketfillsound, movesound;

  public GameObject board;
  public GameObject game_over;
  public Text game_over_text;
  public Text black_no;
  public Text white_no;
  bool player = false;
  public int black, white, red;
  public bool coveringTheQueen = false;
  public Slider move_slider;

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
    black_no.text = black + "";
    white_no.text = white + "";

    // if (black == 9 && red == 1)
    // {
    //   game_over.SetActive(true);
    //   game_over_text.text = "Opponent Won";

    // }
    // else if (white == 9 && red == 1)
    // {
    //   game_over.SetActive(true);
    //   game_over_text.text = "You Won";

    // }
    if (rb.velocity == Vector2.zero)
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
    if (rb.velocity == Vector2.zero)
    {
      if (t == false)
      {
        this.transform.position = new Vector3(move_slider.value, -1.575f, 0);
      }
      else
      {
        this.transform.position = new Vector3(move_slider.value, 1.575f, 0);
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
      {
        endPos = cm.ScreenToWorldPoint(Input.mousePosition) + camoffset;
        lr.SetPosition(1, endPos);

        lr.enabled = true;
      }

      if (Input.GetMouseButtonUp(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended))
      {
        lr.enabled = false;
        // hit = true;
        st = true;
        targetDirection = endPos - startPos;

        // Draw a debug line to visualize the target direction
        Debug.DrawLine(startPos, endPos, Color.green, 2f);

        if (targetDirection.magnitude > 10.007f)
        {
          lr.material.color = Color.red;

          breakshots[2].clip = hitsound[Random.Range(0, hitsound.Length)];
          breakshots[2].Play();

          rb.AddForce(targetDirection * 200);

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
      Debug.Log("Striker Hit a Pawn");
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