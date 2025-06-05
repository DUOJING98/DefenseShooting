using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    [SerializeField] Transform firePoint;

    private Camera mainCamera;


    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        AimAtMouse();
    }

    private void AimAtMouse()
    {
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mouseWorldPos - transform.position;
        float angle = Mathf.Atan2 (direction.y, direction.x)*Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0,0,angle);
    }

    void HandleShooting()
    {
        if(Input.GetMouseButtonDown(0))
        {
            //shoot
        }
    }

    void Shoot()
    {

    }
}
