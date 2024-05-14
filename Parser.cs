using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Compiler
{
    internal class Parser
    {
        public string error = "";


        public bool Parser1(SortedDictionary<string, string> data,string[] oData)
        {
            Stack loopChecker = new Stack();//im so smart for using this nash please notice

            //string[] keys = new string[data.Keys.Count];
            //string[] values = new string[data.Keys.Count];
            //int i = 0, j = 0;
            //ICollection keysC = data.Keys;
            //ICollection valuesC = data.Values;
            //foreach (var item in keysC)
            //{
            //    keys[i] = item.ToString();
            //    i++;
            //}
            //i = 0;

            //foreach (var item in keysC)
            //{
            //    if (keys[i].Contains("loop("))
            //    {

            //    }
            //    else if (keys[i].Contains("end_loop"))
            //    {

            //    }
            //    else if (keys[i].Contains("if("))
            //    {

            //    }
            //    else if (keys[i].Contains("end_if"))
            //    { 

            //    }
            //    else
            //    {
            //        i++;
            //    }
            //}
            //foreach (var items in valuesC)
            //{
            //    values[j] = items.ToString();
            //    j++;                
            //}

            /*GRAMMAR WALKTHROUGH:
             * <program> -> <program><statements>||<program><loop>||<endprogram>
             * <statements> -> <operand><statement>||<statement>||<loop><statement>||<endl>
             * <operand> -> (statement)||+||-||/||*||%||<logic>||endl
             * <logic> -> !||=||etc.
             * <loop> - <data><logic><data><loop>||<endloop>
             * 
             * Maybe not, i might just ignore this. The walkthrough is on the file? kinda? i dont kneoeeeejriwehtiewriuqewnotinw
             * 
             * i TOTALLY shouldve used a for loop, not foreach loop. For each is causing me too much pain.
             * WHY is there no function for getting previous??????
             */
            int i = 1;
            foreach (var item in data)
            {
                if (item.Key.Equals(i - 1 + ": loop("))//loop is 0
                {
                    loopChecker.Push(0);
                    if(!item.Value.Contains(":") || item.Value.Contains(";"))
                    {
                        error = "faulty if, you forgot : or added some weird stuff";
                        return false;
                    }
                    oData[i] = "<loop>(<var>=<var>:<var>)";
                }
                else if (item.Key.Equals(i - 1 + ": if("))//if is 1
                {
                    loopChecker.Push(1);
                    //start if chain for determining which logic of an if.
                    if (item.Value.Contains("=="))
                    {
                        oData[i] = "if(<var>(==)<var>)";
                    }
                    else if (item.Value.Contains("!="))
                    {
                        oData[i] = "if(<var>(!=)<var>)";
                    }
                    else if (item.Value.Contains(">="))
                    {
                        oData[i] = "if(<var>(>=)<var>)";
                    }
                    else if (item.Value.Contains("=<"))
                    {
                        oData[i] = "if(<var>(=<)<var>)";
                    }
                    else if (item.Value.Contains(">"))
                    {
                        oData[i] = "if(<var>(>)<var>)";
                    }
                    else if (item.Value.Contains("<"))
                    {
                        oData[i] = "if(<var>(<)<var>)";
                    }
                    else
                    {
                        error = "faulty loop/if";
                        return false;
                    }
                }
                else if (item.Key.Equals(i - 1 + ": end_if"))
                {
                    oData[i] = "<end_if>";

                    if (loopChecker.Count == 0)
                    {
                        error = "You forgot add an if you goofball you";
                        return false;
                    }
                    else if (loopChecker.Count == 0 || loopChecker.Pop().Equals(0))
                    {
                        error = "You forgot if you goofball you";
                        return false;
                    }


                    //loopChecker.Pop();
                }
                else if (item.Key.Equals(i - 1 + ": end_loop"))
                {
                    oData[i] = "<end_loop>";

                    if (loopChecker.Count == 0||loopChecker.Pop().Equals(1))
                    {
                        error = "You forgot loop you goofball you";
                        return false;
                    }

                   // loopChecker.Pop();
                }
                else//repetition 
                {
                    bool flag = false;
                    bool flagForChar = true;
                    bool flagForP = false;

                    oData[i] = "<statements><operand>";
                    foreach (char c in item.Value)
                    {
                        if (c.Equals('/') || c.Equals('+') || c.Equals('-') || c.Equals('*') || c.Equals('%'))// need to do ()
                        {
                            if (flagForP)//sees is previous is parth
                            {
                                oData[i] = oData[i] + "<operand>";
                                flagForChar = false;
                            }
                            else if (flag == false)
                            {
                                oData[i] = oData[i] + "<statements>" + "<operand>";
                                flag = true;
                            }
                            else
                            {
                                oData[i] = oData[i] + "<statements>" + "<operand>";
                            }
                        }

                        else if (System.Char.IsDigit(c) && flagForChar && i + 1 < item.Value.Length && System.Char.IsDigit(item.Value[i + 1]))
                        {
                            int p = i;
                            //loop for integers, b/c 32 is still a number, dumb.
                            while (p < item.Value.Length && System.Char.IsDigit(item.Value[p]))
                            {
                                p++;//i dont think there is a point for me to have p here, but im keeping it just in case.
                            }
                            oData[i] = oData[i] + "<statements>";
                            flagForChar = false;
                        }
                        else if (c.Equals('('))
                        {
                            oData[i] = oData[i] + "<(>";
                        }
                        else if (c.Equals(')'))
                        {
                            oData[i] = oData[i] + "<statements>";
                            oData[i] = oData[i] + "<)>";
                            flagForP = true;
                        }
                        //else if (c.Equals(';'))
                        //{
                        //    error = "free lying ; i think";
                        //    return false;
                        //}
                        //oData[i] = oData[i] + "<statements>";
                    }
                    oData[i] = oData[i] + "<statements>";
                }
                i++;
            }
            oData[i] = "<end_program>";

            if (loopChecker.Count != 0)//checks to make sure loopchecker is 0. If its not, then something wasnt done right
            {
                if (loopChecker.Contains(0) && loopChecker.Contains(1))
                {
                    error = "You forgot a end_loop AND an end_if, you REALLY suck at coding.";
                    return false;
                }
                else if (loopChecker.Contains(0))
                {
                    if (!oData.Contains("end_loop"))
                    {
                        error = "You SUPER sneaky dog, but i caught you again. you tried putting a colon after end_loop, or you messed up the loop. please try again:)";
                        return false;
                    }
                    error = "You forgot a end_loop, i think ( i hope )";
                    return false;
                }
                else if (loopChecker.Contains(1))
                {
                    if (!oData.Contains("end_if;"))
                    {
                        error = "You SUPER sneaky dog, but i caught you again. you tried putting a colon after end_if, or you messed up the loop. please try again:)";
                        return false;
                    }
                    error = "You forgot a end_if, i think ( i hope )";
                    return false;
                }
            }

            return true;
        }
    }
}
