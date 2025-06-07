using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    [SerializeField] Transform firePoint;
    [SerializeField] private GameObject bulletParticlePrefab;
    private Camera mainCamera;


    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        AimAtMouse();
        HandleShooting();
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
            Shoot();
        }
    }

    void Shoot()
    {
        Instantiate(bulletParticlePrefab, firePoint.position, firePoint.rotation);
    }
}
