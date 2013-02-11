﻿using System;

namespace SimTelemetry.Domain.Memory
{
    public interface IMemoryObject : ICloneable
    {
        string Name { get; }
        MemoryProvider Memory { get; }
        MemoryAddress AddressType { get; }

        bool IsDynamic { get; }
        bool IsStatic { get; }
        bool IsConstant { get; }

        MemoryPool Pool { get; }
        int Offset { get; }
        int Address { get; }
        int Size { get; }
        Type ValueType { get; }

        TOut ReadAs<TOut>();
        object Read();
        bool HasChanged();

        void Refresh();
        void SetProvider(MemoryProvider provider);
        void SetPool(MemoryPool pool);
    }
}