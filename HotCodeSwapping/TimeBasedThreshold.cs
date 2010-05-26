using System;

namespace HotCodeSwapping
{
    public class TimeBasedThreshold
    {
        private readonly TimeSpan threshold;
        private DateTime lastChanged = DateTime.MinValue;

        public TimeBasedThreshold(TimeSpan threshold)
        {
            this.threshold = threshold;
        }

        public bool EventIsUnderThreshold()
        {
            if ((DateTime.UtcNow - lastChanged) < threshold)
                return true;

            lastChanged = DateTime.UtcNow;
            return false;
        }
    }
}