using System;

namespace MoonClimber.Maths
{
    public class Vector
    {
        private double _amount;
        private double _angle;
        private double _x;
        private double _y;

        public double Amount
        {
            get => _amount;
            set
            {
                _amount = value;
                _x = Amount * Math.Cos(Angle);
                _y = Amount * Math.Sin(Angle);
            }
        }

        public double Angle
        {
            get => _angle;
            set
            {
                _angle = value;
                _x = Amount * Math.Cos(Angle);
                _y = Amount * Math.Sin(Angle);
            }
        }

        public double X
        {
            get => _x;
            set
            {
                _x = value;
                _amount = Math.Sqrt(X * X + Y * Y);
                _angle = Math.Atan2(Y, X);
            }
        }

        public double Y
        {
            get => _y;
            set
            {
                _y = value;
                _amount = Math.Sqrt(X * X + Y * Y);
                _angle = Math.Atan2(Y, X);
            }
        }



        public static Vector FromPolar(double amount, double angle)
        {
            return new Vector
            {
                Amount = amount,
                Angle = angle
            };
        }

        public static Vector FromCartesian(double x, double y)
        {
            return new Vector
            {
                X = x,
                Y = y
            };
        }

        public static Vector operator +(Vector left, Vector right)
        {
            var vector =   Vector.FromCartesian(right.X + left.X, right.Y + left.Y);

            return vector;
        }

        public static Vector operator /(Vector vector, double divisor)
        {
            return Vector.FromPolar(vector.Amount/divisor, vector.Angle);
        }

        public static Vector operator *(Vector vector, double multiplicator)
        {
            return Vector.FromPolar(vector.Amount * multiplicator, vector.Angle);
        }
    }
}
