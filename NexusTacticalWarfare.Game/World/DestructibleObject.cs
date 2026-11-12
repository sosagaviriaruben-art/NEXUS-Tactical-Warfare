using System;
using System.Collections.Generic;
using System.Numerics;
using Raylib_cs;

namespace NexusTacticalWarfare.Game.World
{
    public class DebrisChunk
    {
        public Vector3 Position;
        public Vector3 Velocity;
        public Vector3 Size;
        public Color Color;
        public float Lifetime;
        public float MaxLifetime = 2.5f;

        public bool IsDead => Lifetime >= MaxLifetime;

        public void Update(float dt)
        {
            Lifetime += dt;
            Velocity.Y -= 9.8f * dt; // Gravedad
            Position += Velocity * dt;
            if (Position.Y < Size.Y * 0.5f)
            {
                Position.Y = Size.Y * 0.5f;
                Velocity.X *= 0.6f;
                Velocity.Z *= 0.6f;
                Velocity.Y = -Velocity.Y * 0.3f; // Rebote
            }
        }
    }

    public class DestructibleObject
    {
        public Vector3 Position { get; set; }
        public Vector3 Size { get; set; }
        public float MaxHealth { get; set; } = 80f;
        public float CurrentHealth { get; set; } = 80f;
        public bool IsDestroyed => CurrentHealth <= 0f;
        public Color BaseColor { get; set; }
        public Color AccentColor { get; set; }
        public string ObjectName { get; set; } = "Cobertura Destructible";

        public DestructibleObject(Vector3 pos, Vector3 size, float health, Color baseCol, Color accentCol, string name)
        {
            Position = pos;
            Size = size;
            MaxHealth = health;
            CurrentHealth = health;
            BaseColor = baseCol;
            AccentColor = accentCol;
            ObjectName = name;
        }

        public bool TakeDamage(float damage, out List<DebrisChunk> spawnedDebris)
        {
            spawnedDebris = new List<DebrisChunk>();
            if (IsDestroyed) return false;

            CurrentHealth -= damage;
            if (IsDestroyed)
            {
                // Generar fragmentos físicos 3D al destruirse
                Random rnd = new Random();
                int chunkCount = 12;
                for (int i = 0; i < chunkCount; i++)
                {
                    Vector3 chunkPos = Position + new Vector3(
                        (float)(rnd.NextDouble() - 0.5) * Size.X * 0.8f,
                        (float)(rnd.NextDouble() - 0.5) * Size.Y * 0.8f,
                        (float)(rnd.NextDouble() - 0.5) * Size.Z * 0.8f
                    );

                    Vector3 chunkVel = new Vector3(
                        (float)(rnd.NextDouble() - 0.5) * 8.0f,
                        (float)(rnd.NextDouble() * 5.0f + 2.0f),
                        (float)(rnd.NextDouble() - 0.5) * 8.0f
                    );

                    Vector3 chunkSize = new Vector3(
                        Size.X / 3.5f * (0.6f + (float)rnd.NextDouble() * 0.4f),
                        Size.Y / 3.5f * (0.6f + (float)rnd.NextDouble() * 0.4f),
                        Size.Z / 3.5f * (0.6f + (float)rnd.NextDouble() * 0.4f)
                    );

                    Color c = (rnd.Next(2) == 0) ? BaseColor : AccentColor;
                    spawnedDebris.Add(new DebrisChunk
                    {
                        Position = chunkPos,
                        Velocity = chunkVel,
                        Size = chunkSize,
                        Color = c
                    });
                }
                return true; // Destruido en este impacto
            }
            return false;
        }

        public BoundingBox GetBoundingBox()
        {
            return new BoundingBox(
                Position - Size * 0.5f,
                Position + Size * 0.5f
            );
        }
    }
}
