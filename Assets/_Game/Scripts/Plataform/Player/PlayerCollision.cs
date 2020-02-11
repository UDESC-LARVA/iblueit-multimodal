using Ibit.Core.Audio;
using NaughtyAttributes;
using System;
using System.Collections;
using UnityEngine;

namespace Ibit.Plataform
{
    public partial class Player
    {
        #region Events

        public event Action OnPlayerDeath;
        public event Action<GameObject> OnObjectHit;

        #endregion Events

        [SerializeField] [BoxGroup("Animation Control")] private Animator animator;
        [SerializeField] [BoxGroup("Properties")] private int invincibilityTime = 2;

        private bool isPlayerDead;
        private bool isInvulnerable;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            HitResult(collision.gameObject);
            OnObjectHit?.Invoke(collision.gameObject);
        }

        private void HitResult(GameObject hit)
        {
            if (hit.CompareTag("WaterTarget") || hit.CompareTag("AirTarget"))
            {
                SoundManager.Instance.PlaySound("TargetGet");
            }
            else if (hit.CompareTag("WaterObstacle") || hit.CompareTag("AirObstacle"))
            {
                TakeDamage();
                SoundManager.Instance.PlaySound("PlayerDamage");
            }
            else if (hit.CompareTag("RelaxObject"))
            {
                SoundManager.Instance.PlaySound("BonusTargetGet", true);
            }
        }

        /*  Somehow, the animation method is generating an 
            "Assertion Failed IsNormalized" kinda of an error.
            At the moment, DamageAnimation() is using a band-aid fix,
            to trick the player that it is playing an animation.
            Use the band-aid method until the correct method is fixed. */
        /// <summary>
        /// Disables Collision and plays invincible animation.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        private IEnumerator DisableCollisionForXSeconds(int i)
        {
            isInvulnerable = true;
            //animator.SetBool("DamageTaken", isInvulnerable);

            var component = this.GetComponent<Collider2D>();

            component.enabled = false;
            yield return new WaitForSeconds(i);
            component.enabled = true;

            isInvulnerable = false;
            //animator.SetBool("DamageTaken", isInvulnerable);
        }

        /// <summary>
        /// Band-aid fix. Check DisableCollisionForXSeconds() description.
        /// </summary>
        /// <returns></returns>
        private IEnumerator DamageAnimation(float duration)
        {
            var delta = 0f;
            var renderer = this.GetComponentInChildren<SpriteRenderer>();
            var delay = 0.2f;

            animator.enabled = false;

            while (delta <= duration)
            {
                renderer.enabled = !renderer.enabled;
                yield return new WaitForSeconds(delay);
                delta += delay;
            }

            renderer.enabled = true;
            animator.enabled = true;
        }

        /// <summary>
        /// Band-aid fix. Check DisableCollisionForXSeconds() description.
        /// </summary>
        private Coroutine _animRoutine;

        [Button("Take Damage")]
        private void TakeDamage()
        {
            if (isPlayerDead || isInvulnerable)
                return;

            StartCoroutine(DisableCollisionForXSeconds(invincibilityTime));

            // Band-aid fix. Remove once it's fixed.
            #region Damage Animation Band-Aid Fix.

            if (_animRoutine != null)
                StopCoroutine(_animRoutine);

            _animRoutine = StartCoroutine(DamageAnimation(invincibilityTime));

            #endregion Damage Animation Band-Aid Fix.

            heartPoints--;

            if (heartPoints > 0)
                return;

            isPlayerDead = true;
            OnPlayerDeath?.Invoke();
        }
    }
}