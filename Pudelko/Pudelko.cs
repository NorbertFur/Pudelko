using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PudelkoLibrary 
{
    public enum UnitOfMeasure { milimeter , centimeter, meter }

    public sealed class Pudelko : IEquatable<Pudelko>, IFormattable, IEnumerable<double>
    {
        private static readonly Dictionary<UnitOfMeasure, double> units = new Dictionary<UnitOfMeasure, double>
        {
            {UnitOfMeasure.centimeter, 10 },
            {UnitOfMeasure.milimeter, 100 },
            {UnitOfMeasure.meter, 0.1 },
        };

        private readonly double a;
        private readonly double b;
        private readonly double c;
        private readonly UnitOfMeasure unit;

        public double A => ToMeters(a, unit);

        public double B => ToMeters(b, unit);

        public double C => ToMeters(c, unit);

        public Pudelko(double a, double b, double c, UnitOfMeasure unit = UnitOfMeasure.meter)
        {
            if(!IsCorrectSize(a, b, c, unit))
            {
                throw new ArgumentOutOfRangeException();
            }

            this.a = a;
            this.b = b;
            this.c = c;
            this.unit = unit;
        }

        public Pudelko(double a, double b, UnitOfMeasure unit = UnitOfMeasure.meter) : this(a, b, units[unit], unit)
        {

        }

        public Pudelko(double a, UnitOfMeasure unit = UnitOfMeasure.meter) : this(a, units[unit], unit)
        {

        }

        public Pudelko(UnitOfMeasure unit = UnitOfMeasure.meter) : this(units[unit], unit)
        {

        }

        private bool IsCorrectSize(double a, double b, double c, UnitOfMeasure unit)
        {
            if(a <= 0 || b <= 0 || c <= 0)
            {
                return false;
            }

            bool negative;
            int maxValue = 10;
            switch (unit)
            {
                case UnitOfMeasure.milimeter:
                    negative = Math.Round(a) <= 0 || Math.Round(b) <= 0 || Math.Round(c) <= 0;
                    if (negative)
                    {
                        return false;
                    }
                    maxValue *= 1000;
                    break;
                case UnitOfMeasure.centimeter:
                    negative = Math.Round(a, 1) <=0 || Math.Round(b, 1) <= 0 || Math.Round(c, 1) <= 0;
                    if(negative)
                    {
                        return false;
                    }
                    maxValue *= 100;
                    return Math.Round(a, 1) <= maxValue && Math.Round(b, 1) <= maxValue && Math.Round(c, 1) <= maxValue;
                case UnitOfMeasure.meter:
                    maxValue *= 1;
                    break;
            }
            return a <= maxValue && b <= maxValue && c <= maxValue;
        }

        public bool Equals(Pudelko other)
        {
            if(this == other)
            {
                return true;
            }
            return A == other.A && B == other.B && C == other.C;
        }

        public string ToString(string format, IFormatProvider formatProvider = null)
        {
            if(format == "m" || format == null)
            {
                return ToString();
            }
            else if(format == "cm")
            {
                return $"{ToCentimeters(a, unit).ToString("0.0")} cm × {ToCentimeters(b, unit).ToString("0.0")} cm × {ToCentimeters(c, unit).ToString("0.0")} cm";
            }
            else if(format == "mm")
            {
                return $"{ToMilimiters(a, unit).ToString("0")} mm × {ToMilimiters(b, unit).ToString("0")} mm × {ToMilimiters(c, unit).ToString("0")} mm";
            }

            throw new FormatException();
        }

        public override string ToString()
        {
            return $"{A.ToString("0.000")} m × {B.ToString("0.000")} m × {C.ToString("0.000")} m";
        }

        public IEnumerator<double> GetEnumerator()
        {
            yield return a;
            yield return b;
            yield return c;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public static explicit operator double[](Pudelko p)
        {
            return new double[] { p.a, p.b, p.c };
        }

        public static implicit operator Pudelko((double, double, double) arg)
        {
            return new Pudelko(arg.Item1, arg.Item2, arg.Item3, UnitOfMeasure.milimeter);
        }

        public double this[int index]
        {
            get
            {
                switch(index)
                {
                    case 0:
                        return a;
                    case 1:
                        return b;
                    case 2:
                        return c;
                    default:
                        return 0;
                }
            }
        }

        private static double ToMeters(double value, UnitOfMeasure unit)
        {
            switch (unit)
            {
                case UnitOfMeasure.milimeter:
                    return value / 1000;
                case UnitOfMeasure.centimeter:
                    return value / 100;
                case UnitOfMeasure.meter:
                    return value;
            }
            return 0;
        }

        private static double ToCentimeters(double value, UnitOfMeasure unit)
        {
            switch (unit)
            {
                case UnitOfMeasure.milimeter:
                    return Math.Round(value / 10, 1);
                case UnitOfMeasure.centimeter:
                    return Math.Round(value, 1);
                case UnitOfMeasure.meter:
                    return Math.Round(value * 100, 1);
            }
            return 0;
        }

        private static double ToMilimiters(double value, UnitOfMeasure unit)
        {
            switch (unit)
            {
                case UnitOfMeasure.milimeter:
                    return Math.Round(value);
                case UnitOfMeasure.centimeter:
                    return Math.Round(value*10);
                case UnitOfMeasure.meter:
                    return Math.Round(value * 1000);
            }
            return 0;
        }
    }
}
