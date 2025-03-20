using UnityEngine;

namespace _Game.Systems.MovementSystem
{
    public class ParallaxObject : MonoBehaviour
    {
        [SerializeField] private float parallaxSpeedMultiplier = 0.5f;

        void Update()
        {
            transform.position += Vector3.back * (PlatformMovement.Instance.PlatformSpeed * parallaxSpeedMultiplier * Time.deltaTime);
        }
    }
}