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
                    List<Line> line = new List<Line>();
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
    }
}
