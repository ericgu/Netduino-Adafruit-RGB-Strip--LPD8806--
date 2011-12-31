using System;
using Microsoft.SPOT;

namespace MyApplication
{
    public class Stopwatch
    {
        DateTime _startTime;
        TimeSpan _deltaTime;

        public long Ticks
        {
            get { return _deltaTime.Ticks; }
        }

        public TimeSpan DeltaTime
        {
            get { return _deltaTime; }
        }

        public void Start()
        {
            _startTime = DateTime.Now;
        }

        static TimeSpan _overhead = new TimeSpan(-1493);

        public void Stop()
        {
            TimeSpan delta = DateTime.Now - _startTime;
            _deltaTime = _deltaTime.Add(delta);
            _deltaTime = _deltaTime.Add(_overhead);
        }

        public void StopAndPrint(string label)
        {
            Stop();
            Debug.Print(label + " " + ToString());
        }

        public void Reset()
        {
            _deltaTime = new TimeSpan(0);
        }

        public override string ToString()
        {
            return _deltaTime.ToString() + " (" + _deltaTime.Ticks.ToString() + ")";
        }
    }
}
