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
                        currentLine.Address = (Convert.ToInt32(previousLine.Address, 16) + 3).ToString("X4");
                    }
                    else if (previousLine.Instruction == "WORD")
                    {
                        currentLine.Address = (Convert.ToInt32(previousLine.Address, 16) + 3).ToString("X4");

                    }
                    else if (previousLine.Instruction == "RESW")
                    {
                        int opdecimalFormat = Convert.ToInt32(previousLine.Operand) * 3;
                        int ophexderimalFormat = Convert.ToInt32(opdecimalFormat.ToString("X4"), 16);
                        int finalResult = opdecimalFormat;

                        int newAddress = Convert.ToInt32(previousLine.Address, 16) + finalResult;

                        currentLine.Address = newAddress.ToString("X4");

                    }
                    else if(previousLine.Instruction == "RESB")
                    {
                        //int opdecimalFormat = Convert.ToInt32(previousLine.Operand) +1;
                        //int ophexderimalFormat = Convert.ToInt32(opdecimalFormat.ToString("X2"), 16);
                        //currentLine.Address = newAddress.ToString("X2");
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
                    bool isIndex = false;
                    string operand = item.Operand;
                    if (item.Operand.Contains(","))
                    {
                        isIndex = true;
                        operand = operand.Split(',')[0];

                    }
                    var selectedItem = sampleTable.FirstOrDefault(a => a.Label == operand);
                   
                  string objectCOde =  GetObjectCode(loadInstructionSet[item.Instruction].OpCode, isIndex, Convert.ToInt32( selectedItem.Address,16));

                    item.ObjectCode = objectCOde;

               }
               else if (item.Instruction == "WORD")
                {

                    string decimalString = item.Operand;
                    int number = int.Parse(decimalString);
                    string hex = number.ToString("X6");

                    item.ObjectCode = $"{hex}";

                }
                else
                {
                    item.ObjectCode = "NO_OBJECT_CODE";
                }
            }

            dataGridView1.Refresh();
        }
        private string GetObjectCode(string opcodeHex, bool indexed, int address)
        {
            int opcode = Convert.ToInt32(opcodeHex, 16);
            if (indexed)
            {
                address |= 0x8000;
            }
          
            return $"{opcode:X2}{address.ToString("X4")}";
        }
        public int CalcuateLenght(List<Line> lines)
        {
            int start = Convert.ToInt32( line.FirstOrDefault().Operand,16);
            int last = Convert.ToInt32(line.LastOrDefault().Address, 16);

            return last - start;
        }
     

        private void calcuateHTERecordsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StringBuilder builder = new StringBuilder();
            string programName = line.FirstOrDefault().Label;
            string startingAddress = line.FirstOrDefault().Operand;
            string programLenght = CalcuateLenght(line).ToString("X6");
            builder.Append($"H.{programName}.{programLenght}");

            int lineCounter = 0;
            var filtered = line
             .Where(a => !string.IsNullOrEmpty(a.Label))
             .Select(a => (Line)a.Clone())
             .ToList();
            filtered.RemoveAt(0);

            List<HTERecord> hTERecords = new List<HTERecord>();
            hTERecords.Add(new HTERecord());
            foreach (var item in filtered)
            {
                if (lineCounter > 10 || item.ObjectCode == "NO_OBJECT_CODE")
                {
                    hTERecords.Add(new HTERecord());
                }
                else
                {
                    var lastHte = hTERecords.LastOrDefault();
                    lastHte.ObjectCode.Add(Convert.ToInt32( item.ObjectCode,16));

                }
            }
            builder.AppendLine("\n");
            foreach (var item in hTERecords)
            {
                builder.Append(item.ToString());
            }
          


            txtHTERecord.Text = builder.ToString();

        }
        public class HTERecord
        {
            public List<int> ObjectCode { get; set; } = new List<int>();

            public int GetLenght()
            {
                int start = ObjectCode.FirstOrDefault();
                int last = ObjectCode.LastOrDefault();
                return last - start;
            }
            public int GetFirst()
            {
                return ObjectCode.FirstOrDefault();
            }
            public override string ToString()
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append($"T.{GetFirst()}.{GetLenght().ToString("X2")}");
                foreach (var item in ObjectCode)
                {
                    stringBuilder.Append(item.ToString("X6"));
                }
                return stringBuilder.ToString();
            }

        }
    }
}
