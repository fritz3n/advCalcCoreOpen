using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace advCalcCore.Values
{
	public class ListValue : Value, ICountable, IEnumerable<Value>
	{
		private List<Value> values;

		public int Count => values.Count;

		public ListValue(IEnumerable<Value> values)
		{
			this.values = values.ToList();
		}

		public ListValue()
		{
			values = new List<Value>();
		}

		public override CastingType GetCastTo(Type type)
		{
			if (type == typeof(Value))
				return CastingType.Implicit;
			if (type == typeof(ListValue))
				return CastingType.Implicit;

			if (type == typeof(TextValue))
				return CastingType.Explicit;

			return CastingType.None;
		}

		public override Value CastTo(Type type, bool explicitCast = false)
		{
			if (type == typeof(Value))
				return this;
			if (type == typeof(ListValue))
				return this;

			if (explicitCast)
			{
				if (type == typeof(TextValue))
					return new TextValue(ToString());
			}

			return base.CastTo(type, explicitCast);
		}

		public override Value this[int index]
		{
			get => values[index];
			set
			{
				if (index < -values.Count)
					throw new IndexOutOfRangeException();

				if (index < 0)
					index = values.Count + index;


				if (values.Count > index)
				{
					values[index] = value;
				}
				else
				{
					for (int i = 0; i < index - values.Count; i++)
						values.Add(NullValue.Null);
					values.Add(value);
				}
			}
		}

		public override Value ApplyOperator(Func<Value, Value, Value> operation, Value right)
		{
			if (right is ListValue rightList)
			{
				if (rightList.Count != Count)
					throw new NotSupportedException($"Can only do calculations with lists of equal lengths");

				IEnumerable<Value> Calculate()
				{
					for (int i = 0; i < Count; i++)
					{
						yield return operation(values[i], rightList[i]);
					}
				}

				return new ListValue(Calculate());
			}
			else
			{
				IEnumerable<Value> Calculate()
				{
					for (int i = 0; i < Count; i++)
					{
						yield return operation(values[i], right);
					}
				}

				return new ListValue(Calculate());
			}
		}

		public override Value ApplyOperator(Func<decimal, decimal, decimal> operation, Value right)
		{
			if (right is ListValue rightList)
			{
				if (rightList.Count != Count)
					throw new NotSupportedException($"Can only do calculations with lists of equal lengths");

				IEnumerable<Value> Calculate()
				{
					for (int i = 0; i < Count; i++)
					{
						yield return values[i].ApplyOperator(operation, rightList[i]);
					}
				}

				return new ListValue(Calculate());
			}
			else
			{
				IEnumerable<Value> Calculate()
				{
					for (int i = 0; i < Count; i++)
					{
						yield return values[i].ApplyOperator(operation, right);
					}
				}

				return new ListValue(Calculate());
			}
		}

		public override Value Add(Value right) => ApplyOperator((Value left, Value right) => left + right, right);
		public override Value Subtract(Value right) => ApplyOperator((Value left, Value right) => left - right, right);
		public override Value Divide(Value right) => ApplyOperator((Value left, Value right) => left / right, right);
		public override Value Multiply(Value right) => ApplyOperator((Value left, Value right) => left * right, right);
		public override Value Modulo(Value right) => ApplyOperator((Value left, Value right) => left % right, right);
		public override Value Pow(Value right) => ApplyOperator((Value left, Value right) => left ^ right, right);

		public override string ToString() => "[" + string.Join(", ", values) + "]";

		public IEnumerator<Value> GetEnumerator()
		{
			return ((IEnumerable<Value>)values).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable)values).GetEnumerator();
		}
	}
}
