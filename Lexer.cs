using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Collections.Specialized;
using System.Windows.Forms;

namespace Compiler
{
    internal class Lexer
    {
        string temp = "";//key
        string value = "";//value
        public string error = "";
        int iteration = 0;
        /*
        * Takes a string input input from what is typed into textbox1 in the GUI. 
        * Takes a hashtable to add and keep track of variables
        * Works for signle variable cases, delims space values. 
        * Goes line by line. TBD
        */
        public bool Lexer1(string input, Hashtable data, int i)
        {
            while (temp.Contains("program") == false)
            {
                temp = temp + input[i] ;
                i++;

                if (i > input.Length)// i made this < instead of > and got so scared i broke everything, i couldnt figure out what was wrong
                {
                    throw new Exception("Bruh, you didnt even put program.");
                }
            }
            //handles duplicates
            //if (data.ContainsKey(temp) || data.ContainsValue(temp))
            //{
            //    temp = temp + "'";
            //    data.Add(temp,value);
            //    temp = "";
            //    return;
            //}
//          i = i + 2;//accounts for new line 

            temp = "";
            while (i< input.Length)
            {
                if (input[i].Equals(';') || temp.Equals("end_program") || input[i].Equals(')'))
                {
                    if (temp != "end_program")//recursive begin, if temp != endprogram.
                    {
                        /*
                         * If is new, only the else existed before
                         */
                        //if (value.Length == 0)
                        //{
                            //data.Add(iteration + ": " + temp, value);
                            //iteration++;
                           // temp = "";
                          //  value = "";
                         //   i++;
                        //}
                        //else
                       // {
                            temp = "";
                            value = "";
                            i++;
                       // }
                     //   i = i + 3;//acount for /n/r
                     //Lexer1(input, data, i);

                     /*
                      * Recursive, but not necesary???
                      */
                    }
                    else
                    {
                        break;
                    }
                }

                //else if (input[i].Equals(' '))
                //{
                //    i++;
                //}

                else if (input[i].Equals('='))
                {
                    i++;

                    //if (i >= input.Length)
                    //{
                    //    throw new Exception("input OOB error, did you forget a ;?");
                    //}

                    /*
                     * ;error exception handling. Idea to compare numbers. 
                     */

                    while (input[i].Equals(';') == false)
                    {
                        value = value + input[i];
                        i++;

                        if (i >= input.Length)
                        {
                            error = "input OOB error, did you forget a ;?";
                            return false;
                            //throw new Exception("input OOB error, did you forget a ;?");
                        }
                        else if (value.EndsWith("="))
                        {
                            error = "input OOB error, did you forget a ;?";
                            return false;
                        }
                        //just in case, Nash, isnt this not an error? technically its saying a long variable is equal to something
                        //So it shouldnt throw and error. It only throws an error if there is an equals, because this it is an error.
                    }

                    if (data.ContainsKey(temp) || data.ContainsValue(temp))
                    {
                        temp = temp + "'";
                        data.Add(iteration + ": " + temp, value);
                        temp = "";
                        iteration++;
                        //return; Im an idiot, i spent forever trying to figure out why the data table was short lol
                    }
                    else
                    {
                        data.Add(iteration + ": " + temp, value);
                        iteration++;
                    }

                }

                //else if (input[i].Equals('+') || input[i].Equals('-') ||input[i].Equals('*') ||input[i].Equals('/') ||input[i].Equals('%'))
                //{
                //    Operand(input, i, data);
                //}

                else if (temp.Equals("loop("))
                {
                    bool loopChecksE = true;
                    bool loopChecksS = true;
                    while (input[i].Equals(')') == false)
                    {
                        if (input[i].Equals('='))
                        {
                            if (!loopChecksE)
                            {
                                error = "More than one equals in a loop.";
                                return false;
                            }
                            loopChecksE = false;
                        }
                        else if (input[i].Equals(':'))
                        {
                            if (!loopChecksS)
                            {
                                error = "more than one : in loop";
                                return false;
                            }
                            loopChecksS = false;
                        }
                        value = value + input[i];
                        i++;
                    }
                    if (!value.Contains('=') || value.Contains('+') || value.Contains('-') || value.Contains('/') || value.Contains('*') || !value.Contains(':'))
                    {
                        error = "loop syntax error";
                        return false;
                    }
                    data.Add(iteration+": " +temp, value+")");
                    temp = "";
                    value = "";
                    iteration++;
                }
            

                else if (temp.Equals("end_loop"))
                {
                    if (data.ContainsKey(temp) || data.ContainsValue(temp))
                    {
                        temp = temp + "'";
                        //return; Im an idiot, i spent 3 days trying to figure out why the data table was short lol
                        //2 HOURS LATER AND IT WAS NOT THAT, IT WAS A SNEAKY LOOP ISSUE, AND NOW IT OUTPUTS CODE BACKWARDS 
                        //WHICH WOULDNT BE A PROBLEM, BUT THE FIRST VARIABLE IS CORRECT SO IT MAKES NO SENSE!!!!!
                    }
                    data.Add(iteration + ": " + temp, value);
                    temp = "";
                    value = "";
                    iteration++;
                }

                else if (temp.Equals("if("))
                {
                    bool loopChecksE = true;
                    bool loopChecksS = true;
                    if (i >= input.Length)
                    {
                        error = "Error. did you forget a )?";
                        return false;
                    }
                    while (input[i].Equals(')') == false)
                    {
                        if (input[i].Equals('='))
                        {
                            if (!loopChecksE)
                            {
                                error = "More than one equals in a loop.";
                                return false;
                            }
                            loopChecksE = false;
                        }
                        else if (input[i].Equals(':') || input[i].Equals(';') || input[i].Equals('+') || input[i].Equals('-') || input[i].Equals('/') || input[i].Equals('*') || input[i].Equals('%'))
                        {
                               error = "Syntax error in if";
                               return false;
                        }
                        value = value + input[i];
                        i++;
                        if (i >= input.Length)
                        {
                            error = "Error. did you forget a )?";
                            return false;
                        }
                    }
                    data.Add(iteration + ": " + temp, value+")");
                    temp = "";
                    value = "";
                    iteration++;
                }

                else if (temp.Equals("end_if"))
                {
                    if (data.ContainsKey(temp) || data.ContainsValue(temp))
                    {
                        temp = temp + "'";
                        //return; Im an idiot, i spent days trying to figure out why the data table was short lol
                    }
                    data.Add(iteration + ": " + temp, value);
                    temp = "";
                    value = "";
                    iteration++;
                }

                else
                {
                    temp = temp + input[i];
                    i++;//was outside......
                }
            }
            return true;
        }

