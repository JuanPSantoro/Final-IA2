using UnityEngine;

public class TimeUI : MonoBehaviour
{
    [SerializeField]
    private GameObject _clock = default;

    public void UpdateClock(float timeOfDay)
    {
        float zRot = Mathf.Lerp(0f, 360f, timeOfDay / 24f);
        Vector3 eulerAngles = _clock.transform.localRotation.eulerAngles;
        eulerAngles.z = zRot;
        _clock.transform.localRotation = Quaternion.Euler(eulerAngles);
    }
}
