using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SIC_And_SICXE
{
    public partial class Main : Form
    {
        SICComplier SICComplier = new SICComplier();
       

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
                    SICComplier.ReadAllLines(fileDialog.FileName);
                    dataGridView1.DataSource = SICComplier.Line;

               }

            }
        }

    
        private void locationCounterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SICComplier.CalculateLocationCounter();
            dataGridView1.Refresh();

        }

        private void symbolTableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sampleTableGrid.DataSource = SICComplier.GetSampleTable();
        }

        private void objectCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SICComplier.CalculateObjectCode();
            dataGridView1.Refresh();
        }

        private void hTERecordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtHTERecord.Text = SICComplier.GetHTERecord();
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            SICComplier.Line.Clear();
            SICComplier.SampleTable.Clear();
            txtHTERecord.Text = "";
            dataGridView1.Refresh();
            sampleTableGrid.Refresh();
        }

        private void executeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SICComplier.CalculateLocationCounter();
            sampleTableGrid.DataSource = SICComplier.GetSampleTable();
            SICComplier.CalculateObjectCode();
            dataGridView1.Refresh();
            txtHTERecord.Text = SICComplier.GetHTERecord();
        }
    }
}
