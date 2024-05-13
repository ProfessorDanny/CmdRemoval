

// LRS Filter program for to remove Format Block ^FB Old CER FW does not support correctly block code for new Specimine Label 
// looking to remove the following code block from the label ^FB273,1,0,L  replace with " " Blank Space

//The filter definition in VPXS should look like this
//Datatype all
//Command - CmdRemoval.exe (this program)
//Arguments: &infile &outfile 

//  The following is how to post information to the LRS Printer Log
//  Console.WriteLine("<!VPSX-MsgType>INFO");
//  Console.WriteLine("This is what the Info message type will look like"); 

//  Console.WriteLine("<!VPSX-MsgType>WARN");
//  Console.WriteLine("This is what the Warning message type will look like");

//  Console.WriteLine("<!VPSX-MsgType>ERROR");
//  Console.WriteLine("This is what the Error message type will look like");

//  Console.WriteLine("<!VPSX-MsgType>DEBUG");
//  Console.WriteLine("This is what the Debug message type will look like");

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace CmdRemoval
{
    class Program
    {

        static int Main(string[] args)
        {
            //Declare any variables to use

            string InputFileName;
            string OutputFileName;

            // Old Cerner Certified FW does not support the following command correctly with Font type ^ACN,36,10
            // this is the first line of the new Label Design removal the ^FB273,1,0,L as seen in new designe will
            // not change the look of the label.
            // ^FB maxWidth, maxLines, lineSpacing, alignment, hangingIndent 

            // STRING TO REMOVE AND REPLACE *******************************

            string CMDtoRemove = "^FB273,1,0,L";
            string CMDtoReplace = " ";

            //String AttrFileName;
            string line;
            string TempFile;
 
            InputFileName = args[0];
            OutputFileName = args[1];
 
            Console.WriteLine("<!VPSX-MsgType>DEBUG");
            Console.WriteLine("Check for Cerner FW incorrect implementation of Cmd ^FB273,1,0,L");

            Console.WriteLine("<!VPSX-MsgType>INFO");
            Console.WriteLine("The Input Filename is {0}", InputFileName);

            Console.WriteLine("<!VPSX-MsgType>INFO");
            Console.WriteLine("The Output Filename is {0} ", OutputFileName);

            // Create a file to write to.

            using (StreamWriter sw = File.CreateText(OutputFileName))
            {
                Console.WriteLine("<!VPSX-MsgType>DEBUG");
                Console.WriteLine("Create Output File");
            }

            // Create a temp file to write to. LRS VPSX does not allow overwrite of input file must create a temp file to work with.
            TempFile = string.Format(@"D:\temp\{0}.TXT", Guid.NewGuid());

            File.Copy(InputFileName, TempFile);

            //VPSX expects all filters to create an altered output file. 

            // Read data from input file to start the process
              
            System.IO.StreamReader file = new System.IO.StreamReader(TempFile);

            Console.WriteLine("<!VPSX-MsgType>DEBUG");
            Console.WriteLine("Open File for Read");

            while ((line = file.ReadLine()) != null)
            {
                // Look for the ZPL CMD to remove and replace with a blank space
                if (line.Contains(CMDtoRemove))
                {
                    line = line.Replace(CMDtoRemove, CMDtoReplace);
 
                    Console.WriteLine("<!VPSX-MsgType>DEBUG");
                    Console.WriteLine("String " + CMDtoRemove + " Found and Removed");
                }

                // Write the line to the output file
                using (StreamWriter sw = File.AppendText(OutputFileName))
                {
                    sw.WriteLine(line);
                }
            }

            // close the file and done
            file.Close();
            // Delete the temp file
            File.Delete(TempFile);

            // Post a message to the LRS Printer Log
            Console.WriteLine("<!VPSX-MsgType>DEBUG");
            Console.WriteLine("Done Close File and Delete Tempory File:");

            Console.WriteLine("<!VPSX-MsgType>DEBUG");
            Console.WriteLine(TempFile);

            return 0;

        }
    }
}