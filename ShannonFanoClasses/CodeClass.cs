using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TINF_Lab.ShannonFanoClasses
{
    public class CodeClass<T> : IEnumerable<CodeEntry<T>>
    {
        private readonly HashSet<CodeEntry<T>> _elements;

        #region Constructors

            /// <summary>
            /// Constructor for CodeClass.
            /// </summary>
            /// <param name="group">The group which you can make the starting Code Class with.</param>
            public CodeClass(Group<T> group)
            {
                _elements = new HashSet<CodeEntry<T>>();

                foreach (var g in group)
                    _elements.Add(new CodeEntry<T>(g));
            }

            /// <summary>
            /// Constructor for CodeClass.
            /// </summary>
            /// <param name="codes">Any kind of CodeEntry collection you can fill the code entries with.</param>
            public CodeClass(IEnumerable<CodeEntry<T>> codes = null)
            {
                _elements = new HashSet<CodeEntry<T>>();

                if (codes == null) return;
                
                foreach (var c in codes)
                    Add(new CodeEntry<T>(c));
            }

        #endregion

        #region Analysis Methods
        
            /// <summary>
            /// Method that returns the entropy of the random variable contained in this code class.
            /// </summary>
            /// <returns>The entropy of the random variable contained in this code class.</returns>
            public decimal Entropy()
            {
                var toRet = 0M;
    
                //The formula used for entropy is sum(from i=1 to n){-log_2[p(x_i)] * p(x_i)}
                foreach (var x in _elements)
                    toRet += (decimal)(-Math.Log((double)x.Element.Probability, 2) * (double)x.Element.Probability);
    
                return toRet;
            }

            /// <summary>
            /// Method that returns the median length of the current code class instance.
            /// </summary>
            /// <returns>Median code length of the current code class instance.</returns>
            public decimal Length()
            {
                var toRet = 0M;

                foreach (var x in _elements)
                    toRet += x.Influence();

                return toRet;
            }

            /// <summary>
            /// Method that returns the ordered list of code entries for the current code class instance.
            /// </summary>
            /// <param name="ascending">True if ordered by ascending probabilities, false otherwise.</param>
            /// <returns>The ordered list of code entries contained in this code class instance.</returns>
            private List<CodeEntry<T>> GetOrdered(bool ascending = true) =>
                ascending
                    ? _elements.OrderBy(x => x.Code).ToList()
                    : _elements.OrderByDescending(x => x.Code).ToList();
        
            /// <summary>
            /// Static method that returns a coded code class for a certain group. Is recursive.
            /// </summary>
            /// <param name="group">The group which you want to code for.</param>
            /// <param name="currentClass">The instance of the code class so far. Is null if you just began coding.</param>
            /// <returns>The resulting coded CodeClass instance.</returns>
            public static CodeClass<T> GetCode(Group<T> group, CodeClass<T> currentClass = null)
            {
                //First, if we just started coding, we need to create a fresh new CodeClass using our group.
                if(currentClass == null)
                    currentClass = new CodeClass<T>(group);
                    
                //If our group count is larger than 1, it means the algorithm hasn't ended yet.
                if (group.Count() > 1)
                {
                    //First we need to split group into two groups roughly equal by the sum of probabilities.
                    var newGroups = group.SplitIntoTwo();
    
                    //We define 2 classes, zeroClass which will have 0s appended and oneClass, with 1s appended.
                    //We splice the class based on the split groups we got in the previous step.
                    var zeroClass = currentClass.GetSpliced(newGroups[0]);
                    var oneClass = currentClass.GetSpliced(newGroups[1]);
                        
                    //We add the code we need to add to them by appending values to them all.
                    zeroClass.AppendAll();
                    oneClass.AppendAll(true);
                        
                    //And then we continue coding, editing our existing zeroClass and oneClass and adding more code into it.
                    zeroClass = GetCode(newGroups[0], zeroClass);
                    oneClass = GetCode(newGroups[1], oneClass);
    
                    //When we come here, the tree has collapsed. Now we need to merge the two branches we had into one.
                    return Merge(zeroClass, oneClass);
                }
                    
                //If this passes, it means our input was 1 letter. We will append a 0 to it, but it's entropy is 0.
                if (currentClass.Length() == 0)
                    currentClass.AppendAll();
                    
                //Finally, whether it be the end of the tree or just a branch, we return the current code class instance.
                return currentClass;
            }

        #endregion

        #region Methods of change

            /// <summary>
            /// Method that adds a code entry to this code class instance.
            /// </summary>
            /// <param name="toAdd">Code entry to add.</param>
            private void Add(CodeEntry<T> toAdd) => _elements.Add(toAdd);

            /// <summary>
            /// Appends a value to all code entries in this code class instance.
            /// </summary>
            /// <param name="isOne">True if you want to append a 1, false for 0.</param>
            private void AppendAll(bool isOne = false)
            {
                foreach(var e in _elements)
                    e.Code.Append(isOne);
            }

            /// <summary>
            /// Method that spliced the current code class instance and makes a new one with only group members present.
            /// </summary>
            /// <param name="group">The only stochastic elements you want to have in your new code class.</param>
            /// <returns>A new code class containing only the stochastic elements found in group.</returns>
            private CodeClass<T> GetSpliced(Group<T> group) =>
                new CodeClass<T>(_elements.Where(x => group.Contains(x.Element.Value)));

            /// <summary>
            /// Method that merges two code class instances into one. Order is irrelevant.
            /// </summary>
            /// <param name="first">The first code class instance.</param>
            /// <param name="second">The second code class instance.</param>
            /// <returns>The new code class instance having members both from the first and the second.</returns>
            private static CodeClass<T> Merge(CodeClass<T> first, CodeClass<T> second)
            {
                //We assume the first is the main one, so we create a deep copy of it as toRet.
                var toRet = new CodeClass<T>(first);

                //We simply add every member from second into toRet.
                foreach (var x in second)
                    toRet.Add(x);

                return toRet;
            }

        #endregion

        #region Interface implementations and Overrides
        
            //There is absolutely nothing special here.

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    
            public IEnumerator<CodeEntry<T>> GetEnumerator() => _elements.GetEnumerator();

            public override bool Equals(object obj) => obj is CodeClass<T> other && Equals(other);

            private bool Equals(CodeClass<T> other)
            {
                foreach(var e in _elements)
                    if (!other.Contains(e))
                        return false;

                return true;
            }
    
            public override int GetHashCode() => (_elements != null ? _elements.GetHashCode() : 0);
    
            public override string ToString()
            {
                var sb = new StringBuilder();
    
                foreach (var x in GetOrdered())
                    sb.AppendLine(x.ToString());
    
                return sb.ToString();
            }

        #endregion
    }
}