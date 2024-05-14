using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Compiler
{
    public partial class Form1 : Form
    {
        bool hasBeenRun = false;
        //  string[] userInput;
        bool buttonClicked = false;
        //Hashtable data = new Hashtable();
        //SortedDictionary<string, string> dataSorted = new SortedDictionary<string, string>();
        public Form1()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            //bool errorHandle = false;
            pictureBox3.Visible = false;
            pictureBox4.Visible = false;
            pictureBox5.Visible = false;
            if (hasBeenRun)
            {
                textBox3.Text = "";
                hasBeenRun = false;
            }
            string[] userInput;
            Hashtable data = new Hashtable();
            SortedDictionary<string, string> dataSorted = new SortedDictionary<string, string>();
            //just gets it into a array
            userInput = textBox1.Text.Split();
            string temp = "";
            string newUserInput = "";
            for (int i = 0; i < userInput.Length - 1; i++)
            {
                //if (temp.Contains(");"))
                //{
                //    textBox3.Text = "You messed up a loop, aka you did ); you sneaky dog you";
                //    return;
                //}
                temp = "" + userInput[i] + "" + userInput[i + 1];
                if (temp.Equals("\\n") || temp.Equals("\\r"))
                {
                    userInput[i] = null;
                    userInput[i + 1] = null;
                }
                temp = "";
            }
            for (int i = 0; i < userInput.Length; i++)
            {
                if (userInput[i].Equals(null))
                {
                    return;
                }
                else 
                { 
                    newUserInput = newUserInput+userInput[i];
                }   
            }

            if (newUserInput.Contains(");") || newUserInput.Contains(";;"))
            {
                if (newUserInput.Contains(";;"))
                {
                    textBox3.Text =  "Nah man, loose ; somewhere";
                    hasBeenRun = true;
                    errorPictures();
                    return;
                }
                textBox3.Text = "OOOOH i caught you trying to do the thing. You did a loop and tried to put a semi-colon after. nice try.";
                hasBeenRun = true;
                errorPictures();
                return;
            }

            if (!newUserInput.Contains("end_program"))
            {
                resetHandler("Error. Did you forget end_program?");
                hasBeenRun = true;
                errorPictures();
                return;
                //throw new Exception("You forgot program end.");
            }

            Lexer lexer = new Lexer();
            Parser parser = new Parser();
            if(!lexer.Lexer1(newUserInput, data, 0))//calls lexer starting at 0. returns false if error.
            {
                textBox3.Text = lexer.error;
                hasBeenRun = true;
                errorPictures();
                return;
            }

            if (data.Count == 0)
            {
                resetHandler("Error. Did you forget program?");
                hasBeenRun = true;
                errorPictures();
                return;
                //throw new Exception("Error. Did you forget program or program_end?");
            }

            lexer.Orderer(data, dataSorted);
            string[] output = new string[dataSorted.Count + +3];//+2 for start and end program. update, was +2 now its +3, because i changed the way integers are handled.
            output[0] = "<program>";

            if (!parser.Parser1(dataSorted,output))
            { 
                textBox3.Text = parser.error;
                hasBeenRun = true;
                errorPictures();
                return;
            }

            foreach(var item in output)
            {
                textBox3.AppendText(item+"\n");
                textBox3.AppendText(Environment.NewLine);
                textBox3.AppendText(Environment.NewLine);
            }
            data = null;
            dataSorted = null;
            newUserInput = null;
            output = null;
            hasBeenRun = true;
           // Reset reset = new Reset();

        }

        public void resetHandler(string text)
        {
            textBox3.Text = text;
        }

        public void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        
        public void changeTextBox(string error)
        {
            textBox3.Text = error;
        }

        //public string[] getInput()
        //{
        //   // return userInput;
        //}

        public void textBox3_TextChanged(object sender, EventArgs e)
        {
            pictureBox4.Visible = true;
            pictureBox3.Visible = true;
        }

        public void errorPictures()
        {
            pictureBox3.Visible=false;
            pictureBox4.Visible=false;
            pictureBox5.Visible = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
