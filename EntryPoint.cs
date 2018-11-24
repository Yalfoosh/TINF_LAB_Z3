using System;
using System.Text;

namespace TINF_Lab
{
    internal static class EntryPoint
    {
        /// <summary>
        /// Method that prints out the help dialogue in console.
        /// </summary>
        private static void Help()
        {
            var sb = new StringBuilder();

            sb.AppendLine(Global.EP_ENTER_INPUT_DESCRIPTION_STRING + "\n");
            sb.AppendLine(Global.EP_INPUT_DESCRIPTION_FORM_1);
            sb.AppendLine(Global.EP_INPUT_DESCRIPTION_FORM_2);
            sb.AppendLine(Global.EP_INPUT_DESCRIPTION_FINAL);
            
            Console.Clear();
            Console.WriteLine(sb.ToString());
        }

        /// <summary>
        /// Method that exits the program with exit code 0.
        /// </summary>
        private static void Exit() => Environment.Exit(0);

        /// <summary>
        /// Method that listens to the user and does a command when it recognises it.
        /// </summary>
        /// <returns>The input description if one is entered.</returns>
        private static string AwaitCommand()
        {
            //Line will be the current read line, while buffer is the complete read content.
            string line = "";
            string buffer = "";

            while ((line = Console.ReadLine()) != null)
            {
                //If we read an empty line, the input is over. Have to check if buffer is large enough, though.
                if (line == "")
                    return buffer.Length > 1
                        ? buffer.Substring(0, buffer.Length - 1) 
                        : "";
                
                //Ordinarily, without the line break the lines would be concatenated. We don't want that.
                buffer += line + "\n";

                //Check if a specific console command has been called.
                if (buffer == "-help\n")
                {
                    Help();
                    buffer = "";
                }
                else if(buffer == "-exit\n")
                    Exit();
            }

            //Finally, since nothing failed or triggered, I guess we have an input description.
            return buffer;
        }
        
        public static void Main(string[] args)
        {
            Help();
            
            //Shiver at my magnificent goto!
            AwaitCommand:
            var read = AwaitCommand();

            try
            {
                var coder = new ShannonFanoCoder(new InputDescription(read));
                
                Console.WriteLine(Global.EP_GIVEN_DESCRIPTION_STRING + "\n");
                Console.WriteLine(coder.Description);
                Console.WriteLine(Global.EP_SHANNON_FANO_CODE_IS_STRING + "\n");
                Console.WriteLine(coder);
            }
            catch (ArgumentException ae)
            {
                Console.WriteLine(ae.Message);
            }
            
            goto AwaitCommand;
        }
    }
}