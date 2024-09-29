// Copyright (C) 2015-2024 The Neo Project.
//
// ReadWrite.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using Neo.Json;
using Neo.SmartContract;
using Neo.SmartContract.Manifest;
using Neo.VM;
using static Neo.Optimizer.OpCodeTypes;
using System.Collections.Generic;

namespace Neo.Optimizer
{
    public static class ReadWrite
    {
        /// <summary>
        /// Used for not writing static classes and static methods in source code.
        /// The script will generate an empty array in the eval stack, used as arg 0
        /// for each non-static method.
        /// 
        /// For any method entry point (including _initialize and _deploy),
        /// just execute it (skipping _initialize)
        /// until the first INITSLOT in its invocation stack is met.
        /// If there is no write operations to storage or static fields,
        /// set the entrance address to the INITSLOT
        /// </summary>
        /// <param name="nef"></param>
        /// <param name="manifest"></param>
        /// <param name="debugInfo"></param>
        /// <returns></returns>
        [Strategy(Priority = 1 << 5)]
        public static (NefFile, ContractManifest, JObject?) RemoveNonEffective(NefFile nef, ContractManifest manifest, JObject? debugInfo = null)
        {
            InstructionCoverage coverage = new(nef);
            Script script = nef.Script;
            foreach (ContractMethodDescriptor method in manifest.Abi.Methods)
            {
                SymbolicVM vm = new();
                vm.LoadScript(script, initialPosition: method.Offset);
                Instruction i = vm.CurrentContext!.CurrentInstruction!;
                while (vm.State != VMState.HALT && vm.State != VMState.FAULT)
                {
                    if (vm.InvocationStack.Count == 1)
                    {
                        if (i.OpCode == OpCode.INITSLOT || i.OpCode == OpCode.SYSCALL)
                            break;
                        if (storeStaticFields.Contains(i.OpCode) || loadStaticFields.Contains(i.OpCode))
                            break;
                    }
                    if (vm.InvocationStack.Count > 1)
                    {
                        if (i.OpCode == OpCode.SYSCALL)
                            break;
                        if (storeStaticFields.Contains(i.OpCode))
                            break;
                    }
                    vm.ExecuteNext();
                    i = vm.CurrentContext!.CurrentInstruction!;
                }
                if (vm.State != VMState.HALT && vm.State != VMState.FAULT)
                    method.Offset = vm.CurrentContext.InstructionPointer;
            }
            return (nef, manifest, debugInfo);
        }
    }
}
