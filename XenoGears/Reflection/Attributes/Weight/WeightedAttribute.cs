using System;
using System.Diagnostics;
using XenoGears.Assertions;

namespace XenoGears.Reflection.Attributes.Weight
{
    [DebuggerNonUserCode]
    public abstract class WeightedAttribute : Attribute
    {
        private double _weight = 1.0;
        public double Weight
        {
            get { return _weight; }
            set
            {
                (value > 0).AssertTrue();
                _weight = value;
                _metric = 1 / value;
            }
        }

        private double _metric = 1.0;
        public double Metric
        {
            get { return _metric; }
            set
            {
                (value > 0).AssertTrue();
                _metric = value;
                _weight = 1 / value;
            }
        }
    }
}