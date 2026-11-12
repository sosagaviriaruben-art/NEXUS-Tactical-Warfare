using System;
using System.Numerics;
using Raylib_cs;

namespace NexusTacticalWarfare.Game.Entities
{
    public class EnergyShieldEntity
    {
        public Vector3 Position { get; set; }
        public Vector3 Forward { get; set; }
        public Vector3 Size { get; set; } = new Vector3(3.2f, 2.0f, 0.3f);
        public float MaxHealth { get; set; } = 300f;
        public float CurrentHealth { get; set; } = 300f;
        public float Duration { get; set; } = 8.0f;
        public bool IsActive => CurrentHealth > 0f && Duration > 0f;
        public Color ShieldColor { get; set; } = new Color(0, 240, 255, 140);

        public void Update(float dt)
        {
            if (Duration > 0f) Duration -= dt;
        }

        public bool CheckHit(Ray ray, out RayCollision collision)
        {
            BoundingBox box = new BoundingBox(
                Position - Size * 0.5f,
                Position + Size * 0.5f
            );
            collision = Raylib.GetRayCollisionBox(ray, box);
            return collision.Hit;
        }

        public void TakeDamage(float dmg)
        {
            CurrentHealth -= dmg;
        }

        public void Draw()
        {
            if (!IsActive) return;

            float pulse = 0.85f + 0.15f * MathF.Sin((float)Raylib.GetTime() * 8f);
            Color drawCol = new Color(ShieldColor.R, ShieldColor.G, ShieldColor.B, (byte)(ShieldColor.A * pulse));

            Raylib.DrawCube(Position, Size.X, Size.Y, Size.Z, drawCol);
            Raylib.DrawCubeWires(Position, Size.X, Size.Y, Size.Z, Color.White);
        }
    }
}
