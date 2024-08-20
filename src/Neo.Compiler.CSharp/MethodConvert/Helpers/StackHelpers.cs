// Copyright (C) 2015-2024 The Neo Project.
//
// The Neo.Compiler.CSharp is free software distributed under the MIT
// software license, see the accompanying file LICENSE in the main directory
// of the project or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

extern alias scfx;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Neo.VM;
using System;
using System.Buffers.Binary;
using System.Numerics;
using scfx::Neo.SmartContract.Framework;
using OpCode = Neo.VM.OpCode;

namespace Neo.Compiler;

internal partial class MethodConvert
{
    #region Instructions
    private Instruction AddInstruction(Instruction instruction)
    {
        _instructions.Add(instruction);
        return instruction;
    }

    private Instruction AddInstruction(OpCode opcode)
    {
        return AddInstruction(new Instruction
        {
            OpCode = opcode
        });
    }

    private SequencePointInserter InsertSequencePoint(SyntaxNodeOrToken? syntax)
    {
        return new SequencePointInserter(_instructions, syntax);
    }

    private SequencePointInserter InsertSequencePoint(SyntaxReference? syntax)
    {
        return new SequencePointInserter(_instructions, syntax);
    }

    private SequencePointInserter InsertSequencePoint(Location? location)
    {
        return new SequencePointInserter(_instructions, location);
    }

    #endregion

    private Instruction Jump(OpCode opcode, JumpTarget target)
    {
        return AddInstruction(new Instruction
        {
            OpCode = opcode,
            Target = target
        });
    }

    private void Push(bool value)
    {
        AddInstruction(value ? OpCode.PUSHT : OpCode.PUSHF);
    }

    private Instruction Push(BigInteger number)
    {
        if (number == BigInteger.MinusOne) return AddInstruction(OpCode.PUSHM1);
        if (number >= BigInteger.Zero && number <= 16) return AddInstruction(OpCode.PUSH0 + (byte)number);
        byte n = number.GetByteCount() switch
        {
            <= 1 => 0,
            <= 2 => 1,
            <= 4 => 2,
            <= 8 => 3,
            <= 16 => 4,
            <= 32 => 5,
            _ => throw new ArgumentOutOfRangeException(nameof(number))
        };
        byte[] buffer = new byte[1 << n];
        number.TryWriteBytes(buffer, out _);
        return AddInstruction(new Instruction
        {
            OpCode = OpCode.PUSHINT8 + n,
            Operand = buffer
        });
    }

    private Instruction Push(string s)
    {
        return Push(Utility.StrictUTF8.GetBytes(s));
    }

    private Instruction Push(byte[] data)
    {
        OpCode opcode;
        byte[] buffer;
        switch (data.Length)
        {
            case <= byte.MaxValue:
                opcode = OpCode.PUSHDATA1;
                buffer = new byte[sizeof(byte) + data.Length];
                buffer[0] = (byte)data.Length;
                Buffer.BlockCopy(data, 0, buffer, sizeof(byte), data.Length);
                break;
            case <= ushort.MaxValue:
                opcode = OpCode.PUSHDATA2;
                buffer = new byte[sizeof(ushort) + data.Length];
                BinaryPrimitives.WriteUInt16LittleEndian(buffer, (ushort)data.Length);
                Buffer.BlockCopy(data, 0, buffer, sizeof(ushort), data.Length);
                break;
            default:
                opcode = OpCode.PUSHDATA4;
                buffer = new byte[sizeof(uint) + data.Length];
                BinaryPrimitives.WriteUInt32LittleEndian(buffer, (uint)data.Length);
                Buffer.BlockCopy(data, 0, buffer, sizeof(uint), data.Length);
                break;
        }
        return AddInstruction(new Instruction
        {
            OpCode = opcode,
            Operand = buffer
        });
    }

    private void Push(object? obj)
    {
        switch (obj)
        {
            case bool data:
                Push(data);
                break;
            case byte[] data:
                Push(data);
                break;
            case string data:
                Push(data);
                break;
            case BigInteger data:
                Push(data);
                break;
            case char data:
                Push((ushort)data);
                break;
            case sbyte data:
                Push(data);
                break;
            case byte data:
                Push(data);
                break;
            case short data:
                Push(data);
                break;
            case ushort data:
                Push(data);
                break;
            case int data:
                Push(data);
                break;
            case uint data:
                Push(data);
                break;
            case long data:
                Push(data);
                break;
            case ulong data:
                Push(data);
                break;
            case Enum data:
                Push(BigInteger.Parse(data.ToString("d")));
                break;
            case null:
                AddInstruction(OpCode.PUSHNULL);
                break;
            case float or double or decimal:
                throw new CompilationException(DiagnosticId.FloatingPointNumber, "Floating-point numbers are not supported.");
            default:
                throw new NotSupportedException($"Unsupported constant value: {obj}");
        }
    }

