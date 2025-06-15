using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    [Header("Fire point & Prefab")]
    [SerializeField] GameObject normalBulletPrefab;
    [SerializeField] GameObject hdrLaserRectPrefab;
    [SerializeField] Transform firePoint;
    [SerializeField] GameObject hdrLaserPreviewPrefab;
    private GameObject previewLaserInstance;

    [Header("Time Setting")]
    [SerializeField] float chargeThresGold = 0.3f;
    [SerializeField] float maxChargeTime = 2f;

    [Header("Charge FX")]
    [SerializeField] ParticleSystem chargeParticle;
    [SerializeField] float minRate = 10f;
    [SerializeField] float maxRate = 100f;

    [Header("Laser Settings")]
    [SerializeField] float laserLength = 20f;
    [SerializeField] float minLaserWidth = 2f;
    [SerializeField] float maxLaserWidth = 10f;
    [SerializeField] LayerMask enemyLayer;

    private Camera mainCamera;
    private float chargeTime = 0.0f;
    private bool isCharging = false;

    private float previewDelay = 0.5f;
    private bool previewShown = false;

    private void Start()
    {
        mainCamera = Camera.main;
        chargeParticle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
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
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void HandleShooting()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartCharging();
        }
        if (Input.GetMouseButton(0) && isCharging)
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
        isCharging = true;
        chargeTime = 0.0f;
        previewShown = false;

        chargeParticle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        chargeParticle.Play();
    }

    private void UpdateCharging()
    {
        chargeTime += Time.deltaTime;
        chargeTime = Mathf.Min(chargeTime, maxChargeTime);

        float ratio = chargeTime / maxChargeTime;
        var emission = chargeParticle.emission;
        emission.rateOverTime = Mathf.Lerp(minRate, maxRate, ratio);

        // 满足延迟条件后生成预览激光
        if (!previewShown && chargeTime >= previewDelay)
        {
            previewLaserInstance = Instantiate(hdrLaserPreviewPrefab, firePoint.position, firePoint.rotation);
            previewLaserInstance.transform.localScale = new Vector3(laserLength, 0.05f, 1);
            previewShown = true;
        }

        // 更新预览激光的位置、旋转和宽度（仅在显示后才变粗）
        if (previewLaserInstance != null && previewShown)
        {
            float adjustedRatio = (chargeTime - previewDelay) / (maxChargeTime - previewDelay);
            float width = Mathf.Lerp(0.05f, 0.5f, Mathf.Clamp01(adjustedRatio));

            previewLaserInstance.transform.position = firePoint.position;
            previewLaserInstance.transform.rotation = firePoint.rotation;
            previewLaserInstance.transform.localScale = new Vector3(laserLength, width, 1);
        }
    }

    void ReleaseCharge()
    {
        float ratio = chargeTime / maxChargeTime;

        if (previewLaserInstance != null)
        {
            Destroy(previewLaserInstance);
            previewLaserInstance = null;
        }
        previewShown = false;

        if (chargeTime >= chargeThresGold)
        {
            FireHDRLaser(ratio);
        }
        else
        {
            FireNormalBullet();
        }

        isCharging = false;
        chargeParticle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }

    void FireNormalBullet()
    {
        GameObject bullet = Instantiate(normalBulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        // 可加入子弹速度逻辑
    }

    void FireHDRLaser(float chargeRatio)
    {
        float width = Mathf.Lerp(minLaserWidth, maxLaserWidth, chargeRatio);

        GameObject laser = Instantiate(hdrLaserRectPrefab, firePoint.position, firePoint.rotation);
        laser.transform.localScale = new Vector3(laserLength, width, 1);

        Vector2 center = (Vector2)firePoint.position + (Vector2)firePoint.right * (laserLength / 2f);
        Vector2 size = new Vector2(laserLength, width);
        float angle = firePoint.eulerAngles.z;

        Collider2D[] hits = Physics2D.OverlapBoxAll(center, size, angle, enemyLayer);
        foreach (var hit in hits)
        {
            Destroy(hit.gameObject);
        }

        Destroy(laser, 0.3f);
    }
}
