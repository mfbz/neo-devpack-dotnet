using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Neo.VM.Types
{
    public class SymStackItem : StackItem
    {
        public StackItemType inferredType = StackItemType.Any;
        public override StackItemType Type => inferredType;

        public int createdByInstructionAddr;

        public override bool GetBoolean()
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        public new SymStackItem GetInteger() => this;
        public static SymStackItem operator +(SymStackItem a) => a;
        public static SymStackItem operator -(SymStackItem a) => a;
        public static SymStackItem operator ~(SymStackItem a) => a;
        public static SymStackItem operator +(SymStackItem a, StackItem b) => a;
        public static SymStackItem operator +(StackItem a, SymStackItem b) => b;
        public static SymStackItem operator -(SymStackItem a, StackItem b) => a;
        public static SymStackItem operator -(StackItem a, SymStackItem b) => b;
        public static SymStackItem operator *(SymStackItem a, StackItem b) => a;
        public static SymStackItem operator *(StackItem a, SymStackItem b) => b;
        public static SymStackItem operator /(SymStackItem a, StackItem b) => a;
        public static SymStackItem operator /(StackItem a, SymStackItem b) => b;
        public static SymStackItem operator %(SymStackItem a, StackItem b) => a;
        public static SymStackItem operator %(StackItem a, SymStackItem b) => b;
        public static SymStackItem operator &(SymStackItem a, StackItem b) => a;
        public static SymStackItem operator &(StackItem a, SymStackItem b) => b;
        public static SymStackItem operator |(SymStackItem a, StackItem b) => a;
        public static SymStackItem operator |(StackItem a, SymStackItem b) => b;
    }
}
