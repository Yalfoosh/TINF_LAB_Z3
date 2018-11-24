    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using TINF_Lab.GeneralClasses;

namespace TINF_Lab.StatisticsClasses
    {
        public class RandomVariable<T> : IEnumerable<StochasticElement<T>>
        {
            private readonly HashSet<StochasticElement<T>> _elements;
    
            #region Constructors
    
                /// <summary>
                /// Constructor for RandomVariable, deep copy.
                /// </summary>
                /// <param name="other">The random variable which to create a deep copy from.</param>
                public RandomVariable(RandomVariable<T> other = null) =>
                    _elements = other == null ? new HashSet<StochasticElement<T>>() : other._elements;
        
                /// <summary>
                /// Constructor for RandomVariable
                /// </summary>
                /// <param name="set">A collection containing the stochastic elements you want to put into this RandomVariable.</param>
                public RandomVariable(IEnumerable<StochasticElement<T>> set) => _elements = new HashSet<StochasticElement<T>>(set);
    
            #endregion
    
            #region Methods of change
    
                /// <summary>
                /// Method that adds a stochastic element to this RandomVariable instance.
                /// </summary>
                /// <param name="toAdd">The stochastic element to add.</param>
                public void Add(StochasticElement<T> toAdd) => _elements.Add(new StochasticElement<T>(toAdd.Value, toAdd.Probability));
            
                /// <summary>
                /// Method that removes a stochastic element for this RandomVariable instance.
                /// </summary>
                /// <param name="toRemove">The stochastic variable to remove.</param>
                public void Remove(StochasticElement<T> toRemove) => _elements.Remove(toRemove);
            
                /// <summary>
                /// Method that removes the stochastic element with the highest probability.
                /// </summary>
                public void RemoveHighest()
                {
                    var toRem = GetOrdered();
            
                    if (toRem != null && toRem.Count > 0 && toRem[0] != null)
                        _elements.Remove(toRem[0]);
                    else
                        Console.Error.WriteLine("{0} {1}", Global.RV_REMOVE_NONEXISTENT_ELEMENT_EXCEPTION, GetHashCode());
                }

                /// <summary>
                /// Method that normalizes the RandomVariable to have the total probability sum of 1.
                /// </summary>
                public void Normalize()
                {
                    var tp = TotalProbability(this);
                    
                    if(tp != 1M)
                        foreach (var x in _elements)
                            x.Probability /= tp;
                }
    
            #endregion
    
            #region Analysis Methods
    
                /// <summary>
                /// Method that returns the number of stochastic elements in this RandomVariable instance.
                /// </summary>
                public int Count => _elements.Count;
    
                /// <summary>
                /// Method that returns the list of ordered stochastic elements.
                /// </summary>
                /// <param name="ascendingProbability">True if you want it ordered by ascending probability, false otherwise.</param>
                /// <returns>The list of stochastic elements ordered by probability.</returns>
                public List<StochasticElement<T>> GetOrdered(bool ascendingProbability = false)
                {
                    var toRet = new List<StochasticElement<T>>();
                    
                    foreach(var p in _elements)
                        toRet.Add(new StochasticElement<T>(p.Value, p.Probability));
        
                    return ascendingProbability
                        ? toRet.OrderBy(x => x.Probability).ThenBy(y => y.Value).ToList()
                        : toRet.OrderByDescending(x => x.Probability).ThenBy(y => y.Value).ToList();
                }
    
                /// <summary>
                /// Static method that returns the sum of probabilities from rv.
                /// </summary>
                /// <param name="rv">The random variable which to calculate the sum of probability from.</param>
                /// <returns></returns>
                public static decimal TotalProbability(RandomVariable<T> rv)
                {
                    var toRet = 0M;
                    
                    foreach (var x in rv)
                        toRet += x.Probability;
        
                    return toRet;
                }
    
                /// <summary>
                /// Static method that returns the probability difference between two random variables. Order is irrelevant.
                /// </summary>
                /// <param name="first">The first random variable.</param>
                /// <param name="second">The second random variable.</param>
                /// <returns>The decimal representation of the difference in first and second's probability sum.</returns>
                public static decimal ProbabilityDifference(RandomVariable<T> first, RandomVariable<T> second) =>
                    Math.Abs(TotalProbability(first) - TotalProbability(second));
    
            #endregion
    
            #region Interface implementation and Overrides
            
                //Again, nothing fancy here.
    
                public IEnumerator<StochasticElement<T>> GetEnumerator() => _elements.GetEnumerator();
                
                IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        
                public override string ToString()
                {
                    var sb = new StringBuilder();
        
                    foreach (var e in GetOrdered())
                        sb.AppendLine(e.ToString());
        
                    return sb.ToString();
                }
    
            #endregion
        }
    }