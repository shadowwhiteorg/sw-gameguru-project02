using _Game.DataStructures;
using UnityEngine;

namespace _Game.Systems.CharacterSystem
{
    public class CharacterBody : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        
        public void PlayIdleAnimation()
        {
            animator.SetTrigger(GameConstants.IdleAnimationTrigger);
        }
        
        public void PlayRunAnimation()
        {
            animator.SetTrigger(GameConstants.RunAnimationTrigger);
        }
        
        public void PlayFallAnimation()
        {
            animator.SetTrigger(GameConstants.FallAnimationTrigger);
        }

        public void PlayDanceAnimation()
        {
            animator.SetTrigger(GameConstants.DanceAnimationTrigger);
        }

        
    }
}