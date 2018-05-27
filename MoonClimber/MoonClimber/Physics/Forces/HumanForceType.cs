namespace MoonClimber.Physics.Forces
{
    /// <summary>
    /// Because human muscle takes time to contract and relax
    /// </summary>
    public class HumanForceType : ForceType
    {
        public override float IncreasingRatio => 0.2f;

        public override float DecreasingRatio => 0.2f;
    }
}
