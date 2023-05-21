using UnityEngine;

namespace Misc
{
    public class EnemyMovePoint : MonoBehaviour
    {
        [SerializeField] private Color _gizmoColor;
        [SerializeField] private float _gizmoRadius = 1;

        private void OnDrawGizmos()
        {
            Gizmos.color = _gizmoColor;
            Gizmos.DrawRay(transform.position, transform.forward * 5);
            Gizmos.DrawSphere(transform.position, _gizmoRadius);
        }
    }
}

