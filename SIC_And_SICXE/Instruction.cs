using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIC_And_SICXE
{
    public class Instruction
    {
        public string Mnemonic { get;  } = string.Empty;
        public string Format { get;  } = string.Empty;
        public string OpCode { get;  } = string.Empty;

        public Instruction(string mnemonic, string format, string opCode)
        {
            Mnemonic = mnemonic;
            Format = format;
            OpCode = opCode;
        }

        public static Dictionary<string, Instruction> GetInstructionSet()
        {
            Dictionary<string, Instruction> instructionTable = new Dictionary<string, Instruction>()
            {
                ["ADD"] = new Instruction("ADD", "3/4", "18"),
                ["ADDF"] = new Instruction("ADDF", "3/4", "58"),
                ["ADDR"] = new Instruction("ADDR", "2", "90"),
                ["AND"] = new Instruction("AND", "3/4", "40"),
                ["CLEAR"] = new Instruction("CLEAR", "2", "B4"),
                ["COMP"] = new Instruction("COMP", "3/4", "28"),
                ["COMPF"] = new Instruction("COMPF", "3/4", "88"),
                ["COMPR"] = new Instruction("COMPR", "2", "A0"),
                ["DIV"] = new Instruction("DIV", "3/4", "24"),
                ["DIVF"] = new Instruction("DIVF", "3/4", "64"),
                ["DIVR"] = new Instruction("DIVR", "2", "9C"),
                ["FIX"] = new Instruction("FIX", "1", "C4"),
                ["FLOAT"] = new Instruction("FLOAT", "1", "C0"),
                ["HIO"] = new Instruction("HIO", "1", "F4"),
                ["J"] = new Instruction("J", "3/4", "3C"),
                ["JEQ"] = new Instruction("JEQ", "3/4", "30"),
                ["JGT"] = new Instruction("JGT", "3/4", "34"),
                ["JLT"] = new Instruction("JLT", "3/4", "38"),
                ["JSUB"] = new Instruction("JSUB", "3/4", "48"),
                ["LDA"] = new Instruction("LDA", "3/4", "00"),
                ["LDB"] = new Instruction("LDB", "3/4", "68"),
                ["LDCH"] = new Instruction("LDCH", "3/4", "50"),
                ["LDF"] = new Instruction("LDF", "3/4", "70"),
                ["LDL"] = new Instruction("LDL", "3/4", "08"),
                ["LDS"] = new Instruction("LDS", "3/4", "6C"),
                ["LDT"] = new Instruction("LDT", "3/4", "74"),
                ["LDX"] = new Instruction("LDX", "3/4", "04"),
                ["LPS"] = new Instruction("LPS", "3/4", "D0"),
                ["MUL"] = new Instruction("MUL", "3/4", "20"),

                // ---- Added from page 2 & 3 ----
                ["MULF"] = new Instruction("MULF", "3/4", "60"),
                ["MULR"] = new Instruction("MULR", "2", "98"),
                ["NORM"] = new Instruction("NORM", "1", "C8"),
                ["OR"] = new Instruction("OR", "3/4", "44"),
                ["RD"] = new Instruction("RD", "3/4", "D8"),
                ["RMO"] = new Instruction("RMO", "2", "AC"),
                ["RSUB"] = new Instruction("RSUB", "3/4", "4C"),
                ["SHIFTL"] = new Instruction("SHIFTL", "2", "A4"),
                ["SHIFTR"] = new Instruction("SHIFTR", "2", "A8"),
                ["SIO"] = new Instruction("SIO", "1", "F0"),
                ["SSK"] = new Instruction("SSK", "3/4", "EC"),
                ["STA"] = new Instruction("STA", "3/4", "0C"),
                ["STB"] = new Instruction("STB", "3/4", "78"),
                ["STCH"] = new Instruction("STCH", "3/4", "54"),
                ["STF"] = new Instruction("STF", "3/4", "80"),
                ["STI"] = new Instruction("STI", "3/4", "D4"),
                ["STL"] = new Instruction("STL", "3/4", "14"),
                ["STS"] = new Instruction("STS", "3/4", "7C"),
                ["STSW"] = new Instruction("STSW", "3/4", "E8"),
                ["STT"] = new Instruction("STT", "3/4", "84"),
                ["STX"] = new Instruction("STX", "3/4", "10"),
                ["SUB"] = new Instruction("SUB", "3/4", "1C"),
                ["SUBF"] = new Instruction("SUBF", "3/4", "5C"),
                ["SUBR"] = new Instruction("SUBR", "2", "94"),
                ["SVC"] = new Instruction("SVC", "2", "B0"),
                ["TD"] = new Instruction("TD", "3/4", "E0"),
                ["TIO"] = new Instruction("TIO", "1", "F8"),
                ["TIX"] = new Instruction("TIX", "3/4", "2C"),
                ["TIXR"] = new Instruction("TIXR", "2", "B8"),
                ["WD"] = new Instruction("WD", "3/4", "DC"),
            };
            return instructionTable;
        }
    }
}
