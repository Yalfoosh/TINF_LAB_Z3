using System;
using System.Text;
using TINF_Lab.ShannonFanoClasses;

namespace TINF_Lab
{
    public class ShannonFanoCoder
    {
        public readonly InputDescription Description;
        private readonly CodeClass<char> _class;

        #region Constructors

            /// <summary>
            /// Constructor for the Shannon Fano Coder.
            /// </summary>
            /// <param name="description">The input description you're going to be coding for.</param>
            /// <exception cref="ArgumentNullException">Throws if the input description is null.</exception>
            public ShannonFanoCoder(InputDescription description)
            {
                if(description != null)
                    Description = description;
                else
                    throw new ArgumentNullException(Global.SFC_INPUT_DESC_NULL_EXCEPTION);
                
                //When creating the coder, this automatically fetches the code for the loaded input description.
                _class = CodeClass<char>.GetCode(new Group<char>(Description.Input));
            }

        #endregion
        
        #region Interface implementations and Overrides
        
            public override string ToString()
            {
                var sb = new StringBuilder();
                
                sb.AppendLine(Global.SFC_FRAME_STRING);
                sb.AppendLine(_class.ToString());
                sb.AppendLine($"{Global.CC_MEDIAN_CODE_LENGTH_STRING}{_class.Length():0.000}");
                sb.AppendLine($"{Global.CC_CODE_EFFICIENCY_STRING}{100 * _class.Entropy() / _class.Length():0.000}%");
                sb.AppendLine(Global.SFC_FRAME_STRING + "\n");
    
                return sb.ToString();
            }
        
        #endregion
    }
}