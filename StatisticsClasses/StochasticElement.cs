using System.Collections.Generic;

namespace TINF_Lab.StatisticsClasses
{
    public class StochasticElement<T>
    {
        public T Value { get; }
        public decimal Probability { get; }

        #region Constructors

            /// <summary>
            /// Constructor for StochasticElement, deep copy.
            /// </summary>
            /// <param name="toCopy">The stochastic element instance which to deep copy from.</param>
            public StochasticElement(StochasticElement<T> toCopy) : this(toCopy.Value, toCopy.Probability) { }
        
            /// <summary>
            /// Constructor for StochasticElement.
            /// </summary>
            /// <param name="value">The value of the stochastic element.</param>
            /// <param name="probability">The probability the stochastic element should have.</param>
            public StochasticElement(T value, decimal probability)
            {
                Value = value;
                Probability = probability;
            }

        #endregion

        #region Interface implementation and Overrides
        
            //Nothing special here.

            public override string ToString() => $"<{Value}>    {Probability:0.000}";
    
            public override bool Equals(object obj) => obj is StochasticElement<T> element && Equals(element);
        
            private bool Equals(StochasticElement<T> other) => EqualityComparer<T>.Default.Equals(Value, other.Value);
        
            public override int GetHashCode() => EqualityComparer<T>.Default.GetHashCode(Value);

        #endregion
    }
}