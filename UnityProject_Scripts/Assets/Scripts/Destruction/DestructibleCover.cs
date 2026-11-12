using UnityEngine;

namespace Nexus.Destruction
{
    public class DestructibleCover : MonoBehaviour
    {
        [Header("Resistencia")]
        [SerializeField] private float _maxHealth = 80f;
        private float _currentHealth;

        [Header("Efectos")]
        [SerializeField] private GameObject _debrisPrefab;
        [SerializeField] private AudioClip _breakAudio;

        public bool IsDestroyed => _currentHealth <= 0f;

        private void Awake()
        {
            _currentHealth = _maxHealth;
        }

        public void TakeDamage(float amount)
        {
            if (IsDestroyed) return;

            _currentHealth -= amount;
            if (_currentHealth <= 0f)
            {
                BreakCover();
            }
        }

        private void BreakCover()
        {
            if (_debrisPrefab != null)
            {
                Instantiate(_debrisPrefab, transform.position, transform.rotation);
            }

            if (_breakAudio != null)
            {
                AudioSource.PlayClipAtPoint(_breakAudio, transform.position);
            }

            Destroy(gameObject);
        }
    }
}
