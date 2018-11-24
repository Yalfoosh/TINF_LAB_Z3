using System;
using System.Collections.Generic;
using System.Text;
using TINF_Lab.GeneralClasses;

namespace TINF_Lab.StatisticsClasses
{
    //The reason for this class is because I don't want this program to fail for large codes.
    public class LargePhrase : IComparable<LargePhrase>
    {
        private readonly List<bool> _storage;

        #region Constructors

            /// <summary>
            /// Constructor for LargePhrase, deep copy.
            /// </summary>
            /// <param name="toCopy">The LargePhrase instance which to deep copy.</param>
            public LargePhrase(LargePhrase toCopy)
            {
                _storage = new List<bool>();
                
                foreach(var x in toCopy._storage)
                    _storage.Add(x);
            }
        
            /// <summary>
            /// Constructor for LargePhrase.
            /// </summary>
            /// <param name="bytes">The string representation of bytes the large phrase instance should have.</param>
            public LargePhrase(string bytes = null)
            {
                _storage = new List<bool>();

                if (bytes == null) return;
                
                //Even if the string contains digits other than 0, I'll assume they're equal to 1.
                foreach(var b in bytes)
                    _storage.Add(b != '0');
            }

        #endregion

        #region Analysis Methods

            /// <summary>
            /// Method that returns the length of the code in this phrase.
            /// </summary>
            public int Length => _storage.Count;

        #endregion

        #region Methods of change

            /// <summary>
            /// Method that appends a value to the current large phrase instance.
            /// </summary>
            /// <param name="isOne">True if you want to append 1, false for 0.</param>
            public void Append(bool isOne = false) => _storage.Add(isOne);

        #endregion

        #region Interface implementations and Overrides

            /// <summary>
            /// Static method that compares two LargePhrase instances.
            /// </summary>
            /// <param name="x">The first LargePhrase instance.</param>
            /// <param name="y">The seconds LargePhrase instance.</param>
            /// <returns>1 if x is greater than y, -1 if x is less than y, 0 if they're equal.</returns>
            /// <exception cref="ArgumentNullException"></exception>
            private static int Compare(LargePhrase x, LargePhrase y)
            {
                //Need deep copies of it so it doesn't break existing ones.
                var tx = new LargePhrase(x);
                var ty = new LargePhrase(y);
                
                //First check if we even have an initialized phrase. The tx?. first checks if tx is null,
                //then if _storage is. It's a neat little C# thing.
                if (tx?._storage != null && ty?._storage != null)
                {
                    //While zeroes are the prefix, we want to remove them from both members.
                    while(tx._storage.Count > 0 && !tx._storage[0])
                        tx._storage.RemoveAt(0);
                
                    while(ty._storage.Count > 0 && !ty._storage[0])
                        ty._storage.RemoveAt(0);
    
                    //Then we check the length of the members; whichever one is the longest is the biggest.
                    if (tx.Length > ty.Length)
                        return 1;
                    else if (tx._storage.Count < ty._storage.Count)
                        return -1;
                
                    //If they are of the same length, just compare them 1 by 1, returning 1 if the first one is bigger, -1 otherwise.
                    for (var i = 0; tx._storage.Count > i; ++i)
                    {
                        if (tx._storage[i] && !ty._storage[i])
                            return 1;
                        else if (!tx._storage[i] && ty._storage[i])
                            return -1;
                    }
    
                    //If you've come here, it means the members are equal, so return 0.
                    return 0;
                }
                
                throw new ArgumentNullException(Global.LP_LARGE_PHRASES_NULL_EXCEPTION);
            }

            /// <inheritdoc />
            /// <summary>
            /// Method that compares the current LargePhrase instance with other.
            /// </summary>
            /// <param name="other">The LargePhrase instance to compare the current instance with.</param>
            /// <returns>1 if this is greater than other, -1 if this is less than other, 0 if they're equal.</returns>
            public int CompareTo(LargePhrase other) => Compare(this, other);
    
            public override string ToString()
                {
                    var sb = new StringBuilder();
                    
                    //Since our List is a boolean List, when printing strings we want to append 0s and 1s.
                    foreach (var digit in _storage)
                        sb.Append(digit ? '1' : '0');
        
                    return sb.ToString();
                }

        #endregion
    }
}