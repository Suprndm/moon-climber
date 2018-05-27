namespace MoonClimber.Physics.Forces
{
    /// <summary>
    /// Explosive forces apply really fast
    /// </summary>
    public class ExplosiveForceType:ForceType
    {
        public override float IncreasingRatio => 0.8f;

        public override float DecreasingRatio => 0.8f;
    }
}
