using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    [SerializeField] float duration = 0.5f;

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
        float width = Mathf.Lerp(0.2f,1.0f,chargeRatio);
        Lr.startWidth = width;
        Lr.endWidth = width;

        bx.size = new Vector2(bx.size.x, width);
    }

    private void Update()
    {
        timer+= Time.deltaTime;
        if(timer>duration)
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
