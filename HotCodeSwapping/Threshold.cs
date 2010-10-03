using System;

namespace HotCodeSwapping
{
    public class Threshold
    {
        private readonly TimeSpan duration;
        private DateTime lastChanged = DateTime.MinValue;

        public Threshold(TimeSpan duration)
        {
            this.duration = duration;
        }

        public bool EventIsUnderThreshold()
        {
            if ((DateTime.UtcNow - lastChanged) < duration)
                return true;

            lastChanged = DateTime.UtcNow;
            return false;
        }
    }
}