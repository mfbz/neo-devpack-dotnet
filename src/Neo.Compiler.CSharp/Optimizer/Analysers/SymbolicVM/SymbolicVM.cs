using Neo.VM;
using Neo.VM.Types;
using static Neo.VM.OpCode;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System;

namespace Neo.Optimizer
{
    class SymbolicVM
    {
        SymbolicEvaluationStack symbolicEvaluationStack = new();

        SymbolicStaticFields staticFields = new();
        SymbolicArguments arguments = new();
        SymbolicLocalVariables localVariables = new();

        T PopCheckConst<T>() where T : StackItem
        {
            T n = symbolicEvaluationStack.Remove<T>(0);
            if (n is not SymbolicConst)
                // TODO: a special exception
                throw new NotImplementedException($"Cannot execute opcode without const input {n}");
            return n;
        }
        int PopIntegerCheckConst()
        {
            SymbolicStackItem n = symbolicEvaluationStack.Pop();
            if (n is not SymbolicConst)
                // TODO: a special exception
                throw new NotImplementedException($"Cannot execute opcode without const input {n}");
            return (int)((SymbolicConst)n).GetInteger();
        }
        public SymbolicVM(Script script, Dictionary<int, Instruction> basicBlock)
        {
            basicBlock = basicBlock.OrderBy(obj => obj.Key).ToDictionary(obj => obj.Key, obj => obj.Value);
            List<Instruction> instructions = basicBlock.Values.ToList();
            foreach (Instruction instruction in instructions.SkipLast(1))
                if (OpCodeTypes.allowedBasicBlockEnds.Contains(instruction.OpCode))
                    throw new BadScriptException("Bad basic block with an ending instruction in the middle");
            foreach ((int address, Instruction instruction) in basicBlock)
            {
                OpCode opcode = instruction.OpCode;
                switch (opcode)
                {
                    case OpCode _ when OpCodeTypes.pushInt.Contains(opcode):
                        symbolicEvaluationStack.PushConst(new BigInteger(instruction.Operand.Span));  break;
                    case PUSHT:
                        symbolicEvaluationStack.PushConst(true);  break;
                    case PUSHF:
                        symbolicEvaluationStack.PushConst(false);  break;
                    case PUSHA:
                        int position = checked(address + instruction.TokenI32);
                        if (position < 0 || position > script.Length)
                            throw new InvalidOperationException($"Bad pointer address: {position}");
                        symbolicEvaluationStack.PushConst(new Pointer(script, position));
                        break;
                    case PUSHNULL:
                        symbolicEvaluationStack.PushConst(StackItem.Null);  break;
                    case OpCode _ when OpCodeTypes.pushData.Contains(opcode):
                        symbolicEvaluationStack.PushConst(instruction.Operand);  break;
                    case OpCode _ when OpCodeTypes.pushConstInt.Contains(opcode):
                        symbolicEvaluationStack.PushConst((int)instruction.OpCode - (int)PUSH0);  break;
                    case OpCode _ when OpCodeTypes.allowedBasicBlockEnds.Contains(opcode):
                        // DO NOTHING
                        break;
                    case DEPTH:
                        symbolicEvaluationStack.Push(SymbolicVariable.SymbolicEvaluationStackDepth());  break;
                    case DROP:
                        symbolicEvaluationStack.Pop();  break;
                    case NIP:
                        symbolicEvaluationStack.Remove<SymbolicStackItem>(1);  break;
                    case XDROP:
                        int nDrop = PopIntegerCheckConst();
                        if (nDrop < 0)
                            throw new InvalidOperationException($"The negative value {nDrop} is invalid for OpCode.{instruction.OpCode}.");
                        symbolicEvaluationStack.Remove<SymbolicStackItem>(nDrop);
                        break;
                    case CLEAR:
                        symbolicEvaluationStack.Clear();  break;
                    case DUP:
                        symbolicEvaluationStack.Push(symbolicEvaluationStack.Peek());  break;
                    case OVER:
                        symbolicEvaluationStack.Push(symbolicEvaluationStack.Peek(1));  break;
                    case PICK:
                        int nPick = PopIntegerCheckConst();
                        if (nPick < 0)
                            throw new InvalidOperationException($"The negative value {nPick} is invalid for OpCode.{instruction.OpCode}.");
                        symbolicEvaluationStack.Push(symbolicEvaluationStack.Peek(nPick));
                        break;
                    case TUCK:
                        symbolicEvaluationStack.Insert(2, symbolicEvaluationStack.Peek());   break;
                    case SWAP:
                        symbolicEvaluationStack.Push(symbolicEvaluationStack.Remove<SymbolicStackItem>(1));   break;
                    case ROT:
                        symbolicEvaluationStack.Push(symbolicEvaluationStack.Remove<SymbolicStackItem>(2));   break;
                    case ROLL:
                        int nRoll = PopIntegerCheckConst();
                        if (nRoll < 0)
                            throw new InvalidOperationException($"The negative value {nRoll} is invalid for OpCode.{instruction.OpCode}.");
                        if (nRoll == 0) continue;
                        symbolicEvaluationStack.Push(symbolicEvaluationStack.Remove<SymbolicStackItem>(nRoll));
                        break;
                    case REVERSE3:
                        symbolicEvaluationStack.Reverse(3);  break;
                    case REVERSE4:
                        symbolicEvaluationStack.Reverse(4);  break;
                    case REVERSEN:
                        int nReverse = PopIntegerCheckConst();
                        symbolicEvaluationStack.Reverse(nReverse);
                        break;

                    case OpCode _ when OpCodeTypes.loadStaticFieldsConst.Contains(opcode):
                        symbolicEvaluationStack.Push(staticFields[opcode - LDSFLD0]);  break;
                    case LDSFLD:
                        symbolicEvaluationStack.Push(staticFields[instruction.TokenU8]);  break;
                    case OpCode _ when OpCodeTypes.storeStaticFieldsConst.Contains(opcode):
                        staticFields[opcode - STSFLD0] = symbolicEvaluationStack.Pop();  break;
                    case STSFLD:
                        staticFields[instruction.TokenU8] = symbolicEvaluationStack.Pop();  break;

                    case OpCode _ when OpCodeTypes.loadLocalVariablesConst.Contains(opcode):
                        symbolicEvaluationStack.Push(localVariables[opcode - LDLOC0]);  break;
                    case LDLOC:
                        symbolicEvaluationStack.Push(localVariables[instruction.TokenU8]);  break;
                    case OpCode _ when OpCodeTypes.storeLocalVariablesConst.Contains(opcode):
                        localVariables[opcode - STLOC0] = symbolicEvaluationStack.Pop();  break;
                    case STLOC:
                        localVariables[instruction.TokenU8] = symbolicEvaluationStack.Pop();  break;

                    case OpCode _ when OpCodeTypes.loadArgumentsConst.Contains(opcode):
                        symbolicEvaluationStack.Push(arguments[opcode - LDARG0]); break;
                    case LDARG:
                        symbolicEvaluationStack.Push(arguments[instruction.TokenU8]);  break;
                    case OpCode _ when OpCodeTypes.storeLocalVariablesConst.Contains(opcode):
                        arguments[opcode - STARG0] = symbolicEvaluationStack.Pop();  break;
                    case STARG:
                        arguments[instruction.TokenU8] = symbolicEvaluationStack.Pop(); break;

                    case NEWBUFFER:
                        SymbolicStackItem i = symbolicEvaluationStack.Pop();
                        if (i is SymbolicConst)
                            symbolicEvaluationStack.Push(new Neo.VM.Types.Buffer((int)i.GetInteger()));
                        else
                            symbolicEvaluationStack.Push(new SymbolicVariable(opcode, new() { i }));
                        break;
                    case MEMCPY:
                        int count = PopIntegerCheckConst();
                        if (count < 0)
                            throw new InvalidOperationException($"The value {count} is out of range.");
                        int si = PopIntegerCheckConst();
                        if (si < 0)
                            throw new InvalidOperationException($"The value {si} is out of range.");
                        ReadOnlySpan<byte> src = symbolicEvaluationStack.Pop().GetSpan();
                        if (checked(si + count) > src.Length)
                            throw new InvalidOperationException($"The value {count} is out of range.");
                        int di = PopIntegerCheckConst();
                        if (di < 0)
                            throw new InvalidOperationException($"The value {di} is out of range.");
                        Neo.VM.Types.Buffer dst = PopCheckConst<Neo.VM.Types.Buffer>();
                        if (checked(di + count) > dst.Size)
                            throw new InvalidOperationException($"The value {count} is out of range.");
                        src.Slice(si, count).CopyTo(dst.InnerBuffer.Span[di..]);
                        break;
                    case CAT:
                        //var x2 = Pop().GetSpan();
                        //var x1 = Pop().GetSpan();
                        //int length = x1.Length + x2.Length;
                        //Limits.AssertMaxItemSize(length);
                        //Buffer result = new(length, false);
                        //x1.CopyTo(result.InnerBuffer.Span);
                        //x2.CopyTo(result.InnerBuffer.Span[x1.Length..]);
                        //Push(result);
                        break;
                    //default:
                    //    throw new BadScriptException($"Unknown {nameof(OpCode)} {opcode}");
                }
            }
        }
    }
}
