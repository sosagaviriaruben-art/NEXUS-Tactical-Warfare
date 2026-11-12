using UnityEngine;
using UnityEngine.UI;
using System;

namespace Nexus.UI
{
    public enum GameRankTier
    {
        Bronce,
        Plata,
        Oro,
        Platino,
        Diamante,
        Heroico,
        Leyenda // Color Esmeralda Exclusivo
    }

    [Serializable]
    public class RankEntry
    {
        public GameRankTier Tier;
        public string TierName;
        public Color BadgeColor;
        public int MinPoints;
    }

    public class LobbyController : MonoBehaviour
    {
        [Header("Navegación Lateral (Imagen #1)")]
        [SerializeField] private Button _btnPlay;
        [SerializeField] private Button _btnCharacters;
        [SerializeField] private Button _btnArsenal;
        [SerializeField] private Button _btnGallery;
        [SerializeField] private Button _btnStore;
        [SerializeField] private Button _btnEvents;

        [Header("Panel Derecho: Temporada y Rango (Imagen #1)")]
        [SerializeField] private Text _seasonText;
        [SerializeField] private Text _rankText;
        [SerializeField] private Image _rankBadge;
        [SerializeField] private Button _bigPlayButton; // Dorado/Amarillo

        [Header("Sub-Tarjetas Inferiores (Imagen #1)")]
        [SerializeField] private Button _cardLuckyRoyale;
        [SerializeField] private Button _cardWeaponsGallery;
        [SerializeField] private Button _cardSkinsStore;
        [SerializeField] private Button _cardCharacterCustom;

        [Header("Configuración de Rangos con Leyenda Esmeralda")]
        [SerializeField] private RankEntry[] _ranks = new RankEntry[]
        {
            new RankEntry { Tier = GameRankTier.Bronce, TierName = "BRONCE", BadgeColor = new Color(0.8f, 0.5f, 0.2f), MinPoints = 0 },
            new RankEntry { Tier = GameRankTier.Plata, TierName = "PLATA", BadgeColor = new Color(0.75f, 0.75f, 0.8f), MinPoints = 1000 },
            new RankEntry { Tier = GameRankTier.Oro, TierName = "ORO", BadgeColor = new Color(1.0f, 0.85f, 0.0f), MinPoints = 2500 }, // Dorado
            new RankEntry { Tier = GameRankTier.Platino, TierName = "PLATINO", BadgeColor = new Color(0.0f, 0.9f, 1.0f), MinPoints = 4000 },
            new RankEntry { Tier = GameRankTier.Diamante, TierName = "DIAMANTE", BadgeColor = new Color(0.6f, 0.15f, 1.0f), MinPoints = 6000 },
            new RankEntry { Tier = GameRankTier.Heroico, TierName = "HEROICO", BadgeColor = new Color(1.0f, 0.1f, 0.25f), MinPoints = 8500 },
            // ESMERALDA para LEYENDA (Reemplazando el dorado repetido de imagen 1):
            new RankEntry { Tier = GameRankTier.Leyenda, TierName = "LEYENDA", BadgeColor = new Color(0.0f, 0.92f, 0.47f), MinPoints = 12000 }
        };

        private void Start()
        {
            UpdateRankDisplay(GameRankTier.Leyenda);
        }

        public void UpdateRankDisplay(GameRankTier tier)
        {
            foreach (var r in _ranks)
            {
                if (r.Tier == tier)
                {
                    if (_rankText != null) _rankText.text = $"RANGO: {r.TierName}";
                    if (_rankBadge != null) _rankBadge.color = r.BadgeColor;
                    break;
                }
            }
        }
    }
}
