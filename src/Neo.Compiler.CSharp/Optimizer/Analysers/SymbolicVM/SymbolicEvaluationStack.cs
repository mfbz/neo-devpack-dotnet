using Neo.VM.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Neo.Optimizer
{
    class SymbolicEvaluationStack
    {
        uint popCount = 0;
        List<StackItem> innerList = new();  // last item is last in
        List<SymbolicVariable> existingVariables = new();  // first item is last in
        public int Count => innerList.Count;

        internal void Clear()
        {
            innerList.Clear();
        }

        internal void CopyTo(SymbolicEvaluationStack stack, int count = -1)
        {
            if (count < -1 || count > innerList.Count)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (count == 0) return;
            if (count == -1 || count == innerList.Count)
                stack.innerList.AddRange(innerList);
            else
                stack.innerList.AddRange(innerList.Skip(innerList.Count - count));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void Insert(int index, StackItem item)
        {
            if (index > innerList.Count) throw new InvalidOperationException($"Insert out of bounds: {index}/{innerList.Count}");
            innerList.Insert(innerList.Count - index, item);
        }

        internal void MoveTo(SymbolicEvaluationStack stack, int count = -1)
        {
            if (count == 0) return;
            CopyTo(stack, count);
            if (count == -1 || count == innerList.Count)
                innerList.Clear();
            else
                innerList.RemoveRange(innerList.Count - count, count);
        }

        /// <summary>
        /// Returns the item at the specified index from the top of the stack without removing it.
        /// </summary>
        /// <param name="index">The index of the object from the top of the stack.</param>
        /// <returns>The item at the specified index.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public StackItem Peek(int index = 0)
        {
            if (index >= innerList.Count)
            {
                popCount = (uint)index;
                StackItem result = new SymbolicVariable(this.GetType(), popCount);
                return result;
            }
            if (index < 0)
            {
                index += innerList.Count;
                if (index < 0) throw new InvalidOperationException($"Peek out of bounds: {index}/{innerList.Count}");
            }
            return innerList[innerList.Count - index - 1];
        }

        StackItem this[int index] => Peek(index);

        /// <summary>
        /// Pushes an item onto the top of the stack.
        /// </summary>
        /// <param name="item">The item to be pushed.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Push(StackItem item)
        {
            innerList.Add(item);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void Reverse(int n)
        {
            if (n < 0 || n > innerList.Count)
                throw new ArgumentOutOfRangeException(nameof(n));
            if (n <= 1) return;
            innerList.Reverse(innerList.Count - n, n);
        }

        /// <summary>
        /// Removes and returns the item at the top of the stack.
        /// </summary>
        /// <returns>The item removed from the top of the stack.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SymbolicStackItem Pop()
        {
            return Remove<SymbolicStackItem>(0);
        }

        /// <summary>
        /// Removes and returns the item at the top of the stack and convert it to the specified type.
        /// </summary>
        /// <typeparam name="T">The type to convert to.</typeparam>
        /// <returns>The item removed from the top of the stack.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Pop<T>() where T : StackItem
        {
            if (innerList.Count > 0) return Remove<T>(0);
            StackItem result = new SymbolicVariable(this.GetType(), popCount);
            popCount++;
            return (T)result;
        }

        internal T Remove<T>(int index) where T : StackItem
        {
            if (index >= innerList.Count)
                throw new ArgumentOutOfRangeException(nameof(index));
            if (index < 0)
            {
                index += innerList.Count;
                if (index < 0)
                    throw new ArgumentOutOfRangeException(nameof(index));
            }
            index = innerList.Count - index - 1;
            if (innerList[index] is not T item)
                throw new InvalidCastException($"The item can't be casted to type {typeof(T)}");
            innerList.RemoveAt(index);
            return item;
        }

        public void PushConst(StackItem item) { Push(new SymbolicConst(item)); }
        public void PushSymbolicDepth() { Push(new SymbolicVariable(this.GetType(), uint.MaxValue)); }
    }
}
