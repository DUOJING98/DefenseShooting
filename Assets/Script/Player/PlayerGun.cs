using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    [Header("Fire point & Prefab")]
    [SerializeField] GameObject normalBulletPrefab;
    [SerializeField] GameObject LaserBeamPrefab;
    [SerializeField] Transform firePoint;

    [Header("Time Setting")]
    [SerializeField] float chargeThresGold = 0.3f;   
    [SerializeField] float maxChargeTime = 2f;   


    private Camera mainCamera;
    private float chargeTime = 0.0f;
    [SerializeField] bool isCharging = false;



    private void Start()
    {
        mainCamera = Camera.main;
        //FireLaserBeam(2f);
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
            StartCharging();
        }
      if(Input.GetMouseButton(0) && isCharging)
        {
            UpdateCharging();
        }
        if (Input.GetMouseButtonUp(0) && isCharging)
        {
            ReleaseCharge();
        }
    }

 

    void StartCharging()
    {
        isCharging=true;
        chargeTime = 0.0f;
    }

    private void UpdateCharging()
    {
        chargeTime += Time.deltaTime;
        chargeTime= Mathf.Min(chargeTime,maxChargeTime);
    }

    void ReleaseCharge()
    {
        float ratio  = chargeTime / maxChargeTime;

        if(chargeTime < chargeThresGold)
        {
            FireNormalBullet();
        }
        else
        {
            FireLaserBeam(ratio);
        }
        isCharging = false;
    }

    void FireNormalBullet()
    {
        GameObject bullet = Instantiate(normalBulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb  = bullet.GetComponent<Rigidbody2D>();
    }

    void FireLaserBeam(float chargeRatio)
    {

        
        GameObject laser = Instantiate(LaserBeamPrefab, firePoint.position, firePoint.rotation);
        LaserBeam laserSC = laser.GetComponent<LaserBeam>();
        if(laserSC != null)
        {
            laserSC.SetPower(chargeRatio);
        }
    }
}
