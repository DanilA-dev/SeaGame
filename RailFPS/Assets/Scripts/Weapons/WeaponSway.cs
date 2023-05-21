using StageSystem;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    [Header("Sway Settings")]
    [SerializeField] private float _smooth;
    [SerializeField] private float _multiplier;

    private void Update()
    {
        if(GameHandler.Instance.State != GameState.Playing)
            return;
        
        float mouseX = Input.GetAxisRaw("Mouse X") * _multiplier;
        float mouseY = Input.GetAxisRaw("Mouse Y") * _multiplier;

        Quaternion rotationX = Quaternion.AngleAxis(-mouseY, Vector3.right);
        Quaternion rotationY = Quaternion.AngleAxis(mouseX, Vector3.up);

        Quaternion targetRotation = rotationX * rotationY;

        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, _smooth * Time.deltaTime);
    }
}
