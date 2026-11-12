using UnityEngine;
using System;

namespace Nexus.Match
{
    public class MatchManager : MonoBehaviour
    {
        [Header("Reglas 5v5")]
        public int TotalRoundsToWin = 5;
        public float RoundDurationSeconds = 120f;
        public int AlliesAlive = 5;
        public int EnemiesAlive = 5;

        public int AlliesScore { get; private set; } = 0;
        public int EnemiesScore { get; private set; } = 0;
        public float RoundTimer { get; private set; }

        public event Action<int, int> OnScoreUpdated;
        public event Action<bool> OnRoundCompleted;

        private void Start()
        {
            StartNewRound();
        }

        public void StartNewRound()
        {
            RoundTimer = RoundDurationSeconds;
            AlliesAlive = 5;
            EnemiesAlive = 5;
            Debug.Log("[MatchManager] Nueva ronda 5v5 iniciada.");
        }

        private void Update()
        {
            if (RoundTimer > 0)
            {
                RoundTimer -= Time.deltaTime;
                if (RoundTimer <= 0)
                {
                    OnRoundTimerExpired();
                }
            }
        }

        public void OnParticipantEliminated(bool isAlly)
        {
            if (isAlly) AlliesAlive--;
            else EnemiesAlive--;

            if (EnemiesAlive <= 0)
            {
                AlliesScore++;
                OnScoreUpdated?.Invoke(AlliesScore, EnemiesScore);
                OnRoundCompleted?.Invoke(true);
            }
            else if (AlliesAlive <= 0)
            {
                EnemiesScore++;
                OnScoreUpdated?.Invoke(AlliesScore, EnemiesScore);
                OnRoundCompleted?.Invoke(false);
            }
        }

        private void OnRoundTimerExpired()
        {
            // Victoria de defensores por tiempo
            EnemiesScore++;
            OnScoreUpdated?.Invoke(AlliesScore, EnemiesScore);
            OnRoundCompleted?.Invoke(false);
        }
    }
}
