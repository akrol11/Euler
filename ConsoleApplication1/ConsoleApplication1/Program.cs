
using System;
using System.IO;
using System.Text;
using JonSkeet.Ebcdic;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine
                    ("Usage: ConvertFile <ebcdic file (input)> <ascii file (output)>");
                return;
            }

            string inputFile = args[0];
            string outputFile = args[1];

            Encoding inputEncoding = EbcdicEncoding.GetEncoding("EBCDIC-US");
            Encoding outputEncoding = Encoding.ASCII;

            try
            {
                // Create the reader and writer with appropriate encodings.
                using (StreamReader inputReader =
                          new StreamReader(inputFile, inputEncoding))
                {
                    using (StreamWriter outputWriter =
                               new StreamWriter(outputFile, false, outputEncoding))
                    {
                        // Create an 8K-char buffer
                        char[] buffer = new char[8192];
                        int len = 0;

                        // Repeatedly read into the buffer and then write it out
                        // until the reader has been exhausted.
                        while ((len = inputReader.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            outputWriter.Write(buffer, 0, len);
                        }
                    }
                }
            }
            // Not much in the way of error handling here - you may well want
            // to do better handling yourself!
            catch (IOException e)
            {
                Console.WriteLine("Exception during processing: {0}", e.Message);
            }
    
        }
    }
}
