using UnityEngine;

public class futa : MonoBehaviour
{
    public float rotateSpeed = 200f;
    public float targetAngle = 33f;
    private float originalAngle;
    private float currentZ;
    private bool opening = false;
    private bool closing = false;

    void Start()
    {
        originalAngle = transform.eulerAngles.z;
        currentZ = originalAngle;
    }

    void Update()
    {
        if (opening)
        {
            currentZ = Mathf.MoveTowardsAngle(currentZ, targetAngle, rotateSpeed * Time.deltaTime);
            transform.eulerAngles = new Vector3(0, 0, currentZ);
            if (Mathf.Abs(Mathf.DeltaAngle(currentZ, targetAngle)) < 0.5f)
                opening = false;
        }
        else if (closing)
        {
            currentZ = Mathf.MoveTowardsAngle(currentZ, originalAngle, rotateSpeed * Time.deltaTime);
            transform.eulerAngles = new Vector3(0, 0, currentZ);
            if (Mathf.Abs(Mathf.DeltaAngle(currentZ, originalAngle)) < 0.5f)
                closing = false;
        }
    }

    public void Open() => opening = true;
    public void Close() => closing = true;
    public bool IsOpened() => Mathf.Abs(Mathf.DeltaAngle(currentZ, targetAngle)) < 1f;
}
