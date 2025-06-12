using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    [SerializeField] float duration = 0.5f;
    [SerializeField] float baseWidth = 0.2f;
    [SerializeField] float maxWidth = 1f;

    private float timer;
    private LineRenderer Lr;
    private BoxCollider2D bx;

    private void Start()
    {
        Lr = GetComponent<LineRenderer>();
        bx = GetComponent<BoxCollider2D>();
        timer = 0f;
    }

    public void SetPower(float chargeRatio)
    {
        float width = Mathf.Lerp(baseWidth, maxWidth, chargeRatio);

        if (Lr != null)
        {
            Lr.startWidth = width;
            Lr.endWidth = width;
        }

        if (bx != null)
        {
            bx.size = new Vector2(bx.size.x, width);
        }

    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= duration)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //“–‚½‚è”»’è
        //if(collision.CompareTag("Enemy"))
        //{
        //    Destroy(collision.gameObject);
        //}
    }
}
