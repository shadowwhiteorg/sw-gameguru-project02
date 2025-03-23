using System;
using System.Collections;
using _Game.DataStructures;
using _Game.Utils;
using UnityEngine;

namespace _Game.Systems.Core
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] private GameObject winCameraParent;
        [SerializeField] private float rotationSpeed = 10;

        private bool _canRotate;


        private void WinCameraAnimation()
        {
            winCameraParent.transform.rotation = Quaternion.Euler(Vector3.zero);
            _canRotate = true;
            StartCoroutine(RotateAroundYAxisInfinite());
        }
        
        private IEnumerator RotateAroundYAxisInfinite()
        {
            while (_canRotate)
            {
                winCameraParent.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
                yield return null;
            }
        }

        private void OnEnable()
        {
            EventBus.Subscribe<OnLevelWinEvent>(e=> WinCameraAnimation());
            EventBus.Subscribe<OnLevelInitializeEvent>(e=> _canRotate=false);
        }
        
        private void OnDisable()
        {
            EventBus.Unsubscribe<OnLevelWinEvent>(e=> WinCameraAnimation());
            EventBus.Unsubscribe<OnLevelInitializeEvent>(e=> _canRotate=false);
        }
    }
}