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
    }

}
