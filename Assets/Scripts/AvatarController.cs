using UnityEngine;

/// <summary>
/// We do not need to apply the head rotation data
/// because we already have a Multi-Aim constraint for the head
/// Meaning that the head looks where the Camera looks
/// But we need to change its position along with the camera
/// if we move(the camera is the head, so if the camera moves),
/// the avatar moves as well
/// </summary>
[System.Serializable]
public class MapTransformHead
{
    public Transform vrTarget;
    public Transform IKTarget;
    public Vector3 trackingPositionOffset;
    public Vector3 trackingRotationOffset;

    public void MapVRAvatar()
    {
        IKTarget.position = vrTarget.TransformPoint(trackingPositionOffset);
    }
}

/// <summary>
/// This Map Transform class is used for mapping the hand tracking data from Manomotion
/// to the left hand of the avatar(everything could be copy-pasted for the right hand)
/// Position of the first joint is used for mapping
/// We need to map both the rotation and position of the hand
/// </summary>
[System.Serializable]
public class MapTransform
{
    public Transform vrTarget;
    public Transform IKTarget;
    public Vector3 trackingPositionOffset;
    public Vector3 trackingRotationOffset;

    public void MapVRAvatar()
    {
        IKTarget.position = vrTarget.TransformPoint(trackingPositionOffset);
        IKTarget.rotation = vrTarget.rotation * Quaternion.Euler(trackingRotationOffset);
    }
}

/// <summary>
/// This Map Transform class is used for mapping the figner tracking data from Manomotion
/// to the fingers of the avatar
/// We need to map both the position of the finger
/// as we have a Chain IK for each finger tip
/// </summary>
[System.Serializable]
public class MapTransformFinger
{
    public Transform vrTarget;
    public Transform IKTarget;
    public Vector3 trackingPositionOffset;
    public Vector3 trackingRotationOffset;

    public void MapVRAvatar()
    {
        IKTarget.position = vrTarget.TransformPoint(trackingPositionOffset);
    }
}

/// <summary>
/// This class is used for mapping the hand tracking and head tracking data
/// to the avatar.
/// Head = Camera
/// Hand = Manomotion Hand tracking data
/// That is how we could simulate a headset
/// </summary>
///
public class AvatarController : MonoBehaviour
{
    //Head, Left Hand, and Left Hand Fingers Map Transforms
    [SerializeField] private MapTransformHead head;
    [SerializeField] private MapTransform leftHand;
    [SerializeField] private MapTransformFinger [] leftHandFingers;

    //Additional variables for mapping the tracking data properly
    [SerializeField] private float turnSmoothness;
    [SerializeField] private Transform IKHead;
    [SerializeField] private Vector3 headBodyOffset;

    private void Start()
    {
        //calculating the offset between the avatar and the head
        headBodyOffset = transform.position - IKHead.position;
    }

    void LateUpdate()
    {
        //In each frame, we basically update the the positions of avatar's head, left hand, and fingers 
        transform.position = IKHead.position + headBodyOffset;
        transform.forward = Vector3.Lerp(transform.forward, Vector3.ProjectOnPlane(IKHead.forward, Vector3.up).normalized, Time.deltaTime * turnSmoothness);
        head.MapVRAvatar();
        leftHand.MapVRAvatar();
        foreach (var fingerMap in leftHandFingers)
        {
            fingerMap.MapVRAvatar();
        }
    }
}