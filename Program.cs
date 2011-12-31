using System;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;

namespace MyApplication
{
    public class Program
    {
        const int NumPixels = 32;

        RGBStrip _rgbStrip;
        Animator _animator;
        Random _random = new Random();

        public static void Main()
        {
            Program program = new Program();
            program.DoAnimations();
        }

        public void DoAnimations()
        {
            _rgbStrip = new RGBStrip(32);
            _rgbStrip.DimFactor = 2;

            _animator = new Animator(_rgbStrip);

            while (true)
            {
                MovingFade();

                Ping();
                Blend();
                Cylon();
                RandomColors();
                AllColors();
            }
        }

        public void Blend()
        {
            _rgbStrip.ClearAllRaw();

            for (int j = 0; j < 6; j++)
            {
                // a progressive yellow/red blend
                for (byte i = 0; i < NumPixels; ++i)
                {
                    _rgbStrip.SetLedColorsRawWithDim(i, 64, (byte)(64 - i * 2), 0);
                    _rgbStrip.WriteToStripRaw();
                    Thread.Sleep(1000 / 32); // march at 32 pixels per second
                }

                for (byte i = 0; i < NumPixels; ++i)
                {
                    _rgbStrip.SetLedColorsRawWithDim(i, 0, (byte)(64 - i * 2), 64);
                    _rgbStrip.WriteToStripRaw();
                    Thread.Sleep(1000 / 32); // march at 32 pixels per second
                }

                for (byte i = 0; i < NumPixels; ++i)
                {
                    _rgbStrip.SetLedColorsRawWithDim(i, (byte)(64 - i * 2), 0, 64);
                    _rgbStrip.WriteToStripRaw();
                    Thread.Sleep(1000 / 32); // march at 32 pixels per second
                }

            }
        }

        public static Animation GetItem(int index, int colorOffset, int target, int cycleCount)
        {
            int redTarget = 0;
            int greenTarget = 0;
            int blueTarget = 0;

            switch ((index + colorOffset) % 7)
            {
                case 0:
                    redTarget = target;
                    break;
                case 1:
                    greenTarget = target;
                    break;
                case 2:
                    blueTarget = target;
                    break;
                case 3:
                    redTarget = target;
                    greenTarget = target;
                    break;
                case 4:
                    redTarget = target;
                    blueTarget = target;
                    break;
                case 5:
                    greenTarget = target;
                    blueTarget = target;
                    break;
                case 6:
                    redTarget = target;
                    greenTarget = target;
                    blueTarget = target;
                    break;
            }

            return new Animation(index, redTarget, greenTarget, blueTarget, 0, cycleCount);
        }

        public void RandomColors()
        {

            _rgbStrip.ClearAllRaw();

            for (int count = 0; count < 1000; count++)
            {
                _rgbStrip.SetLedColorsRawWithDim(
                    _random.GetInt(32),
                    _random.GetByte(127),
                    _random.GetByte(127),
                    _random.GetByte(127));
                _rgbStrip.WriteToStripRaw();

                System.Threading.Thread.Sleep(25);
            }
        }

        public void AllColors()
        {
            int stepCount = 25;
            int sleepMs = 0;

            _rgbStrip.ClearAll();

            for (int j = 0; j < 10; j++)
            {
                for (int i = 0; i < NumPixels; i++)
                {
                    Animation item = GetItem(i, j, 127, stepCount);

                    _animator.AddAnimation(item);
                }
                _animator.DoAnimation(sleepMs);

                for (int i = 0; i < NumPixels; i++)
                {
                    Animation item = GetItem(i, j, 0, stepCount);

                    _animator.AddAnimation(item);
                }
                _animator.DoAnimation(sleepMs);
            }
        }

        public void Cylon()
        {
            int segment = 0;
            int increment = 1;
            const int bounceCountMax = 30;

            _rgbStrip.ClearAll();

            int bounceCount = 0;

            while (bounceCount < 30)
            {
                Animation item = GetItem(bounceCount, 0, 127, 0);

                _rgbStrip.SetLedColorsRawWithDim(segment, 0, 0, 0);

                segment = segment + increment;
                _rgbStrip.SetLedColorsRawWithDim(segment, (byte)item.RedTarget, (byte)item.GreenTarget, (byte)item.BlueTarget);
                _rgbStrip.WriteToStripRaw();

                if (segment == 31 || segment == 0)
                {
                    increment = -increment;
                    bounceCount++;
                }

                System.Threading.Thread.Sleep(bounceCountMax - bounceCount);
            }
        }

        static byte[] DimensionSizes = { 1, 2, 3, 4, 5, 6, 8, 10, 12 };

        int GetRandomDimSize()
        {
            int index = _random.GetInt(9);

            return DimensionSizes[index];
        }

        public void Ping()
        {
            _rgbStrip.ClearAll();

            for (int count = 0; count < 1000; count++)
            {
                if (count % 3 == 0)
                {
                    int pixelIndex = -1;

                    for (int i = 0; i < 10; i++)
                    {
                        int testIndex = _random.GetInt(NumPixels);
                        if (!_animator.IsPixelAnimating(testIndex))
                        {
                            pixelIndex = testIndex;
                            break;
                        }
                    }

                    if (pixelIndex != -1)
                    {
                        int dimSize = GetRandomDimSize();
                        int cycleCount = 120 / dimSize;

                        Animation item = GetItem(pixelIndex, _random.GetInt(7), 120, cycleCount);
                        Animation itemDown = 
                            new Animation(pixelIndex, 0, 0, 0, cycleCount, cycleCount);

                        _animator.AddAnimation(item);
                        _animator.AddAnimation(itemDown);
                    }
                }

                _animator.DoAnimationStep();
            }
        }

        Animation GetRandomColorItem(int pixelIndex, int startCount, int endCount)
        {
            Animation item =
                new Animation(
                    pixelIndex,
                    _random.GetInt(127),
                    _random.GetInt(127),
                    _random.GetInt(127),
                    startCount,
                    endCount);

            return item;
        }


        void TestNew()
        {
            for (int pixel = 0; pixel < 32; pixel++)
            {
                _animator.AddAnimation(GetRandomColorItem(pixel, 0, 50));
            }

            _animator.DoAnimation(20);
        }

        const int PhaseSize = 60;

        void WriteFade(int led, int offset)
        {
            // fade red->green->blue->red etc.

            // Full cycle is 60 long

            // 0-59 convert to 0-3


            int value = (led + offset) % (PhaseSize * 3);
            int phase = value / PhaseSize;
            float factorInPhase = (value - (phase * PhaseSize)) / (float) PhaseSize;

            int red = 0;
            int green = 0;
            int blue = 0;

            switch (phase)
            {
                case 0:
                    red = (int)(127 * (1 - factorInPhase));
                    green = (int)(127 * factorInPhase);
                    blue = 0;
                    break;

                case 1:
                    green = (int)(127 * (1 - factorInPhase));
                    blue = (int)(127 * factorInPhase);
                    red = 0;
                    break;

                case 2:
                    blue = (int)(127 * (1 - factorInPhase));
                    red = (int)(127 * factorInPhase);
                    green = 0;
                    break;
            }

            _rgbStrip.SetLedColorsRawWithDim(led, red, green, blue);
        }


        void MovingFade()
        {
            for (int offset = 0; offset < 900 ; offset++)
            {
                for (int led = 0; led < 32; led++)
                {
                    WriteFade(led, offset);
                }
                _rgbStrip.WriteToStripRaw();
            }
        }
    }
}
