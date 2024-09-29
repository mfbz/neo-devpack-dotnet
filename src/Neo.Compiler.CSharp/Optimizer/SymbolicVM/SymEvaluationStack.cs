using Akka.Util;
using Neo.VM.Types;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Neo.VM
{
    /// <summary>
    /// Represents the evaluation stack in the VM.
    /// </summary>
    public class SymEvaluationStack : EvaluationStack
    {
        public SymEvaluationStack(ReferenceCounter r) : base(r) { }
        public new void Clear() => base.Clear();

        public new void CopyTo(EvaluationStack stack, int count = -1)
        {
            if (count < -1)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (count == 0) return;
            IEnumerable<StackItem> copied = stack.innerList;
            if (count > innerList.Count)
                copied = Enumerable.Repeat(new SymStackItem(), count - innerList.Count).Concat(innerList);
            if (count == -1 || count == copied.Count())
                stack.innerList.AddRange(copied);
            else
                stack.innerList.AddRange(copied.Skip(innerList.Count - count));
        }

        public new IEnumerator<StackItem> GetEnumerator()
        {
            return innerList.GetEnumerator();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public new void Insert(int index, StackItem item)
        {
            if (index > innerList.Count)
                innerList.AddRange(Enumerable.Repeat(new SymStackItem(), index - innerList.Count));
            innerList.Insert(innerList.Count - index, item);
            referenceCounter.AddStackReference(item);
        }

        public new void MoveTo(EvaluationStack stack, int count = -1)
        {
            if (count == 0) return;
            if (count > innerList.Count)
                innerList.AddRange(Enumerable.Repeat(new SymStackItem(), count - innerList.Count));
            CopyTo(stack, count);
            if (count == -1 || count >= innerList.Count)
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
        public new StackItem Peek(int index = 0)
        {
            if (index >= innerList.Count)
                innerList.AddRange(Enumerable.Repeat(new SymStackItem(), index - innerList.Count));
            if (index < 0)
            {
                return new SymStackItem();  // TBD: we do not know the count of eval stack
                //index += innerList.Count;
                //if (index < 0) throw new InvalidOperationException($"Peek out of bounds: {index}/{innerList.Count}");
            }
            return innerList[innerList.Count - index - 1];
        }

        /// <summary>
        /// Pushes an item onto the top of the stack.
        /// </summary>
        /// <param name="item">The item to be pushed.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public new void Push(StackItem item) => base.Push(item);


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public new void Reverse(int n)
        {
            if (n < 0)
                throw new ArgumentOutOfRangeException(nameof(n));
            if (n > innerList.Count)
                innerList.AddRange(Enumerable.Repeat(new SymStackItem(), n - innerList.Count));
            if (n <= 1) return;
            innerList.Reverse(innerList.Count - n, n);
        }

        /// <summary>
        /// Removes and returns the item at the top of the stack.
        /// </summary>
        /// <returns>The item removed from the top of the stack.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public new StackItem Pop()
        {
            if (innerList.Count == 0)
                return new SymStackItem();
            return Remove<StackItem>(0);
        }

        /// <summary>
        /// Removes and returns the item at the top of the stack and convert it to the specified type.
        /// </summary>
        /// <typeparam name="T">The type to convert to.</typeparam>
        /// <returns>The item removed from the top of the stack.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public new StackItem Pop<T>() where T : StackItem
        {
            if (innerList.Count == 0)
                return new SymStackItem();
            return Remove<T>(0);
        }

        public new StackItem Remove<T>(int index) where T : StackItem
        {
            if (index >= innerList.Count)
                innerList.AddRange(Enumerable.Repeat(new SymStackItem(), index - innerList.Count));
            if (index < 0)
            {
                return new SymStackItem();  // TBD: we do not know the count of eval stack
                //index += innerList.Count;
                //if (index < 0)
                //    throw new ArgumentOutOfRangeException(nameof(index));
            }
            index = innerList.Count - index - 1;
            if (innerList[index] is SymStackItem symItem)
                return symItem;
            if (innerList[index] is not T item)
                throw new InvalidCastException($"The item can't be casted to type {typeof(T)}");
            innerList.RemoveAt(index);
            referenceCounter.RemoveStackReference(item);
            return item;
        }

    }
}
