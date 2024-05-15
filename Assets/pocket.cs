using System.Collections;
using UnityEngine;

public class pocket : MonoBehaviour
{
    Animator anim;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        GameManager.Instance.OnStrikerFoul += HandleStrikerFoul;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag != "pocket") return;

        switch (this.gameObject.tag)
        {
            case "striker":
                // StartCoroutine(FallCoroutine(true));
                GameManager.Instance.GetCurrentPlayer().OnFoul();
                Vector2 currentDirection = rb.velocity.normalized;
                rb.velocity = currentDirection.normalized * 0.07f;
                break;
            case "black":
                // StartCoroutine(FallCoroutine(false));
                GameManager.Instance.AddCoinToCurrentPlayer(CoinType.Black);
                rb.velocity = Vector2.zero;
                // Destroy(this.gameObject);
                HandlePawnFall();
                break;
            case "red":
                // StartCoroutine(FallCoroutine(false));
                GameManager.Instance.AddCoinToCurrentPlayer(CoinType.Red);
                rb.velocity = Vector2.zero;
                // Destroy(this.gameObject);
                HandlePawnFall();
                break;
            case "white":
                // StartCoroutine(FallCoroutine(false));
                GameManager.Instance.AddCoinToCurrentPlayer(CoinType.White);
                rb.velocity = Vector2.zero;
                // Destroy(this.gameObject);
                HandlePawnFall();
                break;

            default:
                break;
        }
    }

    void HandleStrikerFoul()
    {
        // Debug.Log("Foul");
    }

    void HandlePawnFall()
    {
        transform.localScale = Vector3.one * 0.56f * 0.9f;
        GetComponent<SpriteRenderer>().color = new Color(180f / 255f, 180f / 255f, 180f / 255f);
        StartCoroutine(FallCoroutine());
    }

    IEnumerator FallCoroutine()
    {
        // anim.SetTrigger(isStriker ? "Striker Fall" : "Coin Fall");
        yield return new WaitForSeconds(0.1f);
        Destroy(this.gameObject);

        // if (isStriker)
        // {
        //     Debug.Log("Foul");
        // }
        // else
        // {
        //     Debug.Log("coin");
        // }
    }
}