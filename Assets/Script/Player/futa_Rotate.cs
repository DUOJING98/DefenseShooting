using UnityEngine;

public class futa : MonoBehaviour
{
    public float rotateSpeed = 200f;
    public float targetAngle = 33f;
    private float originalAngle;
    private bool returning = false;

    void Start()
    {
        originalAngle = transform.eulerAngles.z;
    }

    void Update()
    {
        float currentZ = transform.eulerAngles.z;
        // 转换到 -180 到 180 区间，方便旋转比较
        if (currentZ > 180f) currentZ -= 360f;

        // 按住右键时：旋转到目标角度
        if (Input.GetMouseButton(1))
        {
            float angle = Mathf.MoveTowardsAngle(currentZ, targetAngle, rotateSpeed * Time.deltaTime);
            transform.eulerAngles = new Vector3(0, 0, angle);
            returning = false;
        }
        // 松开右键时：返回原始角度
        else
        {
            float angle = Mathf.MoveTowardsAngle(currentZ, originalAngle, rotateSpeed * Time.deltaTime);
            transform.eulerAngles = new Vector3(0, 0, angle);
            returning = true;
        }
    }
}