        /*
         * returns temp, which is the key name.
         */
        public string key()
        {
            return temp;
        }

        /*
         * returns value, which is the data stored in the key value.
         */
        public string valueE()
        {
            return value;
        }

        public void Operand(string userInput, int i, Hashtable data)
        {

        }

        public void Orderer(Hashtable data, SortedDictionary<string, string> dataSorted)
        {
            int i = 0;
            int l = 0;
            string[] keys = new string[data.Keys.Count];
            int j = 0;
            int k = 0;
            ICollection keysC = data.Keys;
            ICollection valuesC = data.Values;
            string[] values = new string[data.Keys.Count];

            foreach (var item in keysC)
            {
                keys[j] = item.ToString();
                j++;
            }

            foreach (var item in valuesC)
            {
                values[k] = item.ToString();
                k++;

            }

            //This Logic was (future) is (presently) a pain in my a word
            while (i<iteration)
            {
                foreach (var key in keys)
                {
                    if (key.StartsWith(i + ":") == true)
                    {
                         dataSorted.Add(key, values[l]);
                         break;
                    }
                    l = l + 1;
                }
                l = 0;
                i++;
            }
            //Lets GOOOOOOOO it works, the issue was i was not accounting for the iterations of each loop, so my L was not accurate
            //Essentially, i was only sorting the keys, not the values.

            //while (i < iteration)
            //{
            //    foreach (var dat in data)
            //    {
            //        if (dat.ToString().Contains(i+": ") == true)
            //        {
            //            dataSorted.Add(dat.ToString().);
            //        }
            //        i++;
            //    }
            //}
        }
    }
}