    private Instruction PushDefault(ITypeSymbol type)
    {
        return AddInstruction(type.GetStackItemType() switch
        {
            VM.Types.StackItemType.Boolean or VM.Types.StackItemType.Integer => OpCode.PUSH0,
            _ => OpCode.PUSHNULL,
        });
    }

    #region LabelsAndTargets

    private JumpTarget AddLabel(ILabelSymbol symbol, bool checkTryStack)
    {
        if (!_labels.TryGetValue(symbol, out JumpTarget? target))
        {
            target = new JumpTarget();
            _labels.Add(symbol, target);
        }
        if (checkTryStack && _tryStack.TryPeek(out ExceptionHandling? result) && result.State != ExceptionHandlingState.Finally)
        {
            result.Labels.Add(symbol);
        }
        return target;
    }

    #endregion

    private void PushSwitchLabels((SwitchLabelSyntax, JumpTarget)[] labels)
    {
        _switchStack.Push(labels);
        if (_tryStack.TryPeek(out ExceptionHandling? result))
            result.SwitchCount++;
    }

    private void PopSwitchLabels()
    {
        _switchStack.Pop();
        if (_tryStack.TryPeek(out ExceptionHandling? result))
            result.SwitchCount--;
    }

    private void PushContinueTarget(JumpTarget target)
    {
        _continueTargets.Push(target);
        if (_tryStack.TryPeek(out ExceptionHandling? result))
            result.ContinueTargetCount++;
    }

    private void PopContinueTarget()
    {
        _continueTargets.Pop();
        if (_tryStack.TryPeek(out ExceptionHandling? result))
            result.ContinueTargetCount--;
    }

    private void PushBreakTarget(JumpTarget target)
    {
        _breakTargets.Push(target);
        if (_tryStack.TryPeek(out ExceptionHandling? result))
            result.BreakTargetCount++;
    }

    private void PopBreakTarget()
    {
        _breakTargets.Pop();
        if (_tryStack.TryPeek(out ExceptionHandling? result))
            result.BreakTargetCount--;
    }

    /// <summary>
    /// Convert a throw expression or throw statement to OpCodes.
    /// </summary>
    /// <param name="model">The semantic model providing context and information about the Throw.</param>
    /// <param name="exception">The content of exception</param>
    /// <exception cref="CompilationException">Only a single parameter is supported for exceptions.</exception>
    /// <example>
    /// throw statement:
    /// <code>
    /// if (shapeAmount <= 0)
    /// {
    ///     throw new Exception("Amount of shapes must be positive.");
    /// }
    ///</code>
    /// throw expression:
    /// <code>
    /// string a = null;
    /// var b = a ?? throw new Exception();
    /// </code>
    /// <code>
    /// var first = args.Length >= 1 ? args[0] : throw new Exception();
    /// </code>
    /// </example>
    /// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/statements/exception-handling-statements#the-throw-expression">The throw expression</seealso>
    /// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/statements/exception-handling-statements#the-try-catch-statement">Exception-handling statements - throw</seealso>
    private void Throw(SemanticModel model, ExpressionSyntax? exception)
    {
        if (exception is not null)
        {
            var type = model.GetTypeInfo(exception).Type!;
            if (type.IsSubclassOf(nameof(UncatchableException), includeThisClass: true))
            {
                AddInstruction(OpCode.ABORT);
                return;
            }
        }
        switch (exception)
        {
            case ObjectCreationExpressionSyntax expression:
                switch (expression.ArgumentList?.Arguments.Count)
                {
                    case null:
                    case 0:
                        Push("exception");
                        break;
                    case 1:
                        ConvertExpression(model, expression.ArgumentList.Arguments[0].Expression);
                        break;
                    default:
                        throw new CompilationException(expression, DiagnosticId.MultiplyThrows, "Only a single parameter is supported for exceptions.");
                }
                break;
            case null:
                AccessSlot(OpCode.LDLOC, _exceptionStack.Peek());
                break;
            default:
                ConvertExpression(model, exception);
                break;
        }
        AddInstruction(OpCode.THROW);
    }
}