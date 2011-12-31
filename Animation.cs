using System;
using Microsoft.SPOT;

namespace MyApplication
{
    public class Animation
    {
        /// <summary>
        /// The led that is being animated.
        /// </summary>
        public int LedNumber;

        /// <summary>
        /// The target red color value.
        /// </summary>
        public int RedTarget;

        /// <summary>
        /// The target green color value.
        /// </summary>
        public int GreenTarget;

        /// <summary>
        /// The target blue color value.
        /// </summary>
        public int BlueTarget;

        /// <summary>
        /// The number of steps in the animation.
        /// </summary>
        public int Steps;

        /// <summary>
        /// Number of cycles to delay before starting the animation.
        /// </summary>
        public int DelayCount;

        /// <summary>
        /// The current state of the animation.
        /// </summary>
        /// <remarks>
        /// If Current < 0, we are in the delay state.
        /// If Current > 0, we are animating towards the EndCount.
        /// </remarks>
        public int Current;

        /// <summary>
        /// The increment to add to red at each step.
        /// </summary>
        public float RedIncrement;

        /// <summary>
        /// The increment to add to green at each step.
        /// </summary>
        public float GreenIncrement;

        /// <summary>
        /// The increment to add to blue at each step.
        /// </summary>
        public float BlueIncrement;

        /// <summary>
        /// Create a new animation instance.
        /// </summary>
        /// <param name="ledNumber">The led to animate.</param>
        /// <param name="redTarget">The final red value.</param>
        /// <param name="greenTarget">The final green value.</param>
        /// <param name="blueTarget">The final blue value.</param>
        /// <param name="delayCount">The number of cycles to delay before animation starts.</param>
        /// <param name="steps">The number of steps.</param>
        public Animation(
            int ledNumber,
            int redTarget,
            int greenTarget,
            int blueTarget,
            int delayCount,
            int steps)
        {
            LedNumber = ledNumber;
            RedTarget = redTarget;
            GreenTarget = greenTarget;
            BlueTarget = blueTarget;
            Steps = steps;
            DelayCount = delayCount;
        }
    }
}
