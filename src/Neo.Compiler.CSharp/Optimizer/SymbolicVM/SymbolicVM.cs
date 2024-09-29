// Copyright (C) 2015-2024 The Neo Project.
//
// SymbolicVM.cs file belongs to the neo project and is free
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
using System;
using System.Collections.Generic;
using System.Linq;

namespace Neo.Optimizer
{
    public class SymbolicVM : ExecutionEngine
    {
        public new void ExecuteNext()
        {
            try
            {
                base.ExecuteNext();
            }
            catch (Exception ex)
            {
                while (CurrentContext?.EvaluationStack.Count > 0)
                    CurrentContext.EvaluationStack.Pop();
            }
        }
    }
}
