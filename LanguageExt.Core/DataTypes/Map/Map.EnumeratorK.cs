﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace LanguageExt
{
    class MapKeyEnumerator<K, V> : IEnumerator<K>
    {
        internal struct NewStack : New<MapItem<K, V>[]>
        {
            public MapItem<K, V>[] New() =>
                new MapItem<K, V>[32];
        }

        int stackDepth;
        MapItem<K, V>[] stack;
        MapItem<K, V> map;
        int left;
        bool rev;
        int start;

        public MapKeyEnumerator(MapItem<K, V> root, bool rev, int start)
        {
            this.rev = rev;
            this.start = start;
            map = root;
            stack = Pool<NewStack, MapItem<K, V>[]>.Pop();
            Reset();
        }

        private MapItem<K, V> NodeCurrent
        {
            get;
            set;
        }

        public K Current => NodeCurrent.KeyValue.Key;
        object IEnumerator.Current => NodeCurrent.KeyValue.Key;

        public void Dispose()
        {
            if (stack != null)
            {
                Pool<NewStack, MapItem<K, V>[]>.Push(stack);
                stack = null;
            }
        }

        private MapItem<K, V> Next(MapItem<K, V> node) =>
            rev ? node.Left : node.Right;

        private MapItem<K, V> Prev(MapItem<K, V> node) =>
            rev ? node.Right : node.Left;

        private void Push(MapItem<K, V> node)
        {
            while (!node.IsEmpty)
            {
                stack[stackDepth] = node;
                stackDepth++;
                node = Prev(node);
            }
        }

        public bool MoveNext()
        {
            if (left > 0 && stackDepth > 0)
            {
                stackDepth--;
                NodeCurrent = stack[stackDepth];
                Push(Next(NodeCurrent));
                left--;
                return true;
            }

            NodeCurrent = null;
            return false;
        }

        public void Reset()
        {
            var skip = rev ? map.Count - start - 1 : start;

            stackDepth = 0;
            NodeCurrent = map;
            left = map.Count;

            while (!NodeCurrent.IsEmpty && skip != Prev(NodeCurrent).Count)
            {
                if (skip < Prev(NodeCurrent).Count)
                {
                    stack[stackDepth] = NodeCurrent;
                    stackDepth++;
                    NodeCurrent = Prev(NodeCurrent);
                }
                else
                {
                    skip -= Prev(NodeCurrent).Count + 1;
                    NodeCurrent = Next(NodeCurrent);
                }
            }

            if (!NodeCurrent.IsEmpty)
            {
                stack[stackDepth] = NodeCurrent;
                stackDepth++;
            }
        }
    }
}
