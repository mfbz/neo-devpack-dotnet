using Neo.VM;
using Neo.VM.Types;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Neo.Optimizer
{
    abstract class SymbolicStackItem : StackItem
    {
        public override StackItemType Type => throw new System.NotImplementedException();
        public override bool GetBoolean() => throw new System.NotImplementedException();
    }

    abstract class SymbolicRandomAccessFields
    {
        protected Dictionary<uint, SymbolicStackItem> readed = new();
        protected Dictionary<uint, SymbolicStackItem> written = new();
        protected SymbolicStackItem GetThis(uint key)
        {
            if (!readed.ContainsKey(key))
                readed[key] = new SymbolicVariable(this.GetType(), key);
            return readed[key];
        }
        public SymbolicStackItem this[uint key]
        {
            get => GetThis(key);
            set { written[key] = value; readed[key] = value; }
        }
    }

    class SymbolicStaticFields : SymbolicRandomAccessFields { }
    class SymbolicArguments : SymbolicRandomAccessFields { }
    class SymbolicLocalVariables : SymbolicRandomAccessFields { }

    class SymbolicStorage
    {
        protected Dictionary<SymbolicStackItem, SymbolicStackItem> readed = new();  // symbolic key, value for storage
        protected Dictionary<SymbolicStackItem, SymbolicStackItem> written = new();  // symbolic key, value for storage
        protected SymbolicStackItem GetThis(SymbolicStackItem key)
        {
            if (!readed.ContainsKey(key))
                readed[key] = new SymbolicVariable(key);
            return readed[key];
        }
        public SymbolicStackItem this[SymbolicStackItem key]
        {
            get => GetThis(key);
            set { readed[key] = value; written[key] = value; }
        }
    }

    class SymbolicEvaluationStackDepth : SymbolicStackItem { }

    [DebuggerDisplay("SymbolicVariable {name}")]
    class SymbolicVariable : SymbolicStackItem, IEquatable<SymbolicVariable>
    {
        public readonly System.Type type;
        public readonly uint? index;
        public readonly SymbolicStackItem? key;
        public readonly OpCode? opcode;
        public readonly List<SymbolicStackItem>? operands;
        /// <summary>
        /// For <see cref="SymbolicRandomAccessFields"/> and <see cref="SymbolicEvaluationStack"/>
        /// </summary>
        /// <param name="type"><see cref="SymbolicLocalVariables"/><see cref="SymbolicStaticFields"/><see cref="SymbolicArguments"/></param>
        /// <param name="index">index of variable in <see cref="SymbolicRandomAccessFields"/></param>
        public SymbolicVariable(System.Type type, uint index)
        {
            this.type = type; this.index = index;
            this.opcode = null; this.operands = new();
        }
        public static SymbolicVariable SymbolicEvaluationStackDepth() => new SymbolicVariable(typeof(SymbolicEvaluationStackDepth), uint.MaxValue);
        /// <summary>
        /// For <see cref="SymbolicStorage"/>
        /// </summary>
        /// <param name="key"></param>
        public SymbolicVariable(SymbolicStackItem key)
        {
            this.type = typeof(SymbolicStorage);
            this.key = key;
        }
        /// <summary>
        /// For symbolic function
        /// </summary>
        /// <param name="opcode"></param>
        /// <param name="operands"></param>
        public SymbolicVariable(OpCode opcode, List<SymbolicStackItem> operands)
        {
            this.type = this.GetType();
            this.opcode = opcode; this.operands = operands;
        }

        public bool Equals(SymbolicVariable? other)
        {
            if (ReferenceEquals(this, other)) return true;
            if ((this == null) || (other == null)) return false;
            if (this.type != other.type) return false;
            if (this.index != other.index) return false;
            if (this.key != other.key) return false;
            if (this.opcode != other.opcode) return false;
            if (ReferenceEquals (this.operands, other.operands)) return true;
            if ((this.operands != null) && (other.operands != null))
                return this.operands.SequenceEqual(other.operands);
            return false;
        }
    }

    [DebuggerDisplay("SymbolicConst {stackItem}")]
    class SymbolicConst : SymbolicStackItem
    {
        StackItem stackItem;
        public SymbolicConst(StackItem stackItem) { this.stackItem = stackItem; }
    }
}
