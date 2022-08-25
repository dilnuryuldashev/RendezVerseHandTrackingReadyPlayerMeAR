using UnityEngine;

[System.Serializable]
public class MapTransform
{
    public Transform vrTarget;
    public Transform vrTargetRotation;
    public Transform IKTarget;
    public Vector3 trackingPositionOffset;
    public Vector3 trackingRotationOffset;

    public void MapVRAvatar()
    {
        IKTarget.position = vrTarget.TransformPoint(trackingPositionOffset);
        //IKTarget.rotation = vrTargetRotation.rotation * Quaternion.Euler(trackingRotationOffset);
    }
}
public class AvatarController : MonoBehaviour
{
    [SerializeField] private MapTransform head;
    //[SerializeField] private MapTransform leftHand;
    //[SerializeField] private MapTransform rightHand;

    [SerializeField] private float turnSmoothness;

    [SerializeField] private Transform IKHead;

    [SerializeField] private Vector3 headBodyOffset;
    [SerializeField] private Vector3 cameraOffset;

    private void Start()
    {
        headBodyOffset = transform.position - IKHead.position;
        cameraOffset = transform.position - Camera.main.transform.position;
    }

    void LateUpdate()
    {
        transform.position = IKHead.position + headBodyOffset;// + cameraOffset;
        //transform.LookAt(Camera.main.transform);
        transform.forward = Vector3.Lerp(transform.forward, Vector3.ProjectOnPlane(IKHead.forward, Vector3.up).normalized, Time.deltaTime * turnSmoothness);
        head.MapVRAvatar();
        //leftHand.MapVRAvatar();
        //rightHand.MapVRAvatar();
    }
}