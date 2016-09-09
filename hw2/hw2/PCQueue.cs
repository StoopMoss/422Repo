using System;

namespace CS422
{
	public class PCQueue
	{
		private Node _list;
		private Node _end;

		public PCQueue ()
		{
			_list = null;
		}

		public bool Dequeue(ref int out_value)
		{			
			if (_list == null) 
			{
				return false;
			}

			// There is a node
			out_value = _list.Value;
			_list = _list.Next;
			return true;
		}

		public void Enqueue(int dataValue)
		{			
			if (_list == null) 
			{				
				// no nodes yet
				_list = new Node (dataValue, null);
				//_end = _list;
			}else if (_end == null)
			{
				// theres one item
				_end = new Node (dataValue, null);
				_list.Next = _end;
			}
			else 
			{
				// nodes exsit
				_end.Next = new Node (dataValue, null);
				_end = _end.Next;
			}
		}
	}

	internal class Node
	{	
		private Node _next;
		private int _value;

		public int Value
		{
			get{ return _value; }
			set{ _value = value; }
		}

		public Node Next
		{
			get{ return _next; }
			set{ _next = value; }
		}

		public Node (int value, Node next)
		{	
			if (next != null) {
				_next = next;
			} else {
				_next = null;
			}
			_value = value;
		}

	}
}

