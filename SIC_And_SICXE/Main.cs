using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SIC_And_SICXE
{
    public partial class Main : Form
    {
        List<Line> line = new List<Line>();
        List<SampleTableRecord> sampleTable = new List<SampleTableRecord>();
        public Main()
        {
            InitializeComponent();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using(OpenFileDialog fileDialog = new OpenFileDialog())
            {
               if( fileDialog.ShowDialog() == DialogResult.OK)
               {
                   string[] data =  File.ReadAllLines(fileDialog.FileName);
                  
                    foreach (var item in data)
                    {
                        
                        var stringList = item.Split(' ').ToList();
                      
                            stringList.RemoveAll(a => string.IsNullOrEmpty(a));
                            string[] currentLine = stringList.ToArray();
                        if (currentLine.Length == 3)
                        {
                            line.Add(new Line()
                            {
                                Address = "",
                                Label = currentLine[0],
                                Instruction = currentLine[1],
                                Operand = currentLine[2],
                                ObjectCode = ""
                            });
                        }
                        else if(currentLine.Length == 2)
                        {
                            line.Add(new Line()
                            {
                                Address = "",
                                Label = "",
                                Instruction = currentLine[0],
                                Operand = currentLine[1]
                            });
                        }
                       
                    }
                    dataGridView1.DataSource = line;

               }

            }
        }

        private void calculateLOCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var instructionSet = Instruction.GetInstructionSet();
            var indexedSet = line.ToList();
            string lastAddress = "";

            for (int i = 0; i < line.Count; i++)
            {
                if (i == 0)
                {
                    indexedSet[i].Address = indexedSet[i].Operand;
                }
                else if (i == 1)
                {
             
                    indexedSet[i].Address = indexedSet[i - 1].Address;
                }
                else
                {
                    var currentLine = indexedSet[i];
                    var previousLine = indexedSet[i - 1];
                    if (instructionSet.ContainsKey(previousLine.Instruction))
                    {
                        currentLine.Address = (Convert.ToInt32(previousLine.Address, 16) + 3).ToString("X2");
                    }
                    else if (previousLine.Instruction == "WORD")
                    {
                        currentLine.Address = (Convert.ToInt32(previousLine.Address, 16) + 3).ToString("X2");

                    }
                    else if (previousLine.Instruction == "RESW")
                    {
                        int opdecimalFormat = Convert.ToInt32(previousLine.Operand) * 3;
                        int ophexderimalFormat = Convert.ToInt32(opdecimalFormat.ToString("X2"), 16);
                        int finalResult = opdecimalFormat;

                        int newAddress = Convert.ToInt32(previousLine.Address, 16) + finalResult;

                        currentLine.Address = newAddress.ToString("X2");

                    }
                    

                }
            }


                dataGridView1.Refresh();
        }

        private void calculateSampleTableToolStripMenuItem_Click(object sender, EventArgs e)
        {

            var filtered = line
              .Where(a => !string.IsNullOrEmpty(a.Label))
              .Select(a => (Line) a.Clone())
              .ToList();
            filtered.RemoveAt(0);
            foreach (var item in filtered)
            {
                sampleTable.Add(new SampleTableRecord()
                {
                    Label = item.Label,
                    Address = item.Address
                });
            }
            sampleTableGrid.DataSource = sampleTable;
        }

        private void calcuateObjectCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var loadInstructionSet = Instruction.GetInstructionSet();
            foreach (var item in line)
            {
               if( loadInstructionSet.ContainsKey(item.Instruction))
               {
                  string objectCOde =  GetObjectCode(loadInstructionSet[item.Instruction].OpCode, false, Convert.ToInt32(item.Address, 16));

                    item.ObjectCode = objectCOde;

               }
               else
               {
                    item.ObjectCode = "NO_OBJECT_CODE";
               }
            }

            dataGridView1.Refresh();
        }
        string GetObjectCode(string opcodeHex, bool indexed, int address)
        {
            string[] arrayOfObjectCode = new string[6];
            int opcode = Convert.ToInt32(opcodeHex, 16);
            string opcodeStr = opcode.ToString("X2"); // "X2" = 2 hex digits
            arrayOfObjectCode[0] = opcodeStr[0].ToString();
            arrayOfObjectCode[1] = opcodeStr[1].ToString();

            arrayOfObjectCode[2] = indexed ? "1" : "0";
            string addressString = address.ToString("X3");
            arrayOfObjectCode[3] = addressString[0].ToString();
            arrayOfObjectCode[4] = addressString[1].ToString();
            arrayOfObjectCode[5] = addressString[2].ToString();


            string hexObjectCode = string.Join("", arrayOfObjectCode);

            return hexObjectCode;

        }
    }
}
