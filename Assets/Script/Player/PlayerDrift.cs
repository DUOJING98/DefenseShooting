using UnityEngine;

public class PlayerDrift : MonoBehaviour
{
    [SerializeField] float driftRadius = 0.5f;
    [SerializeField] float driftSpeed = 1f;

    private Vector3 startPos;
    private Vector3 TargetOffset;

    private void Start()
    {
        startPos = transform.position;
        PickNewTargetOffset();
    }

    private void Update()
    {
        transform.position  = Vector3.Lerp(transform.position, startPos+TargetOffset, driftRadius*Time.deltaTime);
        if(Vector3.Distance(transform.position, startPos + TargetOffset) < 0.05f)
        {
            PickNewTargetOffset();
        }
    }

    void PickNewTargetOffset()
    {
        float offsetX  = Random.Range(-driftRadius, driftRadius);
        float offsetY  = Random.Range(-driftRadius, driftRadius);
        TargetOffset = new Vector3(offsetY, offsetX, 0);
    }
}
