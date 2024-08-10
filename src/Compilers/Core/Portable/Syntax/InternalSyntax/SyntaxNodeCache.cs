// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.CodeAnalysis.Syntax.InternalSyntax;

using System;
using System.Diagnostics;
using Roslyn.Utilities;

#if STATS
using System.Threading;
#endif
namespace Microsoft.CodeAnalysis.Syntax.InternalSyntax
{
    internal static class SyntaxNodeCache
    {
        private const int CacheSizeBits = 16;
        private const int CacheSize = 1 << CacheSizeBits;
        private const int CacheMask = CacheSize - 1;

        private readonly struct Entry
        {
            public readonly int hash;
            public readonly GreenNode? node;

            internal Entry(int hash, GreenNode node)
            {
                this.hash = hash;
                this.node = node;
            }
        }

        private static readonly Entry[] s_cache = new Entry[CacheSize];

        internal static void AddNode(GreenNode node, int hash)
        {
            if (AllChildrenInCache(node) && !node.IsMissing)
            {
                Debug.Assert(node.GetCacheHash() == hash);

                var idx = hash & CacheMask;
                s_cache[idx] = new Entry(hash, node);
            }
        }

        private static bool CanBeCached(GreenNode? child1)
        {
            return child1 == null || child1.IsCacheable;
        }

        private static bool CanBeCached(GreenNode? child1, GreenNode? child2)
        {
            return CanBeCached(child1) && CanBeCached(child2);
        }

        private static bool CanBeCached(GreenNode? child1, GreenNode? child2, GreenNode? child3)
        {
            return CanBeCached(child1) && CanBeCached(child2) && CanBeCached(child3);
        }

        private static bool ChildInCache(GreenNode? child)
        {
            // for the purpose of this function consider that 
            // null nodes, tokens and trivias are cached somewhere else.
            // TODO: should use slotCount
            if (child == null || child.SlotCount == 0) return true;

            int hash = child.GetCacheHash();
            int idx = hash & CacheMask;
            return s_cache[idx].node == child;
        }

        private static bool AllChildrenInCache(GreenNode node)
        {
            // TODO: should use slotCount
            var cnt = node.SlotCount;
            for (int i = 0; i < cnt; i++)
            {
                if (!ChildInCache(node.GetSlot(i)))
                {
                    return false;
                }
            }

            return true;
        }

        internal static GreenNode? TryGetNode(int kind, GreenNode? child1, out int hash)
        {
            return TryGetNode(kind, child1, GetDefaultNodeFlags(), out hash);
        }

        internal static GreenNode? TryGetNode(int kind, GreenNode? child1, GreenNode.NodeFlags flags, out int hash)
        {
            if (CanBeCached(child1))
            {
                int h = hash = GetCacheHash(kind, flags, child1);
                int idx = h & CacheMask;
                var e = s_cache[idx];
                if (e.hash == h && e.node != null && e.node.IsCacheEquivalent(kind, flags, child1))
                {
                    return e.node;
                }
            }
            else
            {
                hash = -1;
            }

            return null;
        }

        internal static GreenNode? TryGetNode(int kind, GreenNode? child1, GreenNode? child2, out int hash)
        {
            return TryGetNode(kind, child1, child2, GetDefaultNodeFlags(), out hash);
        }

        internal static GreenNode? TryGetNode(int kind, GreenNode? child1, GreenNode? child2, GreenNode.NodeFlags flags, out int hash)
        {
            if (CanBeCached(child1, child2))
            {
                int h = hash = GetCacheHash(kind, flags, child1, child2);
                int idx = h & CacheMask;
                var e = s_cache[idx];
                if (e.hash == h && e.node != null && e.node.IsCacheEquivalent(kind, flags, child1, child2))
                {
                    return e.node;
                }
            }
            else
            {
                hash = -1;
            }

            return null;
        }

        internal static GreenNode? TryGetNode(int kind, GreenNode? child1, GreenNode? child2, GreenNode? child3, out int hash)
        {
            return TryGetNode(kind, child1, child2, child3, GetDefaultNodeFlags(), out hash);
        }

        internal static GreenNode? TryGetNode(int kind, GreenNode? child1, GreenNode? child2, GreenNode? child3, GreenNode.NodeFlags flags, out int hash)
        {
            if (CanBeCached(child1, child2, child3))
            {
                int h = hash = GetCacheHash(kind, flags, child1, child2, child3);
                int idx = h & CacheMask;
                var e = s_cache[idx];
                if (e.hash == h && e.node != null && e.node.IsCacheEquivalent(kind, flags, child1, child2, child3))
                {
                    return e.node;
                }
            }
            else
            {
                hash = -1;
            }

            return null;
        }

        public static GreenNode.NodeFlags GetDefaultNodeFlags()
        {
            return GreenNode.NodeFlags.IsNotMissing;
        }

        private static int GetCacheHash(int kind, GreenNode.NodeFlags flags, GreenNode? child1)
        {
            int code = (int)(flags) ^ kind;
            code = Hash.Combine(System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(child1), code);

            // ensure nonnegative hash
            return code & Int32.MaxValue;
        }

        private static int GetCacheHash(int kind, GreenNode.NodeFlags flags, GreenNode? child1, GreenNode? child2)
        {
            int code = (int)(flags) ^ kind;

            if (child1 != null)
            {
                code = Hash.Combine(System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(child1), code);
            }
            if (child2 != null)
            {
                code = Hash.Combine(System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(child2), code);
            }

            // ensure nonnegative hash
            return code & Int32.MaxValue;
        }

        private static int GetCacheHash(int kind, GreenNode.NodeFlags flags, GreenNode? child1, GreenNode? child2, GreenNode? child3)
        {
            int code = (int)(flags) ^ kind;

            if (child1 != null)
            {
                code = Hash.Combine(System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(child1), code);
            }
            if (child2 != null)
            {
                code = Hash.Combine(System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(child2), code);
            }
            if (child3 != null)
            {
                code = Hash.Combine(System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(child3), code);
            }

            // ensure nonnegative hash
            return code & Int32.MaxValue;
        }
    }
}
