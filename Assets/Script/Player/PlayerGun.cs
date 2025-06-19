using UnityEngine;
using UnityEngine.UI;

public class PlayerGun : MonoBehaviour
{
    [Header("Fire point & Prefab")]
    [SerializeField] GameObject normalBulletPrefab;
    [SerializeField] GameObject hdrLaserRectPrefab;
    [SerializeField] Transform NormalfirePoint;
    [SerializeField] Transform LaserfirePoint;
    [SerializeField] GameObject hdrLaserPreviewPrefab;
    [SerializeField] private Image chargeBarFill;  // UI图

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
    private bool futaOpened = false;

    private void Start()
    {
        mainCamera = Camera.main;
        chargeParticle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }

    private void Update()
    {
        if (Time.timeScale != 0)
        {
            AimAtMouse(); // 普通子弹方向
        }
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
        futaOpened = false;

        chargeParticle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        if (chargeBarFill != null)
        {
            chargeBarFill.fillAmount = 0f;

        }
    }

    private void UpdateCharging()
    {
        chargeTime += Time.deltaTime;
        chargeTime = Mathf.Min(chargeTime, maxChargeTime);

        if (chargeBarFill != null)
        {
            float uiratio = chargeTime / maxChargeTime;
            chargeBarFill.fillAmount = uiratio;

        }
        if (!headOpenTriggered && chargeTime >= chargeThresGold)
        {
            headOpenTriggered = true;
            headController.Open();
            futaOpened = true;
        }

        if (!headController.IsOpened()) return;

        chargeParticle.transform.position = LaserfirePoint.position;
        chargeParticle.transform.rotation = Quaternion.identity;

        if (!chargeParticle.isPlaying)
            chargeParticle.Play();

        float ratio = chargeTime / maxChargeTime;
        var emission = chargeParticle.emission;
        emission.rateOverTime = Mathf.Lerp(minRate, maxRate, ratio);



        if (!previewShown && chargeTime >= chargeThresGold + 0.1f)
        {
            previewLaserInstance = Instantiate(hdrLaserPreviewPrefab);
            previewLaserInstance.transform.position = LaserfirePoint.position;
            previewLaserInstance.transform.rotation = Quaternion.identity;
            previewLaserInstance.transform.localScale = new Vector3(laserLength, 0.05f, 1);
            previewShown = true;
        }

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

        if (chargeTime >= maxChargeTime)
        {
            FireHDRLaser(1f);
        }
        else
        {
            FireNormalBullet();
        }

        chargeParticle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        if (futaOpened && headController.IsOpened())
        {
            headController.Close();
        }

        futaOpened = false;

        if (chargeBarFill != null)
        {
            chargeBarFill.fillAmount = 0f;

        }
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
