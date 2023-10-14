using System;
using UniRx;
using UnityEngine;

namespace StageSystem
{
    public class RaillReachSignal
    {
        public int Index { get; private set; }

        public RaillReachSignal(int index)
        {
            Index = index;
        }
    }
    
    public class RaillPoint : MonoBehaviour
    {
        [SerializeField] private int _index;

        public void ReachPoint()
        {
            MessageBroker.Default.Publish(new RaillReachSignal(_index));
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(transform.position, 1f);
            
            Gizmos.color = Color.green;
            Gizmos.DrawRay(transform.position, transform.forward * 10);
        }
    }

}
