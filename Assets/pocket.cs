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
                StartCoroutine(FallCoroutine(true));
                GameManager.Instance.GetCurrentPlayer().OnFoul();
                Vector2 currentDirection = rb.velocity.normalized;
                rb.velocity = currentDirection.normalized * 0.07f;
                break;
            case "black":
                StartCoroutine(FallCoroutine(false));
                GameManager.Instance.AddCoinToCurrentPlayer(CoinType.Black);
                rb.velocity = Vector2.zero;
                // Destroy(this.gameObject);
                break;
            case "red":
                StartCoroutine(FallCoroutine(false));
                GameManager.Instance.AddCoinToCurrentPlayer(CoinType.Red);
                rb.velocity = Vector2.zero;
                // Destroy(this.gameObject);
                break;
            case "white":
                StartCoroutine(FallCoroutine(false));
                GameManager.Instance.AddCoinToCurrentPlayer(CoinType.White);
                rb.velocity = Vector2.zero;
                // Destroy(this.gameObject);
                break;

            default:
                break;
        }
    }

    void HandleStrikerFoul()
    {
        Debug.Log("Foul");
    }
    void HandleCoinFall()
    {
        Debug.Log("coin");
    }

    IEnumerator FallCoroutine(bool isStriker)
    {
        anim.SetTrigger(isStriker ? "Striker Fall" : "Coin Fall");
        yield return new WaitForSeconds(1f);

        if (isStriker)
        {
            HandleStrikerFoul();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}