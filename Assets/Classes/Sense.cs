using Assets.Enums;

namespace Assets.Classes
{
    public class Sense
    {
        public SenseType SenseType { get; set; }
        public double Intensity { get; set; }

        public Sense(SenseType senseType, double intensity)
        {
            SenseType = senseType;
            Intensity = intensity;
        }
    }
}
