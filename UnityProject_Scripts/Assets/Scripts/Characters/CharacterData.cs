using UnityEngine;
using System.Collections.Generic;

namespace Nexus.Characters
{
    [System.Serializable]
    public class SkinDefinition
    {
        public string SkinId;
        public string SkinName;
        public Material ArmorMaterial;
        public Color VisorColor;
        public Color GlowColor;
        public Sprite PreviewIcon;
    }

    [CreateAssetMenu(fileName = "NewCharacter", menuName = "Nexus/Character Data")]
    public class CharacterData : ScriptableObject
    {
        [Header("Identidad (Imagen #3)")]
        public string CharacterName = "Vulcan";
        public string CharacterTitle = "Soldado Humano";
        public string Motto = "Disciplina y precisión.";
        [TextArea] public string Lore = "Especialista táctico de asalto equilibrado.";

        [Header("Estadísticas Base")]
        public float BaseHealth = 100f;
        public float BaseShield = 100f;
        public float MovementSpeedMultiplier = 1.0f;

        [Header("Habilidad Táctica")]
        public string AbilityName = "Barrera Energética";
        public float AbilityCooldown = 12.0f;
        public GameObject AbilityPrefab;

        [Header("Colección de Skins (Imagen #3)")]
        public List<SkinDefinition> AvailableSkins = new List<SkinDefinition>();
    }
}
