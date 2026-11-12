using System;
using System.Collections.Generic;
using System.Numerics;
using Raylib_cs;

namespace NexusTacticalWarfare.Game.World
{
    public class Particle3D
    {
        public Vector3 Position;
        public Vector3 Velocity;
        public Color Color;
        public float Size;
        public float Lifetime;
        public float MaxLifetime;
        public bool AffectedByGravity = true;

        public bool IsDead => Lifetime >= MaxLifetime;

        public void Update(float dt)
        {
            Lifetime += dt;
            if (AffectedByGravity) Velocity.Y -= 9.8f * dt;
            Position += Velocity * dt;
        }
    }

    public class BulletTracer
    {
        public Vector3 Start;
        public Vector3 End;
        public Color Color;
        public float Lifetime;
        public float MaxLifetime = 0.15f;

        public bool IsDead => Lifetime >= MaxLifetime;

        public void Update(float dt)
        {
            Lifetime += dt;
        }
    }

    public class ParticleManager
    {
        private readonly List<Particle3D> _particles = new();
        private readonly List<BulletTracer> _tracers = new();
        private readonly Random _rnd = new();

        public void SpawnBulletTracer(Vector3 from, Vector3 to, Color color)
        {
            _tracers.Add(new BulletTracer
            {
                Start = from,
                End = to,
                Color = color,
                Lifetime = 0f,
                MaxLifetime = 0.12f
            });
        }

        public void SpawnHitSparks(Vector3 hitPos, Vector3 hitNormal, Color sparkColor, int count = 8)
        {
            for (int i = 0; i < count; i++)
            {
                Vector3 vel = hitNormal * 3f + new Vector3(
                    (float)(_rnd.NextDouble() - 0.5) * 4f,
                    (float)(_rnd.NextDouble() * 3f),
                    (float)(_rnd.NextDouble() - 0.5) * 4f
                );

                _particles.Add(new Particle3D
                {
                    Position = hitPos,
                    Velocity = vel,
                    Color = sparkColor,
                    Size = 0.08f,
                    Lifetime = 0f,
                    MaxLifetime = 0.4f + (float)_rnd.NextDouble() * 0.3f,
                    AffectedByGravity = true
                });
            }
        }

        public void SpawnEnergyPulse(Vector3 center, Color pulseColor, int count = 24)
        {
            for (int i = 0; i < count; i++)
            {
                float angle = (float)(_rnd.NextDouble() * Math.PI * 2.0);
                float speed = 6.0f + (float)_rnd.NextDouble() * 4.0f;
                Vector3 vel = new Vector3(MathF.Cos(angle) * speed, (float)(_rnd.NextDouble() * 2f), MathF.Sin(angle) * speed);

                _particles.Add(new Particle3D
                {
                    Position = center,
                    Velocity = vel,
                    Color = pulseColor,
                    Size = 0.15f,
                    Lifetime = 0f,
                    MaxLifetime = 0.7f,
                    AffectedByGravity = false
                });
            }
        }

        public void Update(float dt)
        {
            for (int i = _particles.Count - 1; i >= 0; i--)
            {
                _particles[i].Update(dt);
                if (_particles[i].IsDead) _particles.RemoveAt(i);
            }

            for (int i = _tracers.Count - 1; i >= 0; i--)
            {
                _tracers[i].Update(dt);
                if (_tracers[i].IsDead) _tracers.RemoveAt(i);
            }
        }

        public void Draw()
        {
            // Dibujar trazadoras láser
            foreach (var tr in _tracers)
            {
                float alpha = 1.0f - (tr.Lifetime / tr.MaxLifetime);
                Color c = new Color(tr.Color.R, tr.Color.G, tr.Color.B, (byte)(255 * alpha));
                Raylib.DrawLine3D(tr.Start, tr.End, c);
            }

            // Dibujar partículas
            foreach (var p in _particles)
            {
                float alpha = 1.0f - (p.Lifetime / p.MaxLifetime);
                Color c = new Color(p.Color.R, p.Color.G, p.Color.B, (byte)(255 * alpha));
                Raylib.DrawCube(p.Position, p.Size, p.Size, p.Size, c);
            }
        }

        public void Clear()
        {
            _particles.Clear();
            _tracers.Clear();
        }
    }
}
