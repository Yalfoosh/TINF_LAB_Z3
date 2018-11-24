using System.Collections.Generic;
using TINF_Lab.StatisticsClasses;

namespace TINF_Lab.ShannonFanoClasses
{
    public class CodeEntry<T>
    {
        public readonly StochasticElement<T> Element;
        public readonly LargePhrase Code;

        #region Constructors

            /// <summary>
            /// Constructor for CodeEntry, deep copy.
            /// </summary>
            /// <param name="toCopy">The code entry instance which to deep copy into this.</param>
            public CodeEntry(CodeEntry<T> toCopy)
            {
                Element = new StochasticElement<T>(toCopy.Element);
                Code = new LargePhrase(toCopy.Code);
            }
    
            /// <summary>
            /// Constructor for CodeEntry.
            /// </summary>
            /// <param name="element">The stochastic element for the entry.</param>
            /// <param name="phrase">The code for the given stochastic element, can be null.</param>
            public CodeEntry(StochasticElement<T> element, LargePhrase phrase = null)
            {
                Element = new StochasticElement<T>(element);
                
                //Some of you might not know what this means, basically, it checks if phrase is null;
                //if it is, then it inserts new LargePhrase() into Code, if it isn't, then it inserts itself into Code.
                Code = phrase ?? new LargePhrase();
            }

        #endregion

        #region Analysis Methods

            /// <summary>
            /// Method that returns the influence of a stochastic element. In other words, it's the median length of a sign.
            /// </summary>
            /// <returns>The median length of a sign contained in this code entry's stochastic variable.</returns>
            public decimal Influence() => Code.Length * Element.Probability;

        #endregion

        #region Interface implementations and Overrides

            //Again, nothing special here.
        
            public override string ToString() => $"<{Element.Value}>    ->    {Code}";
        
            public override bool Equals(object obj) => obj is CodeEntry<T> entry && Equals(entry);
        
            private bool Equals(CodeEntry<T> other) =>
                EqualityComparer<T>.Default.Equals(Element.Value, other.Element.Value) && Equals(Code, other.Code);
    
            public override int GetHashCode()
            {
                unchecked
                {
                    return (EqualityComparer<T>.Default.GetHashCode(Element.Value) * 397) ^ (Code != null ? Code.GetHashCode() : 0);
                }
            }

        #endregion
    }
}