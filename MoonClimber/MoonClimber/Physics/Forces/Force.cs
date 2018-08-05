using System;
using System.Threading.Tasks;
using MoonClimber.Maths;
using Odin.Services;

namespace MoonClimber.Physics.Forces
{
    /// <summary>
    /// Right now we assume that every force is applied to the center of the object
    /// A force apply to an object, but not always as an explosion
    /// A force can become stronger as a humain muscle contracts
    /// This is describe by Force type.
    /// The force duration is for forces that maintains
    /// </summary>
    public class Force : Vector
    {
        private Logger _logger;
        private string _name;
        private readonly int _duration;
        private readonly ForceType _type;
        private readonly double _targetAmount;
        public ForceState State { get; set; }

        public Force(double amount, double angle, ForceType type, int duration = 0, string name = "force lambda")
        {
            _name = name;
            _logger = GameServiceLocator.Instance.Get<Logger>();
            _duration = duration;
            _type = type;
            _targetAmount = amount;
            Angle = angle;
            State = ForceState.Initializing;
        }


        public void Update()
        {
            if (State == ForceState.Initializing)
            {
                Amount = Math.Min(_targetAmount, Amount + _targetAmount * _type.IncreasingRatio);

                if (_targetAmount == Amount)
                {
                    State = ForceState.Living;
                    if (_duration == -1)
                        return;

                    Task.Factory.StartNew(() =>
                    {
                        Task.Delay(_duration);
                        State = ForceState.Dying;
                    });
                }
            }
            else if (State == ForceState.Dying)
            {
                Amount = Math.Max(0, Amount - _targetAmount * _type.IncreasingRatio);
                if (Amount == 0)
                {
                    State = ForceState.Dead;
                    _logger.Log($"{_name} - DEAD !!!");
                }
            }
        }

        public override string ToString()
        {
            return $"({X}, {Y} - {Amount}-{Angle * 180 / Math.PI}°";
        }
    }
}
