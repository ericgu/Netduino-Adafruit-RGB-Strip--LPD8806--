using System;
using Microsoft.SPOT;
using System.Collections;
using System.Threading;

namespace MyApplication
{
    /// <summary>
    /// A class that will handle multiple animations at once. 
    /// </summary>
    public class Animator
    {
        RGBStrip _rgbStrip;
        ArrayList _animations = new ArrayList();

        /// <summary>
        /// Create an instance of the animator.
        /// </summary>
        /// <param name="rgbStrip">The strip to animate.</param>
        public Animator(RGBStrip rgbStrip)
        {
            _rgbStrip = rgbStrip;
        }

        /// <summary>
        /// Add an animation.
        /// </summary>
        /// <param name="animation">The animation.</param>
        public void AddAnimation(Animation animation)
        {
            animation.Current = -animation.DelayCount;
            _animations.Add(animation);
        }

        /// <summary>
        /// Execute the current animations until they are all done.
        /// </summary>
        /// <param name="_rgbStrip"></param>
        /// <param name="sleepTime">Time to sleep between each cycle.</param>
        public void DoAnimation(int sleepTime)
        {
            while (_animations.Count != 0)
            {
                DoAnimationStep();

                if (sleepTime != 0)
                {
                    Thread.Sleep(sleepTime);
                }
            }
        }

        /// <summary>
        /// Do a single step of the animation.
        /// </summary>
        public void DoAnimationStep()
        {
            //Stopwatch stopwatch = new Stopwatch();
            //Stopwatch stopwatch2 = new Stopwatch();
            //Stopwatch stopwatch3 = new Stopwatch();
            //Stopwatch stopwatch4 = new Stopwatch();
            //Stopwatch stopwatch5 = new Stopwatch();
            //Stopwatch stopwatch6 = new Stopwatch();

            //stopwatch.Start();
            //stopwatch.StopAndPrint("Empty");


            //stopwatch.Start();
            for (int i = _animations.Count - 1; i >= 0; i--)
            {
                //stopwatch2.Start();
                Animation animateItem = (Animation) _animations[i];
                //stopwatch2.StopAndPrint("index");

                if (animateItem.Current < 0)
                {
                    animateItem.Current++;
                    continue;
                }

                //stopwatch3.Start();
                _rgbStrip.DoAnimationStep(animateItem);
                //stopwatch3.StopAndPrint("SetAndIncrementPixel");

                //stopwatch4.Start();
                animateItem.Current++;
                //stopwatch4.StopAndPrint("Inc");

                //stopwatch5.Start();
                if (animateItem.Current >= animateItem.Steps)
                {
                    _animations.RemoveAt(i);
                }
                //stopwatch5.StopAndPrint("Remove");
            }
            //stopwatch.StopAndPrint("Increment: ");

            //stopwatch.Reset();
            //stopwatch.Start();
            _rgbStrip.WriteToStrip();
            //stopwatch.StopAndPrint("Write: ");
        }

        /// <summary>
        /// Return true if an led is currently animating.
        /// </summary>
        /// <param name="ledNumber">The led number.</param>
        /// <returns></returns>
        public bool IsPixelAnimating(int ledNumber)
        {
            foreach (Animation item in _animations)
            {
                if (item.LedNumber == ledNumber)
                {
                    return true;
                }
            }

            return false;
        }
    }
}

