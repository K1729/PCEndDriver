using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1.Model
{
    /// <summary>
    /// Instruction class includes instructions and indicator that tells if said instruction is completed.
    /// </summary>
    class Instruction
    {
        public bool newCommand { get; set; }
        private string instructions;
        
        public Instruction() { }

        public override string ToString()
        {
            return instructions;
        }

        public Instruction(bool newcommand, string str)
        {
            newCommand = newcommand;
            instructions = str;
        }
    }
}
