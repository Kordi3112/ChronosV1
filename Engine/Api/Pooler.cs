using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Api
{
    /// <summary>
    /// Api for Object Poolers
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Pooler<T> : IEnumerator<T>, IEnumerable<T> where T : class, IPollerObject 
    {
        int position = -1;

        internal List<T> ObjectsList { get; set; }

        protected Queue<int> EmptyFields { get; set; }

        protected Queue<T> ToRemoveList { get; set; }
        protected Queue<T> ToAddList { get; set; }

        public int ActiveObjects => ObjectsList.Count - EmptyFields.Count;

        public int Count => ObjectsList.Count;

        public T Current => ObjectsList[position];

        object IEnumerator.Current => ObjectsList[position];

        public Pooler()
        {
            ObjectsList = new List<T>();
            EmptyFields = new Queue<int>();
            ToRemoveList = new Queue<T>();
            ToAddList = new Queue<T>();
        }

        public void Add(T newObject)
        {
            if (EmptyFields.Count == 0)
            {
                ObjectsList.Add(newObject);
                newObject.SetPollerId(ObjectsList.Count - 1);
            }
            else
            {
                int id = EmptyFields.Dequeue();

                ObjectsList[id] = newObject;

                newObject.SetPollerId(id);
            }

            newObject.Delete(false);

            AfterAddAction(newObject);
        }

        public void AddToList(T removedObject)
        {
            ToAddList.Enqueue(removedObject);
        }

        public void AddFromList()
        {
            while (ToAddList.Count > 0)
            {
                Add(ToAddList.Dequeue());
            }
        }

        protected virtual void AfterAddAction(T newObject)
        {

        }

        public void AddToRemoveList(T removedObject)
        {
            ToRemoveList.Enqueue(removedObject);
        }

        public void Remove(T removedObject)
        {
            BeforeRemoveAction(removedObject);

            int id = removedObject.GetPoolerId();
            //ObjectsList[id] = null;

            removedObject.Delete(true);

            EmptyFields.Enqueue(id);
        }

        public void RemoveFromList()
        {
            while (ToRemoveList.Count > 0)
            {
                Remove(ToRemoveList.Dequeue());
            }
        }

        protected virtual void BeforeRemoveAction(T removedObject)
        {
            
        }

        public void Clear()
        {
            ObjectsList.Clear();
            EmptyFields.Clear();
        }

        public T this[int index] => ObjectsList[index];

        public void ActionForAll(Action<T> action)
        {
            foreach (T obj in ObjectsList)
            {
                action(obj);
            }
        }

        public void ActionForeachActive(Action<T> action)
        {
            foreach(T obj in ObjectsList)
            {
                if (obj == null)
                    continue;
                if(!obj.IsDeleted())
                    action(obj);
            }
        }

        public void ActionForeachTwoActives(Action<T, T> action)
        {
            if (ActiveObjects > 1)
            {
                for (int i = 1; i < ObjectsList.Count; i++)
                {

                    if (ObjectsList[i] == null)
                        continue;

                    if (ObjectsList[i].IsDeleted())
                        continue;

                    for (int j = 0; j < i; j++)
                    {
                        if (ObjectsList[j] == null)
                            continue;

                        if (ObjectsList[j].IsDeleted())
                            continue;

                        action(ObjectsList[i], ObjectsList[j]);
                    }
                }

            }
        }

        public bool MoveNext()
        {
            position++;
            return (position < ObjectsList.Count);
        }

        public void Reset()
        {
            position = 0;
        }

        public void Dispose()
        {
            
        }

        public IEnumerator<T> GetEnumerator()
        {
            return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this;
        }
    }
}
