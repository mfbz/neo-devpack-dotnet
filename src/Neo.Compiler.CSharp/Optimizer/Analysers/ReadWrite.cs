using Neo.Json;
using Neo.SmartContract;
using Neo.SmartContract.Manifest;
using Neo.VM;
using System.Collections.Generic;

namespace Neo.Optimizer
{
    static class ReadWrite
    {
        public static void AnalyseReadWrite(Script script, Dictionary<int, Instruction> basicBlock)
        {
            SymbolicVM svm = new(script, basicBlock);
        }
    }
}
