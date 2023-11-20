using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace codewhileloop
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }        

        bool result;
        string error="";
        bool crusleft = false;
        bool comentslach = false;
        bool crusright = false;
        bool bacetcrusleft = false;
        bool bacetcrusright = false;
        string andor = "";

        //دالة البناء 
        private void button1_Click(object sender, EventArgs e)
        {
            //التخلص من الفراغات والنزول سطر
            string code;
            Text2 = richTextBox1.Text;
            code = richTextBox1.Text.Trim();
            richTextBox1.Text = "";
            colorsChange(Text2.Trim());
            chickerror(Text2.Trim());
            code = code.Replace("\n", " ");
            code = code.Replace("\t", " ");
            code = code.Replace(" ", "");            
            //الرموز الطرفية وغير الطرفية...
            string op= @"(>|<|==|!=|>=|<=)";
            string op2 = @"(&&|\|\|)";
            string bools = @"(true|false)";
            string varInt = @"(((int_([a-zA-Z]+))((=)(\d+|([a-zA-Z]+)))*));+";
            string varString = @"(((string_([a-zA-Z]+))((=)('([a-zA-Z]*)'))*));+";
            string Condation = @"((([a-zA-Z]+)" + op + @"(\d+|([a-zA-Z]+)))|" + bools + ")";
            string equleVar= @"((([a-zA-Z]+)(=|\+=|-=|\*=)(\d+|([a-zA-Z]+))));+";
            string equleVar2 = @"((([a-zA-Z]+)(=)('([a-zA-Z]+)')));+";
            string coments = @"(//(([a-zA-Z]|>|<|==|!=|>=|<=|\+|-|=|!|\$|\@|'|;|\d|_|\.)*)\.\.\.)";
            string MulteCondation = @"(" + Condation+@")" + "("+op2 + @"(" + Condation +@")"+")+";
            string varIncer= @"(([a-zA-Z]+)(((\+\+)|(\-\-))+);)";
            string BLOCKST = @"((statement;+)|"+ varIncer + "|"+ equleVar +"|"+ equleVar2 + "|" + varInt+"|"+ varString + "|"+ coments + ")";            
            //التعبير المنطقي
            Regex Dowhileloop = new Regex(@"do{(statement;|"+ BLOCKST + @")+}While\((("+ Condation + @")|(" + MulteCondation + @")+)\);");            
            result = Dowhileloop.IsMatch(code);            
            if(richTextBox1.Text=="")
            {
                richTextBox2.ForeColor = Color.YellowGreen;
                button3.BackColor = Color.FromArgb(250, 7, 7);
                richTextBox2.Text = " it's Empty!   >_< ";
            }
            else
            {
                if(result)
            {
                richTextBox2.ForeColor = Color.LightGreen;
                button3.BackColor = Color.LightGreen;
                richTextBox2.Text = "  Your Code Is Exact ....";
                error = "";
                crusleft = false;
                crusright = false;
                bacetcrusleft = false;
                bacetcrusright = false;
                comentslach = false;
                    andor = "";
                }
            else
            {
                richTextBox2.ForeColor = Color.FromArgb(250,7, 7);
                button3.BackColor = Color.FromArgb(250, 7, 7);
                    richTextBox2.Text = " ";                    
                    if (!bacetcrusleft)
                        error = "{";
                    else if(!bacetcrusright)
                        error = "{";
                    else if(!crusleft)
                        error = "(";
                    else if (!crusright)
                        error = ")";
                    else if(!comentslach)
                        error = "/";
                    else if(andor!="")
                        error = andor;
                    richTextBox2.UpdateText(new ColouredText("  Your Code Is Wrong! it's ( ", Color.Red, Color.Transparent));
                    richTextBox2.UpdateText(new ColouredText(error, Color.White, Color.Transparent));
                    richTextBox2.UpdateText(new ColouredText(" )  >_<", Color.Red, Color.Transparent));

                }
            }        
        }

        void chickerror(string text)
        {
            for (int i = 0; i < text.Length; i++)
            {

                if ((text[i] == '+') && (text[i - 1] != '+' && text[i + 1] != '+'))
                    error = "+";
                else if ((text[i] == '-') && (text[i - 1] != '-' && text[i + 1] != '-'&& text[i + 1] != '='))
                    error = "-";
                else if (text[i] == '\n' && text[i - 2] != ';' && text[i - 2] != '.' && text[i - 1] != '{' && text[i - 2] != '{' && text[i + 1] != '}')
                {
                    for (int s = 0; s < 3; s++)
                    {
                        if (text[i - s] != ';' && text[i - s] != '{' && text[i - s] != '/')
                        {
                            error = ";";
                            break;
                        }
                        else
                            error = "";
                    }
                }
                else if (text[i] == ')' && (text[text.Length - 1] != ';' || i == text.Length - 1))
                    error = ";";
            }
        }

        //دالة التلوين#####################################################
        void colorsChange(string text)
        {

            for(int i=0;i< text.Length;i++)
            {
  
                if (text.Length > i + 2 && text[i] == 'd' && text[i + 1] == 'o'&& (text[i + 2] == '\n' || text[i + 2] == ' ' || text[i + 2] == '{'))
                {
                    richTextBox1.UpdateText(new ColouredText("do", Color.DodgerBlue, Color.Transparent));
                    i += 2;
                }
                if (text.Length > i + 3 && text[i] == 'i' && text[i + 2] == 't' && (text[i + 3] == '_'))
                {
                    richTextBox1.UpdateText(new ColouredText("int_", Color.DodgerBlue, Color.Transparent));
                    i += 4;
                }
                if (text.Length > i + 6 && text[i] == 's' && text[i + 5] == 'g' && (text[i + 6] == '_'))
                {
                    richTextBox1.UpdateText(new ColouredText("string_", Color.DodgerBlue, Color.Transparent));
                    i += 7;
                }
                if (text.Length > i + 5 && text[i] == 'W' && text[i + 4] == 'e' && (text[i + 5] == ' ' || text[i + 5] == '(' || text[i + 2] == '\n'))
                {
                    richTextBox1.UpdateText(new ColouredText("While", Color.DodgerBlue, Color.Transparent));
                    i += 5;
                }
                if (text[i] == '{' || text[i] == '}')
                {
                    if (text[i] == '{')
                        bacetcrusleft = true;
                    else
                        bacetcrusright = true;
                    richTextBox1.UpdateText(new ColouredText(text[i].ToString(), Color.Violet, Color.Transparent));
                }
                else if (text[i] == '(' || text[i] == ')')
                {
                    if (text[i] == '(')
                        crusleft = true;
                    else
                        crusright = true;
                    richTextBox1.UpdateText(new ColouredText(text[i].ToString(), Color.Yellow, Color.Transparent));                   
                }
                else if (text[i] == '&'|| text[i] == '|' || text[i] == '=' || text[i] == '+')
                {
                    if ((text[i] == '&'|| text[i] == '|') && (text[i + 1] != text[i] && text[i - 1] != text[i]))
                        andor = text[i].ToString();                    
                    richTextBox1.UpdateText(new ColouredText(text[i].ToString(), Color.Snow, Color.Transparent));
                    
                }
                else if (text[i] == '-' || text[i] == '\'')
                {
                    
                    richTextBox1.UpdateText(new ColouredText(text[i].ToString(), Color.Orange, Color.Transparent)); 
                }
                else if (text[i] == ';')
                    richTextBox1.UpdateText(new ColouredText(text[i].ToString(), Color.Red, Color.Transparent));
                else if (text[i] == '/')
                {
                    if (text[i] == '/' && (text[i + 1] != '/' && text[i - 1] != '/'))
                    {                       
                        for (int j = i; j < text.Length; j++)
                        {
                            if (text[j] == '\n')
                            {                               
                                richTextBox1.UpdateText(new ColouredText("... \n", Color.SlateBlue, Color.Transparent));
                                i = j ;
                                break;
                            }
                            else if(text[j] == '.')
                            {
                                richTextBox1.UpdateText(new ColouredText("...", Color.SlateBlue, Color.Transparent));
                                i = j+2;
                                break;
                            }
                            richTextBox1.UpdateText(new ColouredText(text[j].ToString(), Color.SlateBlue, Color.Transparent));
                        }
                    }
                    else
                    {
                        comentslach = true;
                        for (int j = i; j < text.Length; j++)
                        {
                            if (text[j] == '\n')
                            {
                                richTextBox1.UpdateText(new ColouredText("... \n", Color.ForestGreen, Color.Transparent));                                
                                i = j ;
                                //button1_Click(null, null);
                                break;
                            }
                            else if (text[j] == '.')
                            {
                                richTextBox1.UpdateText(new ColouredText("...", Color.ForestGreen, Color.Transparent));
                                i = j + 2;
                                break;
                            }
                            richTextBox1.UpdateText(new ColouredText(text[j].ToString(), Color.ForestGreen, Color.Transparent));
                        }
                    }
                }
                else
                {
                    
                    richTextBox1.UpdateText(new ColouredText(text[i].ToString(), Color.Turquoise, Color.Transparent));
                }
            }
            
        }

        string Text2;
        private void Form1_Load(object sender, EventArgs e)
        {
            Text2 = "do\n{\n //This Simple Compiler for Statement DoWhile in C++ \n\t statement;\n\t statement;\n\t string_ NameCoputer='HP';\n\t int_ number=10;\n\t name='ali'\n\t string_ text;\n\t int_ count;\n\t y-=10;\n\t count=x;\n\t x++;\n} While (x==10 && true);";
            colorsChange(Text2.Trim());
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            richTextBox2.ForeColor = Color.White;
            richTextBox2.Text = "  Writing ....";
            button3.BackColor = Color.Teal;
           
        }

        private void richTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar==Convert.ToChar(Keys.Enter))
            {
                Text2 = richTextBox1.Text;
                richTextBox1.Text = "";
                colorsChange(Text2.Trim());
            }
            
        }
    }
}
