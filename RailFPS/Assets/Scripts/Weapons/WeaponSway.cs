using Player.Combat;
using StageSystem;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    [SerializeField] private PlayerCombatHandler _playerCombatHandler;
    [SerializeField] private Transform _lookPoint;
    [SerializeField] private Vector2 _minMaxX;
    [SerializeField] private Vector2 _minMaxY;
    [SerializeField] private float _rotateSpeed;

    private Vector3 _worldPos;
    private Vector3 _screenPos;
    private Camera _cam;

    private void Start()
    {
        _cam = Camera.main;
    }

    private void Update()
    {
        if(GameHandler.Instance.State != GameState.Playing)
            return;
        MoveLookPoint();

        Vector3 target = _playerCombatHandler.CombatState == PlayerCombatState.Ready ||
                         _playerCombatHandler.CombatState == PlayerCombatState.Attacking
            ? _lookPoint.position
            : new Vector3(transform.position.x, transform.position.y, transform.position.z + 10);
        
        RotateWeaponToPoint(target);
    }

    private void RotateWeaponToPoint(Vector3 target)
    {
        Vector3 dir = target - transform.position;
        Quaternion q = Quaternion.LookRotation(dir);
        q.x = Mathf.Clamp(q.x, _minMaxX.x, _minMaxX.y);
        q.y = Mathf.Clamp(q.y, _minMaxY.x, _minMaxY.y);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, q, _rotateSpeed * Time.deltaTime);
    }

    private void MoveLookPoint()
    {
        _screenPos = Input.mousePosition;
        _screenPos.z = _cam.nearClipPlane + 10;
        _worldPos = _cam.ScreenToWorldPoint(_screenPos);
        _lookPoint.position = _worldPos;
    }
}
