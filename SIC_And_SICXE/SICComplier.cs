using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SIC_And_SICXE
{
    public class SICComplier
    {
        List<Line> _line = new List<Line>();
        List<SampleTableRecord> _sampleTable = new List<SampleTableRecord>();

        public List<Line> Line { get => _line; set => _line = value; }
        public List<SampleTableRecord> SampleTable { get => _sampleTable; set => _sampleTable = value; }

        public  void ReadAllLines(string fileName)
      {
            string[] data = File.ReadAllLines(fileName);

            foreach (var item in data)
            {

                var stringList = item.Split(' ').ToList();

                stringList.RemoveAll(a => string.IsNullOrEmpty(a));
                string[] currentLine = stringList.ToArray();
                if (currentLine.Length == 3)
                {
                    Line.Add(new Line()
                    {
                        Address = "",
                        Label = currentLine[0],
                        Instruction = currentLine[1],
                        Operand = currentLine[2],
                        ObjectCode = ""
                    });
                }
                else if (currentLine.Length == 2)
                {
                    Line.Add(new Line()
                    {
                        Address = "",
                        Label = "",
                        Instruction = currentLine[0],
                        Operand = currentLine[1]
                    });
                }

            }
        }
        public void CalculateLocationCounter()
        {
            var instructionSet = Instruction.GetInstructionSet();
            var indexedSet = _line.ToList();
       

            for (int i = 0; i < _line.Count; i++)
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
                    else if (previousLine.Instruction == "RESB")
                    {
                        
                    }



                }
            }
        }

        public List<SampleTableRecord> GetSampleTable()
        {
            var filtered = _line
             .Where(a => !string.IsNullOrEmpty(a.Label))
             .Select(a => (Line)a.Clone())
             .ToList();
            filtered.RemoveAt(0);
            foreach (var item in filtered)
            {
                _sampleTable.Add(new SampleTableRecord()
                {
                    Label = item.Label,
                    Address = item.Address
                });
            }
            return _sampleTable;
           
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

        public void CalculateObjectCode()
        {

            var loadInstructionSet = Instruction.GetInstructionSet();
            foreach (var item in _line)
            {
                if (loadInstructionSet.ContainsKey(item.Instruction))
                {
                    bool isIndex = false;
                    string operand = item.Operand;
                    if (item.Operand.Contains(","))
                    {
                        isIndex = true;
                        operand = operand.Split(',')[0];

                    }
                    var selectedItem = _sampleTable.FirstOrDefault(a => a.Label == operand);

                    string objectCOde = GetObjectCode(loadInstructionSet[item.Instruction].OpCode, isIndex, Convert.ToInt32(selectedItem.Address, 16));

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
        }

        public int CalcuateLenght()
        {
            int start = Convert.ToInt32(_line.FirstOrDefault().Operand, 16);
            int last = Convert.ToInt32(_line.LastOrDefault().Address, 16);

            return last - start;
        }

        public string GetHTERecord()
        {
            StringBuilder builder = new StringBuilder();
            string programName = _line.FirstOrDefault().Label;
            string startingAddress = _line.FirstOrDefault().Operand;
            string programLenght = CalcuateLenght().ToString("X6");
            builder.Append($"H^{programName}^{programLenght}");

            int lineCounter = 0;
            var filtered = _line
             .Select(a => (Line)a.Clone())
             .ToList();
            filtered.RemoveAt(0);

            List<HTERecords> hTERecords = new List<HTERecords>();
            hTERecords.Add(new HTERecords());
            foreach (var item in filtered)
            {
                if (item.ObjectCode == "NO_OBJECT_CODE")
                {



                    var lastHteRecord = hTERecords.LastOrDefault();
                    if (lastHteRecord != null)
                    {
                        lastHteRecord.ObjectCode.Add(new HTERecord() { Address = item.Address, ObjectCode = item.ObjectCode });
                    }

                    hTERecords.Add(new HTERecords());
                    lineCounter = 0;
                }
                else if (lineCounter > 9)
                {

                    var lastHteRecord = hTERecords.LastOrDefault();
                    if (lastHteRecord != null)
                    {
                        lastHteRecord.ObjectCode.Add(new HTERecord() { Address = item.Address, ObjectCode = item.ObjectCode });
                    }

                    hTERecords.Add(new HTERecords());

                    var lastHte = hTERecords.LastOrDefault();

                    lastHte.ObjectCode.Add(new HTERecord() { Address = item.Address, ObjectCode = item.ObjectCode });

                    lineCounter = 0;
                }
                else
                {
                    var lastHte = hTERecords.LastOrDefault();

                    lastHte.ObjectCode.Add(new HTERecord() { Address = item.Address, ObjectCode = item.ObjectCode });

                }
                lineCounter++;
            }

            hTERecords.RemoveAll(a => a.ObjectCode.Count == 1 && a.ObjectCode.Any(c => c.ObjectCode == "NO_OBJECT_CODE"));
            hTERecords.RemoveAll(a => a.ObjectCode.Count == 0);

            foreach (var item in hTERecords)
            {
                builder.AppendLine();
                builder.Append(item.ToString());
            }
            builder.AppendLine();
            builder.Append($"E^{int.Parse(startingAddress, NumberStyles.HexNumber):X6}");

            return  builder.ToString();
        }
    }
}
