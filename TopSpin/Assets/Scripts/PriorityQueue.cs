using System;
using System.Collections.Generic;
using System.Linq;

public class PriorityQueue<T>
{
    private SortedDictionary<int, Queue<T>> _elements = new SortedDictionary<int, Queue<T>>();

    public int Count { get; private set; } = 0;

    public void Enqueue(T item, int priority)
    {
        if (!_elements.ContainsKey(priority))
        {
            _elements[priority] = new Queue<T>();
        }

        _elements[priority].Enqueue(item);
        Count++;
    }

    public T Dequeue()
    {
        if (Count == 0)
        {
            throw new InvalidOperationException("The queue is empty.");
        }

        // Obt�n la cola con la prioridad m�s baja
        var firstElement = _elements.First();
        var item = firstElement.Value.Dequeue();
        Count--;

        // Elimina la entrada si la cola est� vac�a
        if (firstElement.Value.Count == 0)
        {
            _elements.Remove(firstElement.Key);
        }

        return item;
    }
}
