using System;
using System.Runtime.InteropServices;
using Raylib_cs;

namespace NexusTacticalWarfare.Game.Audio
{
    public static class SoundSynthesizer
    {
        private static bool _initialized = false;
        private static Sound _sndRifle;
        private static Sound _sndSniper;
        private static Sound _sndShotgun;
        private static Sound _sndSmg;
        private static Sound _sndPistol;
        private static Sound _sndReload;
        private static Sound _sndHit;
        private static Sound _sndHeadshot;
        private static Sound _sndShield;
        private static Sound _sndExplosion;
        private static Sound _sndClick;

        public static void Initialize()
        {
            if (_initialized) return;

            try
            {
                Raylib.InitAudioDevice();
                if (!Raylib.IsAudioDeviceReady()) return;

                _sndRifle = CreateSynthSound(0.12f, 440f, 160f, 0.7f, NoiseType.SquareNoise);
                _sndSniper = CreateSynthSound(0.35f, 980f, 70f, 0.95f, NoiseType.LowBoom);
                _sndShotgun = CreateSynthSound(0.25f, 220f, 50f, 0.9f, NoiseType.WhiteNoise);
                _sndSmg = CreateSynthSound(0.08f, 580f, 240f, 0.6f, NoiseType.Saw);
                _sndPistol = CreateSynthSound(0.10f, 650f, 280f, 0.65f, NoiseType.Square);
                _sndReload = CreateSynthSound(0.20f, 300f, 600f, 0.4f, NoiseType.ClickDouble);
                _sndHit = CreateSynthSound(0.06f, 880f, 880f, 0.5f, NoiseType.Sine);
                _sndHeadshot = CreateSynthSound(0.15f, 1320f, 1760f, 0.75f, NoiseType.SineChime);
                _sndShield = CreateSynthSound(0.40f, 200f, 520f, 0.6f, NoiseType.FuturisticHum);
                _sndExplosion = CreateSynthSound(0.45f, 120f, 30f, 0.85f, NoiseType.HeavyExplosion);
                _sndClick = CreateSynthSound(0.04f, 750f, 900f, 0.35f, NoiseType.Sine);

                _initialized = true;
            }
            catch
            {
                // Audio fallback gracefully silent if device unavailable
                _initialized = false;
            }
        }

        public static void PlayRifle() => SafePlay(_sndRifle);
        public static void PlaySniper() => SafePlay(_sndSniper);
        public static void PlayShotgun() => SafePlay(_sndShotgun);
        public static void PlaySmg() => SafePlay(_sndSmg);
        public static void PlayPistol() => SafePlay(_sndPistol);
        public static void PlayReload() => SafePlay(_sndReload);
        public static void PlayHit() => SafePlay(_sndHit);
        public static void PlayHeadshot() => SafePlay(_sndHeadshot);
        public static void PlayShield() => SafePlay(_sndShield);
        public static void PlayExplosion() => SafePlay(_sndExplosion);
        public static void PlayClick() => SafePlay(_sndClick);

        private static void SafePlay(Sound sound)
        {
            if (_initialized && Raylib.IsAudioDeviceReady())
            {
                Raylib.PlaySound(sound);
            }
        }

        public static void Shutdown()
        {
            if (!_initialized) return;
            try
            {
                Raylib.UnloadSound(_sndRifle);
                Raylib.UnloadSound(_sndSniper);
                Raylib.UnloadSound(_sndShotgun);
                Raylib.UnloadSound(_sndSmg);
                Raylib.UnloadSound(_sndPistol);
                Raylib.UnloadSound(_sndReload);
                Raylib.UnloadSound(_sndHit);
                Raylib.UnloadSound(_sndHeadshot);
                Raylib.UnloadSound(_sndShield);
                Raylib.UnloadSound(_sndExplosion);
                Raylib.UnloadSound(_sndClick);
                Raylib.CloseAudioDevice();
            }
            catch { }
            _initialized = false;
        }

        private enum NoiseType
        {
            Sine,
            Square,
            Saw,
            SquareNoise,
            WhiteNoise,
            LowBoom,
            ClickDouble,
            SineChime,
            FuturisticHum,
            HeavyExplosion
        }

        private static unsafe Sound CreateSynthSound(float duration, float startFreq, float endFreq, float volume, NoiseType type)
        {
            int sampleRate = 22050;
            int totalSamples = (int)(sampleRate * duration);
            if (totalSamples <= 0) totalSamples = 100;

            short* buffer = (short*)NativeMemory.Alloc((nuint)(totalSamples * sizeof(short)));
            Random rnd = new Random(1337);

            for (int i = 0; i < totalSamples; i++)
            {
                float t = (float)i / totalSamples;
                float currentFreq = startFreq + (endFreq - startFreq) * t;
                float envelope = (1f - t); // Linear decay

                float sample = 0f;
                float phase = (float)(i * currentFreq * 2.0 * Math.PI / sampleRate);

                switch (type)
                {
                    case NoiseType.Sine:
                        sample = MathF.Sin(phase);
                        break;
                    case NoiseType.Square:
                        sample = MathF.Sin(phase) > 0 ? 1f : -1f;
                        break;
                    case NoiseType.Saw:
                        sample = (float)(2.0 * ((i * currentFreq / sampleRate) % 1.0) - 1.0);
                        break;
                    case NoiseType.SquareNoise:
                        float sq = MathF.Sin(phase) > 0 ? 0.7f : -0.7f;
                        float nz = (float)(rnd.NextDouble() * 2.0 - 1.0) * 0.3f;
                        sample = sq + nz;
                        break;
                    case NoiseType.WhiteNoise:
                        sample = (float)(rnd.NextDouble() * 2.0 - 1.0);
                        break;
                    case NoiseType.LowBoom:
                        sample = MathF.Sin(phase) * 0.7f + ((float)rnd.NextDouble() * 2f - 1f) * (1f - t * 0.7f) * 0.3f;
                        break;
                    case NoiseType.ClickDouble:
                        sample = (t < 0.2f || (t > 0.4f && t < 0.6f)) ? MathF.Sin(phase * 2f) : 0f;
                        break;
                    case NoiseType.SineChime:
                        sample = MathF.Sin(phase) * 0.7f + MathF.Sin(phase * 1.5f) * 0.3f;
                        break;
                    case NoiseType.FuturisticHum:
                        sample = MathF.Sin(phase) * 0.6f + MathF.Sin(phase * 2.01f) * 0.3f + ((float)rnd.NextDouble() * 0.1f);
                        break;
                    case NoiseType.HeavyExplosion:
                        sample = ((float)rnd.NextDouble() * 2f - 1f) * envelope + MathF.Sin(phase * 0.5f) * 0.4f;
                        break;
                }

                float finalValue = sample * envelope * volume;
                finalValue = Math.Clamp(finalValue, -1f, 1f);
                buffer[i] = (short)(finalValue * 32767f);
            }

            Wave wave = new Wave
            {
                FrameCount = (uint)totalSamples,
                SampleRate = (uint)sampleRate,
                SampleSize = 16,
                Channels = 1,
                Data = buffer
            };

            Sound sound = Raylib.LoadSoundFromWave(wave);
            Raylib.UnloadWave(wave); // Frees the temporary wave container
            return sound;
        }
    }
}
