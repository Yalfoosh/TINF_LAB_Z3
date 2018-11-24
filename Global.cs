// ReSharper disable InconsistentNaming

namespace TINF_Lab
{
    public static class Global
    {
        #region Strings

            #region EntryPoint
    
                public const string EP_ENTER_INPUT_DESCRIPTION_STRING =
                    "For a start, describe the input to the coder in one of the following ways:";
        
                public const string EP_INPUT_DESCRIPTION_FORM_1 =
                    "Enter a string whose occurence percentage corresponds to the occurence percentage of the whole system. " +
                    "For an example, a string of \"aabbbcccccd\" (without the quotation marks) corresponds to a system where\n\n" +
                    "p(a) = 2/11\n" +
                    "p(b) = 3/11\n" +
                    "p(c) = 5/11\n" +
                    "p(d) = 1/11\n";
        
                public const string EP_INPUT_DESCRIPTION_FORM_2 =
                    "Alternatively, you can enter the description through a file this way:\n" +
                    "Every sign will be represented as <sign>, followed by a single space, followed by the probability, " +
                    "defined by either a real number (format X.Y... or X,Y...), or by a fraction (format X/Y).\n" +
                    "Every sign is also delimited by a single semicolon (;). If you wish to enter the semicolon as a sign, " +
                    "enter it as \"<\\;>\".\nAn example of such a notation would be the following serialization:\n\n" +
                    "<a> 0.5;\n" +
                    "<b> 0.125;\n" +
                    "<c> 1/10;\n" +
                    "<d> 0,275\n" +
                    "\n\nNote that the final stochastic variable doesn't need to be delimited.";
        
                public const string EP_INPUT_DESCRIPTION_FINAL =
                    "\n\nBefore you finalize your input description, you can bring this text back by typing \"-help\".\n" +
                    "To exit the program type \"-exit\"";
        
                public const string EP_GIVEN_DESCRIPTION_STRING = "The given description is as follows:";
                public const string EP_SHANNON_FANO_CODE_IS_STRING = "For the given description, the Shannon Fano code is:";
    
            #endregion

            #region InputDescription
    
                public const string ID_PARSE_INVALID_FORMAT_EXCEPTION =
                    "The input description is incorrectly formatted. Consult -help.";
        
                public const string ID_PARSE_SAMPLE_IS_EMPTY_EXCEPTION =
                    "The input sample cannot be empty. Consult -help.";
        
                public const string ID_PARSE_INVALID_FRACTION_EXCEPTION = "The fraction you've entered is invalid";
                public const string ID_PARSE_INVALID_NUMERATOR_EXPRESSION = "The fraction numerator is invalid";
                public const string ID_PARSE_INVALID_DENOMINATOR_EXPRESSION = "The fraction denominator is invalid";
        
                public const string ID_PARSE_INVALID_REAL_EXPRESSION = "The real number you've entered is invalid";
    
            #endregion

            #region ShannonFanoCoder
    
                public const string SFC_INPUT_DESC_NULL_EXCEPTION = "Input Description must be defined.";
                public const string SFC_FRAME_STRING = "----------------------------------------------------------------------";
    
            #endregion

            #region CodeClass
    
                public const string CC_MEDIAN_CODE_LENGTH_STRING = "Median code word length = ";
                public const string CC_CODE_EFFICIENCY_STRING = "Code efficiency: ";
    
            #endregion

            #region LargePhrase
    
                public const string LP_LARGE_PHRASES_NULL_EXCEPTION = "You can't compare uninitialized LargePhrases.";
    
            #endregion

            #region RandomValue
    
                public const string RV_REMOVE_NONEXISTING_ELEMENT_EXCEPTION =
                    "Trying to remove an unexisting element from random variable";
    
            #endregion
        
        #endregion
    }
}