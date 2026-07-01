using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIC_And_SICXE
{
    public class Line:ICloneable
    {

        public string Address { get; set; }

        public string Label { get; set; }

        public string Instruction { get; set; }

        public string Operand { get; set; }

        public string ObjectCode { get; set; }

        public object Clone()
        {
            return new Line
            {
                Label = Label,
                Instruction = Instruction,
                Operand = Operand,
                Address = Address,
                ObjectCode = ObjectCode
            };
        }
    }
    public class Header
    {
       

    }
}
