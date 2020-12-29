using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace LuanNiao.Core.StructUtilTools
{
    /// <summary>
    /// struct type tools 
    /// </summary>
    public static class StructUtilTools
    {

        /// <summary>
        /// use to handle the span data with struct, waring: this method means that your struct was mapping to memory, the target struct can't have any reference type.        
        /// </summary>
        /// <typeparam name="T">target struct type, cann't have reference type</typeparam>
        /// <param name="data">source data</param>
        /// <see cref="AsRef{T}(Span{byte})"/>
        /// <exception cref="ArgumentException"/>
        /// <returns>target struct with ref</returns>

        public static ref T AsRef<T>(Span<byte> data) where T : struct
        {
            if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
            {
                throw new ArgumentException($"InvalidTypeWithPointersNotSupported because {nameof(T)} is contains references ");
            }

            if (Unsafe.SizeOf<T>() > (uint)data.Length)
            {
                throw new ArgumentException($"ArgumentOutOfRangeException because", nameof(data));
            }

            return ref Unsafe.As<byte, T>(ref MemoryMarshal.GetReference(data));
        }

        /// <summary>
        /// use to handle the span data with struct, waring: this method means that your struct was mapping to memory, the target struct can have the reference type with the mashalas attribute.  
        /// </summary>
        /// <typeparam name="T">target struct type, can have the reference type with the mashalas attribute</typeparam>
        /// <param name="data">source data</param>
        /// <see cref="AsRef{T}(Span{byte})"/>
        /// <exception cref="ArgumentException"/>
        /// <returns>target struct with ref</returns>
        public static ref T ToStruct<T>(Span<byte> data) where T : struct
        {
            if (Unsafe.SizeOf<T>() > (uint)data.Length)
            {
                throw new ArgumentException($"ArgumentOutOfRangeException because", nameof(data));
            }

            ref var start = ref MemoryMarshal.GetReference(data);
            IntPtr addr;
            unsafe { addr = (IntPtr)Unsafe.AsPointer<byte>(ref start); };
            object tmp = Marshal.PtrToStructure<T>(addr);
            return ref Unsafe.Unbox<T>(tmp);
        }

        /// <summary>
        /// marshal the struct to bytes with span
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Span<byte> AsRef<T>(ref T t) where T : struct
        {
            if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
            {
                throw new ArgumentException($"InvalidTypeWithPointersNotSupported because {nameof(T)} is contains references ");
            }

            Span<T> valSpan = MemoryMarshal.CreateSpan(ref t, 1);
            Span<byte> sp = MemoryMarshal.AsBytes(valSpan);
            return sp;
        }

        /// <summary>
        /// marshal the struct to bytes with span
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static byte[] ToData<T>(in T t) where T : struct
        {
            byte[] data = new byte[Marshal.SizeOf<T>()];

            GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
            Marshal.StructureToPtr(t, handle.AddrOfPinnedObject(), false);
            handle.Free();

            return data;
        }

        /// <summary>
        /// marshal the struct to bytes with span
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static int CopyDataTo<T>(in T t, in Span<byte> buffer) where T : struct
        {
            byte[] data = new byte[Marshal.SizeOf<T>()];

            GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
            Marshal.StructureToPtr(t, handle.AddrOfPinnedObject(), false);
            handle.Free();
            data.AsSpan().CopyTo(buffer);
            return data.Length;
        }

        /// <summary>
        /// marshal the bytes data to target struct
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static Span<T> ArrayFromData<T>(byte[] data, int offset) where T : struct
        {
            return MemoryMarshal.Cast<byte, T>(data.AsSpan(offset));
        }

        /// <summary>
        ///  marshal the bytes data to target struct
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static Span<T> ArrayFromData<T>(Span<byte> bytes) where T : struct
        {
            return MemoryMarshal.Cast<byte, T>(bytes);
        }
    }
}
