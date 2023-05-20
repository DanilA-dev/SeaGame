using System;
using UnityEngine;

namespace Player
{
    public class PlayerInput : MonoBehaviour
    {
        private Camera _cam;
        private Vector3 _mousePoint;
        
        public Action<Vector2> MousePointUpdate;
        public Action OnLMBPress;

        private void Awake()
        {
            _cam = Camera.main;
        }

        private void Update()
        {
            Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                _mousePoint = hit.point;
                MousePointUpdate?.Invoke(_mousePoint);
            }
            
            if(Input.GetMouseButton(0))
                OnLMBPress?.Invoke();
        }
    }
}