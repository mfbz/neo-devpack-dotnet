// Copyright (C) 2015-2024 The Neo Project.
//
// Null.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using System;
using System.Diagnostics.CodeAnalysis;

namespace Neo.SymVM.Types
{
    /// <summary>
    /// Represents <see langword="null"/> in the VM.
    /// </summary>
    public class UnknownStackItem : SymStackItem
    {
        public new StackItemType Type;

        public UnknownStackItem(StackItemType type = StackItemType.Any)
        {
            this.Type = type;
        }

        public override SymStackItem ConvertTo(StackItemType type)
        {
            return new UnknownStackItem(type: type);
        }

        public new SymStackItem Equals(SymStackItem? other)
        {
            if (ReferenceEquals(this, other))
                return true;
            return new UnknownStackItem(type: StackItemType.Boolean);
        }

        public new SymStackItem GetBoolean()
        {
            return new UnknownStackItem(type: StackItemType.Boolean);
        }

        public new SymStackItem GetHashCode()
        {
            return new UnknownStackItem(type: StackItemType.Integer);
        }

        [return: MaybeNull]
        public override T GetInterface<T>()
        {
            return default;
        }

        public override string? GetString()
        {
            return ToString();
        }

        public override string ToString()
        {
            return $"UNKNOWN {nameof(SymStackItem)} of type {Type}";
        }
    }
}
