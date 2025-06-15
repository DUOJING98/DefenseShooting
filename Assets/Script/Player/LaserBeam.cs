//using UnityEngine;

//public class LaserBeam : MonoBehaviour
//{
//    [SerializeField] float duration = 1f;
//    [SerializeField] float baseWidth = 1f;
//    [SerializeField] float maxWidth = 10f;
//    [SerializeField] float length = 30f; // º§π‚æ‡¿Î

//    private float timer;
//    private LineRenderer Lr;
//    private BoxCollider2D bx;

//    private void Start()
//    {
//        Lr = GetComponent<LineRenderer>();
//        bx = GetComponent<BoxCollider2D>();
//        timer = 0f;
//    }

//    public void SetPower(float chargeRatio)
//    {
//        float width = Mathf.Lerp(baseWidth, maxWidth, chargeRatio);

//        if (Lr != null)
//        {
//            Lr.startWidth = width;
//            Lr.endWidth = width;

            
//            Vector3 dir = transform.right; 
//            Vector3 start = transform.position;
//            Vector3 end = start + dir * length;

            
//            Lr.SetPosition(0, start);
//            Lr.SetPosition(1, end);
//        }

//        if (bx != null)
//        {
//            bx.offset = new Vector2(length / 2f, 0f);   
//            bx.size = new Vector2(length, width);       
//        }
//    }

//    private void Update()
//    {
//        timer += Time.deltaTime;
//        if (timer >= duration)
//        {
//            Destroy(gameObject);
//        }
//    }

//    private void OnTriggerEnter2D(Collider2D collision)
//    {
        
//        //if(collision.CompareTag("Enemy"))
//        //{
//        //    Destroy(collision.gameObject);
//        //}
//    }
//}
