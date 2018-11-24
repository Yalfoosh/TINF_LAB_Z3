using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TINF_Lab.StatisticsClasses;

namespace TINF_Lab.ShannonFanoClasses
{
    public class Group<T> : IEnumerable<StochasticElement<T>>
    {
        private readonly HashSet<StochasticElement<T>> _elements;

        #region Constructors

            /// <summary>
            /// Constructor for Group, deep copy.
            /// </summary>
            /// <param name="toCopy">The Group instance to deep copy.</param>
            public Group(Group<T> toCopy) : this(toCopy._elements) { }
        
            /// <summary>
            /// Constructor for Group.
            /// </summary>
            /// <param name="elements">The collection of stochastic elements to make up the Group.</param>
            private Group(IEnumerable<StochasticElement<T>> elements) => _elements = new HashSet<StochasticElement<T>>(elements);
        
            /// <summary>
            /// Constructor for Group.
            /// </summary>
            /// <param name="rv">The random variable which to translate into a Group.</param>
            public Group(RandomVariable<T> rv) => _elements = new HashSet<StochasticElement<T>>(rv);

        #endregion

        #region Analysis Methods

            /// <summary>
            /// Method that splits the Group into two groups of roughly the same probability sum.
            /// </summary>
            /// <returns>The list of Groups with 2 elements, each of which represents a 0-group and a 1-group.</returns>
            public List<Group<T>> SplitIntoTwo()
            {
                //First we assume that we will fill random variables until we reach a satisfactory state,
                //from the 1-group to the 0-group. The exact order is not important, it's an implementation detail.
                var rv1 = new RandomVariable<T>();
                var rv2 = new RandomVariable<T>(_elements);
    
                var toRet = new List<Group<T>>();
    
                //We assume that the maximum difference in probability is going to be 0 (Full random variable and empty one).
                for (var lastDiff = 1M;;)
                {
                    //So we fetch a stochastic element with the highest probability and put it into toAdd.
                    var toAdd = rv2.GetOrdered()[0];
                    
                    //We add that stochastic element into our 0-group.
                    rv1.Add(toAdd);
                    //And remove it from the 1-group.
                    rv2.RemoveHighest();
                    
                    //We then calculate the current probability difference between these two.
                    var currentDiff = RandomVariable<T>.ProbabilityDifference(rv1, rv2);
    
                    //If the difference is smaller than the last iteration, we should update the toRet variable with it.
                    if (currentDiff < lastDiff)
                    {
                        //And set the last difference to current difference.
                        lastDiff = currentDiff;
    
                        toRet = new List<Group<T>>
                        {
                            new Group<T>(rv1),
                            new Group<T>(rv2)
                        };
                    }
                    //If we've come to the point where our current difference is greater or equal to the last
                    //that means that we're at the boundary for the group, so we should revert the changes and stop.
                    else if (currentDiff >= lastDiff)
                    {
                        var toRemove = rv1.GetOrdered()[rv1.Count - 1];
                        rv2.Add(toRemove);
                        rv1.Remove(toRemove);
                        break;
                    }
                }
                
                //Finally, we should return the last best group list.
                return toRet;
            }

        #endregion

        #region Interface implementations and Overrides
        
            //Nothing here is of great importance.

            public IEnumerator<StochasticElement<T>> GetEnumerator() => _elements.GetEnumerator();
            
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public bool Contains(T element) => _elements.Count(x => x.Value.Equals(element)) > 0;

            public override bool Equals(object obj) => obj is Group<T> group && Equals(group);

            private bool Equals(Group<T> other)
            {
                foreach(var x in other)
                    if (!_elements.Contains(x))
                        return false;
    
                return true;
            }

            public override int GetHashCode() => (_elements != null ? _elements.GetHashCode() : 0);
    
            public override string ToString()
            {
                var sb = new StringBuilder();
        
                foreach (var e in _elements)
                    sb.AppendLine(e.ToString());
        
                return sb.ToString();
            }

        #endregion
    }
}