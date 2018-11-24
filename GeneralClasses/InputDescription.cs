using System;
using System.Collections.Generic;
using TINF_Lab.StatisticsClasses;

namespace TINF_Lab.GeneralClasses
{
    public class InputDescription
    {
        public RandomVariable<char> Input { get; }

        #region Constructors

            /// <summary>
            /// Constructor for InputDescription.
            /// </summary>
            /// <param name="sample">The sample string containing the input description.</param>
            public InputDescription(string sample)
            {
                Input = sample.Trim().StartsWith("<")
                    ? new RandomVariable<char>(ParseValues(sample))
                    : new RandomVariable<char>(ParseFromSample(sample));

                if (RandomVariable<char>.TotalProbability(Input) != 1M)
                    if (Global.NormalizeInput())
                        Input.Normalize();
                    else
                        throw new ArgumentException(Global.RV_INVALID_PROBABILITY_SUM_EXCEPTION);
            }

        #endregion

        #region Parse Methods

            /// <summary>
            /// Method that parses the input description from a textual sample of a message.
            /// </summary>
            /// <param name="sample">The string representation of a message.</param>
            /// <returns>A hashset of stochastic elements representing the input description of the message.</returns>
            /// <exception cref="ArgumentException">Thrown when sample is not in the correct format.</exception>
            private static HashSet<StochasticElement<char>> ParseFromSample(string sample)
            {
                if(sample.Length < 1)
                    throw new ArgumentException(Global.ID_PARSE_SAMPLE_IS_EMPTY_EXCEPTION);
                
                //Alternatively you could use a List with Tuples since this is inefficient for smaller sizes, but meh.
                var samples = new Dictionary<char, uint>();
                var iterations = 0;
    
                //In every iteration I increase the number of seen signs by 1 if they're in the dictionary,
                //otherwise I just add them and count 1.
                foreach (var s in sample)
                {
                    if (samples.ContainsKey(s))
                        samples[s] += 1;
                    else
                        samples.Add(s, 1);
    
                    ++iterations;
                }
                
                //Since I have them all counted now, I will simple create a stochastic element for each sign,
                //giving it the probability equal to number of occurrences divided by total occurrences.
                    
                var toRet = new HashSet<StochasticElement<char>>();
                    
                foreach(var s in samples)
                    toRet.Add(new StochasticElement<char>(s.Key, (decimal)s.Value/iterations));
    
                return toRet;
            }
    
            /// <summary>
            /// Method that parses the input description from a set of delimited stochastic elements.
            /// </summary>
            /// <param name="values">The stochastic elements delimited by a semicolon.</param>
            /// <returns>A hashset of stochastic elements representing the input description of a given system.</returns>
            /// <exception cref="ArgumentException">Thrown when the format of the description is invalid.</exception>
            private static HashSet<StochasticElement<char>> ParseValues(string values)
            {
                //Format is "<_sign_> _probability_;" where
                //_sign_ is the sign you're assigning a probability to
                //_probability_ is a real number depicting the probability of the sign appearing, can be written as a fraction
                //; is the delimiter
                //If you want ; to be the sign, write it as <\;>
                
                //I'm replacing our semicolon character with something that won't f up parsing.
                values = values.Replace(@"<\;>", "><");
                var tuples = values.Split(';');
                var toRet = new HashSet<StochasticElement<char>>();
                
                //tuples contain elements in which 1 is exactly 1 stochastic element.
                foreach (var t in tuples)
                {
                    //Returning our transformed semicolon to the normal form.
                    var tTemp = t.Trim().Replace("><", "<;>");
                    
                    //temp should contain "<sign" in its first index, and the probability in its second.
                    var temp = tTemp.Split(new[] {"> "}, 2, StringSplitOptions.RemoveEmptyEntries);
                    
                    //If the length is not exactly 2, the parsing has failed so we have to throw an exception.
                    if(temp.Length != 2)
                        throw new ArgumentException(Global.ID_PARSE_INVALID_FORMAT_EXCEPTION);

                    //If the probability contains a "/", then it's a fraction.
                    if (temp[1].Contains("/"))
                    {
                        //tNum[0] will contain the numerator and tNum[1] the denominator.
                        var tNum = temp[1].Split('/');
                        
                        //If the length of tNum is not 2, either the user f'd up the input, or it's a complex fraction.
                        //Either way, we can't handle those so we throw an exception.
                        if(tNum.Length != 2)
                            throw new ArgumentException(Global.ID_PARSE_INVALID_FRACTION_EXCEPTION + ": " + temp[1]);

                        //In the next few lines we're TryParse-ing our fraction and throwing if it's not valid.
                        
                        if(!decimal.TryParse(tNum[0], out var numerator))
                            throw new ArgumentException(Global.ID_PARSE_INVALID_NUMERATOR_EXPRESSION + "; " + tNum[0]);
                        
                        if(!decimal.TryParse(tNum[1], out var denominator))
                            throw new ArgumentException(Global.ID_PARSE_INVALID_DENOMINATOR_EXPRESSION + ": " + tNum[1]);

                        //Finally, we're adding them to our hashset as a decimal number.
                        toRet.Add(new StochasticElement<char>(temp[0][1], numerator / denominator));
                    }
                    //If our number isn't a fraction, then it must be a real number.
                    else
                    {
                        //We're TryParse-ing it while replacing "." with ",", since TryParse shits itself with dots.
                        if(!decimal.TryParse(temp[1].Replace(".", ","), out var parsed))
                            throw new ArgumentException(Global.ID_PARSE_INVALID_REAL_EXPRESSION + ": " + temp[1]);
    
                        toRet.Add(new StochasticElement<char>(temp[0][1], parsed));
                    }
                }
    
                return toRet;
            }

        #endregion

        #region Analysis Methods

            /// <summary>
            /// Method that gets the ordered stochastic elements, by default with descending probabilities.
            /// </summary>
            /// <param name="ascendingProbability">True if this should return ascending probabilities, false otherwise.</param>
            /// <returns>The list of stochastic elements ordered by probability.</returns>
            public List<StochasticElement<char>> GetOrdered(bool ascendingProbability = false) =>
                Input.GetOrdered(ascendingProbability);

        #endregion

        #region Interface implementation and Overrides

            public override string ToString() => Input.ToString();

        #endregion
    }
}