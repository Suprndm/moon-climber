namespace MoonClimber.Physics.Forces
{
    /// <summary>
    /// Describes the behavior of a Force
    /// IncreasingRatio set the speed at which a force reaches its on potential
    /// DecreasingRatio set the speed at which a force fades
    /// </summary>
    public abstract class ForceType
    {
        public abstract float IncreasingRatio { get;  }
        public abstract float DecreasingRatio { get;  }
    }
}
