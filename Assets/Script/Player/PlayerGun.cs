using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    [Header("Fire point & Prefab")]
    [SerializeField] GameObject normalBulletPrefab;
    [SerializeField] GameObject hdrLaserRectPrefab;
    [SerializeField] Transform NormalfirePoint;
    [SerializeField] Transform LaserfirePoint;
    [SerializeField] GameObject hdrLaserPreviewPrefab;

    [Header("Head Control")]
    [SerializeField] futa headController;

    [Header("Time Setting")]
    [SerializeField] float chargeThresGold = 0.3f;
    [SerializeField] float maxChargeTime = 3f;

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
    private bool headOpenTriggered = false;
    private bool previewShown = false;

    private GameObject previewLaserInstance;

    private void Start()
    {
        mainCamera = Camera.main;
        chargeParticle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }

    private void Update()
    {
        AimAtMouse(); // 普通子弹方向
        HandleShooting();
    }

    private void AimAtMouse()
    {
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mouseWorldPos - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void HandleShooting()
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

    private void StartCharging()
    {
        isCharging = true;
        chargeTime = 0.0f;
        previewShown = false;
        headOpenTriggered = false;

        chargeParticle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }

    private void UpdateCharging()
    {
        chargeTime += Time.deltaTime;
        chargeTime = Mathf.Min(chargeTime, maxChargeTime);

        // 达到蓄力阈值后展开头部（只执行一次）
        if (!headOpenTriggered && chargeTime >= chargeThresGold)
        {
            headOpenTriggered = true;
            headController.Open();
        }

        // 头部没展开完成就不继续预览线和粒子
        if (!headController.IsOpened()) return;

        // 固定粒子位置
        chargeParticle.transform.position = LaserfirePoint.position;
        chargeParticle.transform.rotation = Quaternion.identity;

        // 启动粒子（只开一次）
        if (!chargeParticle.isPlaying)
            chargeParticle.Play();

        // 粒子速度按蓄力进度变化
        float ratio = chargeTime / maxChargeTime;
        var emission = chargeParticle.emission;
        emission.rateOverTime = Mathf.Lerp(minRate, maxRate, ratio);

        // 创建预览激光
        if (!previewShown && chargeTime >= chargeThresGold + 0.1f)
        {
            previewLaserInstance = Instantiate(hdrLaserPreviewPrefab);
            previewLaserInstance.transform.position = LaserfirePoint.position;
            previewLaserInstance.transform.rotation = Quaternion.identity;
            previewLaserInstance.transform.localScale = new Vector3(laserLength, 0.05f, 1);
            previewShown = true;
        }

        // 预览线宽度变粗 + 闪烁
        if (previewLaserInstance != null)
        {
            previewLaserInstance.transform.position = LaserfirePoint.position;
            previewLaserInstance.transform.rotation = Quaternion.identity;

            float adjustedRatio = (chargeTime - chargeThresGold) / (maxChargeTime - chargeThresGold);
            float width = Mathf.Lerp(0.05f, 0.5f, Mathf.Clamp01(adjustedRatio));
            previewLaserInstance.transform.localScale = new Vector3(laserLength, width, 1);

            if (ratio >= 1f)
            {
                var sprite = previewLaserInstance.GetComponent<SpriteRenderer>();
                if (sprite != null)
                {
                    float glow = Mathf.PingPong(Time.time * 5f, 1f);
                    sprite.color = new Color(1f, 1f, 1f, 0.5f + glow * 0.5f);
                }
            }
        }
    }

    private void ReleaseCharge()
    {
        float ratio = chargeTime / maxChargeTime;

        if (previewLaserInstance != null)
        {
            Destroy(previewLaserInstance);
            previewLaserInstance = null;
        }

        isCharging = false;
        headOpenTriggered = false;
        previewShown = false;

        if (chargeTime >= chargeThresGold)
        {
            FireHDRLaser(ratio);
        }
        else
        {
            FireNormalBullet();
        }

        chargeParticle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        headController.Close();
    }

    private void FireNormalBullet()
    {
        GameObject bullet = Instantiate(normalBulletPrefab, NormalfirePoint.position, NormalfirePoint.rotation);
    }

    private void FireHDRLaser(float chargeRatio)
    {
        float width = Mathf.Lerp(minLaserWidth, maxLaserWidth, chargeRatio);
        GameObject laser = Instantiate(hdrLaserRectPrefab, LaserfirePoint.position, Quaternion.identity);
        laser.transform.localScale = new Vector3(laserLength, width, 1);

        Vector2 center = (Vector2)LaserfirePoint.position + Vector2.right * (laserLength / 2f);
        Vector2 size = new Vector2(laserLength, width);

        Collider2D[] hits = Physics2D.OverlapBoxAll(center, size, 0f, enemyLayer);
        foreach (var hit in hits)
        {
            Destroy(hit.gameObject);
        }

        Destroy(laser, 0.3f);
    }
}
