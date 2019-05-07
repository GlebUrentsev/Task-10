using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace CountingApp
{
    class Program
    {
        static void printG(string s)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(s);
            Console.ResetColor();
        }

        static void Main(string[] args)
        {
            while (true)
            {
                string nStr;
                int n;
                string mStr;
                int m;
                DoublyLinkedList<int> linkedList = new DoublyLinkedList<int>();

                printG("Введите число игроков n=");
                nStr = Console.ReadLine();

                Console.Write("Введите номер выбывающего игрока m=");
                mStr = Console.ReadLine();

                if (int.TryParse(nStr, out n) && int.TryParse(mStr, out m))
                {
                    // добавление элементов
                    // позиция добавляемого игрока в списке, число игроков в списке
                    linkedList.AddAllPlayers(1, n);

                    ListElement<int> curEl = linkedList.Head;
                    //удаляем все, кроме одного
                    while (linkedList.Count > 1)
                    {
                        ListElement<int> lastFoundEl = linkedList.RemoveWithPosition(curEl, 1, m);
                        Console.WriteLine("Игрок номер " + lastFoundEl.Data + " выбыл.");
                        curEl = lastFoundEl.Next;
                    }
                    Console.WriteLine("Остался игрок с номером " + curEl.Next.Data);
                }
                else
                {
                    Console.WriteLine("Введено не число!");
                }
            }            
        }
    }
    public class ListElement<Int>
    {
        public ListElement(int data)
        {
            Data = data;
        }
        public int Data { get; set; }
        public ListElement<int> Previous { get; set; }
        public ListElement<int> Next { get; set; }
    }

    public class DoublyLinkedList<Int> : IEnumerable<int>  // двусвязный список
    {
        ListElement<int> head; // головной/первый элемент
        ListElement<int> tail; // последний/хвостовой элемент
        int count;  // количество элементов в списке

        public ListElement<int> Head { get { return head; } }
        public int Count { get { return count; } }
        
        public void AddAllPlayers(int currElNumber, int maxCount)
        {
            if (currElNumber == maxCount + 1) return;
            else
            {
                ListElement<int> node = new ListElement<int>(currElNumber);
                node.Data = currElNumber;

                if (head == null)
                    head = node;
                else
                {
                    tail.Next = node;
                    node.Previous = tail;
                }
                tail = node;
                
                //зациклим список в кольцо
                if (currElNumber == maxCount)
                {
                    tail.Next = head;
                    head.Previous = tail;
                }
                currElNumber++;
                count++;

                AddAllPlayers(currElNumber, maxCount);
            }
        }

        public ListElement<int> RemoveWithPosition(ListElement<int> currEl, int currPosition, int positionM)
        {
            //условие выхода из рекурсии - найдена позиция
            //найден элемент с искомой позицией для удаления
            if (currPosition == positionM)
            {
                currEl.Next.Previous = currEl.Previous;
                currEl.Previous.Next = currEl.Next;

                count--;
                return currEl;
            }   
            else
            {
                currPosition++;
                return RemoveWithPosition(currEl.Next, currPosition, positionM);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)this).GetEnumerator();
        }

        IEnumerator<int> IEnumerable<int>.GetEnumerator()
        {
            ListElement<int> current = head;
            while (current != null)
            {
                yield return current.Data;
                current = current.Next;
            }
        }
    }
}
