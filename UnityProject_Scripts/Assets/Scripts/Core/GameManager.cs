using UnityEngine;
using System;

namespace Nexus.Core
{
    public enum GameState
    {
        CoverScreen,
        Lobby,
        LoadingMatch,
        InMatch,
        RoundEnd,
        PostMatchSummary
    }

    /// <summary>
    /// Coordinador central del ciclo de vida del juego en Unity.
    /// Administra estados, persistencia de perfil y coordinación entre sistemas.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("Estado de Juego")]
        [SerializeField] private GameState _currentState = GameState.CoverScreen;
        public GameState CurrentState => _currentState;

        [Header("Perfil de Jugador")]
        public string PlayerName = "Operador_Nexus";
        public int PlayerLevel = 42;
        public int Credits = 12500;
        public int Shards = 150;
        public int CompetitiveRating = 12450; // Rango Leyenda Esmeralda

        public event Action<GameState> OnGameStateChanged;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void SetGameState(GameState newState)
        {
            if (_currentState == newState) return;
            _currentState = newState;
            OnGameStateChanged?.Invoke(_currentState);
            Debug.Log($"[GameManager] Estado cambiado a: {newState}");
        }
    }
}
