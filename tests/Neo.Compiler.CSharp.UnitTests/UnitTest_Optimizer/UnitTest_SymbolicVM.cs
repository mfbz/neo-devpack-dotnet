using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neo.SmartContract.TestEngine;
using Neo.Optimizer;
using System.Collections.Generic;
using Neo.SmartContract;
using Neo.Json;
using Neo.SmartContract.Manifest;
using System.Linq;
using Neo.VM;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Neo.Compiler.CSharp.UnitTests.Optimizer
{
    [TestClass]
    public class UnitTest_SymbolicVM
    {
        private TestEngine testengine;

        public void Test_SingleContractSymbolicVM(string fileName)
        {
            testengine = new TestEngine();
            try
            {
                testengine.AddEntryScript(fileName);
            }
            catch (Exception e) { return; }
            (NefFile nef, ContractManifest manifest, JToken debugInfo) = (testengine.Nef, testengine.Manifest, testengine.DebugInfo);
            if (nef == null) { return; }
            Dictionary<int, Dictionary<int, VM.Instruction>> basicBlocks;
            try
            {
                basicBlocks = BasicBlock.FindBasicBlocks(nef, manifest, debugInfo);
            }
            catch (Exception e) { return; }
            foreach (Dictionary<int, VM.Instruction> basicBlock in basicBlocks.Values)
            {
                SymbolicVM symbolicVM = new(nef.Script, basicBlock);
            }
        }

        [TestMethod]
        public void Test_BasicBlockStartEnd()
        {
            string[] files = Directory.GetFiles(Utils.Extensions.TestContractRoot, "Contract*.cs");
            foreach (string file in files)
                Test_SingleContractSymbolicVM(file);
        }
    }
}
