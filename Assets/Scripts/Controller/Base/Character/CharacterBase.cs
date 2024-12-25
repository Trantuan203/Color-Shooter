using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterBase : MonoBehaviour
{
    #region Components
    [Header("---------Components---------")]
    protected Rigidbody2D rb;
    protected Animator anim;
    protected BoxCollider2D collid;
    protected GameObject currentOneWayPlatform;
    [SerializeField] protected List<GameObject> platformCantFall;
    #endregion

    #region Movement
    [Header("---------Movement---------")]
    public float speed = 200.0f;
    public float jumpForce = 18.0f;
    #endregion

    #region Ground Check
    [Header("---------Ground Check---------")]
    [SerializeField] protected LayerMask groundLayer;
    [SerializeField] protected Transform groundCheck;
    #endregion

    [Header("---------Color---------")]
    public CharacterColor currentColor = CharacterColor.GRAY;

    [Header("Stats")]
    public bool isDead = false;

    [HideInInspector] public int character_dir = 1; // Huong cua nhan vat, de tinh toan huong cua vien dan
    protected bool facing_right = true;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        collid = GetComponent<BoxCollider2D>();
    }


    public void Flip()
    {
        character_dir *= -1;
        facing_right = !facing_right;
        rb.transform.Rotate(0, 180, 0);
    }


    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(GameConstants.oneWayPlatform) && !platformCantFall.Contains(collision.gameObject))
        {
            currentOneWayPlatform = collision.gameObject;
        }
    }

    protected void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(GameConstants.oneWayPlatform))
        {
            currentOneWayPlatform = null;
        }
    }

    protected IEnumerator DisableCollision()
    {
        BoxCollider2D platformCollider = currentOneWayPlatform.GetComponent<BoxCollider2D>();

        Physics2D.IgnoreCollision(collid, platformCollider);
        yield return new WaitForSeconds(0.5f);
        Physics2D.IgnoreCollision(collid, platformCollider, false);
    }

    public bool IsGround()
    {
        RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, Vector2.down, 0.05f, groundLayer);
        return hit.collider != null;
    }

    protected void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, Vector2.down, 0.05f, groundLayer);

        Gizmos.DrawRay(groundCheck.position, Vector2.down * 0.05f);

        if (hit.collider != null)
        {
            Gizmos.DrawSphere(hit.point, 0.05f);
        }
    }
}
