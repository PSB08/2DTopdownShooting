using System;
using System.Collections.Generic;
using UnityEngine;

namespace Code.Scripts.Enemies.Astar
{
    public class PriorityQueue<T> where T : IComparable<T>
    {
        public List<T> heap = new List<T>();
        
        public int Count => heap.Count;

        public void Clear()
        {
            heap?.Clear();
        }

        public T Contains(T key)
        {
            int idx = heap.IndexOf(key);
            if(idx < 0) return default;
            return heap[idx];
        }

        public void Push(T data)
        {
            heap.Add(data); // 데이터 맨 끝에 새로운 데이터 삽입
            
            int now = heap.Count - 1; // 삽입한 데이터의 위치
            while (now > 0)
            {
                int next = (now - 1) / 2; // 부모 인덱스
                if(heap[now].CompareTo(heap[next]) < 0) // 비교 했는데 부모가 나보다 작다?
                {
                    break; // 제 위치 찾았으니 멈춘다.
                }
                
                // 그렇지 않으면 값 교환
                (heap[now], heap[next]) = (heap[next], heap[now]);
                now = next; // 현재 인덱스를 부모였던 것의 인덱스로 바꿔주고 계속 진행.
            }
        }

        public T Pop()
        {
            T ret = heap[0];
            int lastIdx = heap.Count - 1;
            heap[0] = heap[lastIdx]; // 마지막 값을 맨 꼭대기로
            heap.RemoveAt(lastIdx); // 마지막 제거
            lastIdx--;

            int now = 0;
            while (true)
            {
                int left = now * 2 + 1;
                int right = now * 2 + 2;

                int next = now;
                
                if (left <= lastIdx && heap[next].CompareTo(heap[left]) < 0) // 왼쪽 < 현재
                    next = left;
                if (right <= lastIdx && heap[next].CompareTo(heap[right]) < 0) // 오른쪽 < 현재 
                    next = right;

                if (next == now)
                    break;
                
                (heap[now], heap[next]) = (heap[next], heap[now]);
                now = next;
            }
            
            return ret;
        }

        public T Peek()
        {
            return heap.Count == 0 ? default : heap[0];
        }
        
    }
}