﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using System.Dynamic;
using Microsoft.CSharp.RuntimeBinder;

internal class PrivateDynamicObjectSpec
{
	[Fact]
	public void WhenAccessingPrivateField_ThenSucceeds()
	{
		dynamic target = new PrivateDynamicObject(new PrivateObject());

		target.field = 5;

		Assert.Equal(5, target.field);
	}

	[Fact]
	public void WhenAccessingPrivateProperty_ThenSucceeds()
	{
		dynamic target = new PrivateDynamicObject(new PrivateObject());

		target.Property = "hello";

		Assert.Equal("hello", target.Property);
	}

	[Fact]
	public void WhenInvokingMethod_ThenSucceeds()
	{
		dynamic target = new PrivateDynamicObject(new PrivateObject());

		var result = target.Echo("hello");

		Assert.Equal("hello", result);
	}

	[Fact]
	public void WhenInvokingMethod2_ThenSucceeds()
	{
		dynamic target = new PrivateDynamicObject(new PrivateObject());

		var result = target.Echo("hello {0}", "world");

		Assert.Equal("hello world", result);
	}

	[Fact]
	public void WhenInvokingMethod_ThenResolvesOverload()
	{
		dynamic target = new PrivateDynamicObject(new PrivateObject());

		var result = target.Echo("hello", 2);

		Assert.Equal("hellohello", result);
	}

	[Fact]
	public void WhenInvokingMethodWithRef_ThenResolvesOverload()
	{
		dynamic target = new PrivateDynamicObject(new PrivateObject());
		var value = default(string);

		var result = target.Echo("hello ", ref value);

		Assert.True(result);
	}

	[Fact(Skip = "Ref/Out arguments are not supported by C# 4.0 dynamic. See Connect bug http://connect.microsoft.com/VisualStudio/feedback/details/543101/net-4-0s-dynamicobject-doesn-t-set-ref-out-arguments")]
	public void WhenInvokingMethodWithRef_ThenReturnsRefValue()
	{
		dynamic target = new PrivateDynamicObject(new PrivateObject());
		var value = default(string);

		var result = target.Echo("hello ", ref value);

		Assert.True(result);
		Assert.Equal("hello world", value);
	}

	[Fact]
	public void WhenInvokingIndexerOverload1_ThenSucceeds()
	{
		dynamic target = new PrivateDynamicObject(new PrivateObject());

		var result = target[9];

		Assert.Equal("9", result);
	}

	[Fact]
	public void WhenInvokingIndexerOverload2_ThenSucceeds()
	{
		dynamic target = new PrivateDynamicObject(new PrivateObject());

		var result = target["9"];

		Assert.Equal(9, result);
	}

	[Fact]
	public void WhenSettingIndexedProperty_ThenSucceeds()
	{
		dynamic target = new PrivateDynamicObject(new PrivateObject());

		target[9] = "kzu";
	}

	[Fact]
	public void WhenSettingNonExistingIndexedProperty_ThenThrows()
	{
		dynamic target = new PrivateDynamicObject(new PrivateObject());

		Assert.Throws<RuntimeBinderException>(() => target[Guid.NewGuid()] = 23);
	}

	[Fact]
	public void WhenInvokingIndexerTwoArgs_ThenSucceeds()
	{
		dynamic target = new PrivateDynamicObject(new PrivateObject());

		var result = target["hello", 2];

		Assert.Equal("llo", result);
	}

	[Fact]
	public void WhenNullObject_ThenAsPrivateDinamicReturnsNull()
	{
		var target = default(object);

		Assert.Null(target.AsPrivateDynamic());
	}

	[Fact]
	public void WhenInvokingExplicitlyImplementedMethod_ThenSucceeds()
	{
		dynamic target = new PrivateDynamicObject(new PrivateObject());

		var clone = target.Clone();

		Assert.Equal(target.Id, clone.Id);
	}

	[Fact]
	public void WhenInvokingExplicitlyImplementedProperty_ThenSucceeds()
	{
		dynamic target = new PrivateDynamicObject(new PrivateObject());

		target.Name = "foo";

		Assert.Equal("foo", target.Name);
	}

	[Fact]
	public void WhenInvokingNonExistingMethod_ThenFails()
	{
		dynamic target = new PrivateDynamicObject(new PrivateObject());

		Assert.Throws<RuntimeBinderException>(() => target.Do());
	}

	[Fact]
	public void WhenGettingNonExistingProperty_ThenFails()
	{
		dynamic target = new PrivateDynamicObject(new PrivateObject());

		Assert.Throws<RuntimeBinderException>(() => target.Blah);
	}

	[Fact]
	public void WhenGettingNonExistingIndex_ThenFails()
	{
		dynamic target = new PrivateDynamicObject(new PrivateObject());

		Assert.Throws<RuntimeBinderException>(() => target[true, 24]);
	}

	[Fact]
	public void WhenSettingNonExistingProperty_ThenFails()
	{
		dynamic target = new PrivateDynamicObject(new PrivateObject());

		Assert.Throws<RuntimeBinderException>(() => target.Blah = true);
	}

	[Fact]
	public void WhenConverting_ThenConvertsTargetObject()
	{
		var inner = new PrivateObject();
		dynamic target = new PrivateDynamicObject(inner);

		PrivateObject obj = target;

		Assert.Same(inner, obj);
	}

	[Fact]
	public void WhenConvertingToImplementedInterface_ThenConvertsTargetObject()
	{
		var inner = new PrivateObject();
		dynamic target = new PrivateDynamicObject(inner);

		ICloneable obj = target;

		Assert.Same(inner, obj);
	}

	[Fact]
	public void WhenInvokingGenericMethod_ThenSucceeds()
	{
		dynamic target = new PrivateDynamicObject(new PrivateObject());

		var value = target.Get<ICloneable>(23);
	}

	[Fact]
	public void WhenInvokingConstructorSecondTime_ThenChangesId()
	{
		var inner = new PrivateObject();
		var id = inner.Id;
		dynamic target = new PrivateDynamicObject(inner);

		target.ctor();

		Assert.NotEqual(id, inner.Id);
	}

	[Fact]
	public void WhenInvokingStaticMembers_ThenSucceeds()
	{
		var target = (dynamic)new PrivateDynamicObject(typeof(PrivateObject));
		var value1 = target.StaticProp;
		target.cctor();

		var value2 = target.StaticProp;

		Assert.NotEqual(value1, value2);

		target.StaticField = "foo";

		Assert.Equal("foo", target.StaticField);

		var value = target.StaticMethod("hello");

		Assert.Equal("hello", value);

		var refvalue = default(string);
		value = target.StaticMethod("hello", ref refvalue);

		Assert.Equal("hello", value);
	}

	[Fact]
	public void WhenInvokingCtorForType_ThenSucceeds()
	{
		var target = (dynamic)new PrivateDynamicObject(typeof(PrivateObject));
		var id = Guid.NewGuid();
		var obj = target.ctor(id);

		Assert.Equal(id, obj.Id);
	}

	[Fact]
	public void WhenConvertingToIncompatible_ThenThrows()
	{
		var target = new PrivateObject().AsPrivateDynamic();

		int id = 0;

		Assert.Throws<RuntimeBinderException>(() => id = target);
	}

	[Fact]
	public void WhenConvertingToIConvertibleCompatibleBuiltInType_ThenSucceeds()
	{
		var target = new ConvertibleObject().AsPrivateDynamic();

		int id = target;

		Assert.Equal(25, id);
	}

	[Fact]
	public void WhenConvertingToIConvertibleCompatibleCustomType_ThenSucceeds()
	{
		var target = new ConvertibleObject().AsPrivateDynamic();

		PrivateObject converted = target;

		Assert.NotNull(converted);
	}

	[Fact]
	public void WhenConvertingToIConvertibleIncompatibleCustomType_ThenSucceeds()
	{
		var target = new ConvertibleObject().AsPrivateDynamic();

		ICloneable converted = null;

		Assert.Throws<RuntimeBinderException>(() => converted = target);
	}

	private class PrivateObject : ICloneable, IPrivate
	{
		static PrivateObject()
		{
			StaticProp = Guid.NewGuid().ToString();
		}

		private PrivateObject(Guid id)
		{
			this.Id = id;
		}

		public PrivateObject()
		{
			this.Id = Guid.NewGuid();
		}

		public static string StaticProp { get; set; }
		public static string StaticField;
		public static string StaticMethod(string value)
		{
			return value;
		}

		public static string StaticMethod(string value, ref string refstring)
		{
			return value;
		}

		public Guid Id { get; set; }

		private int field;

		private string Property { get; set; }

		private string Echo(string value)
		{
			return value;
		}

		private string Echo(string value, string format)
		{
			return string.Format(value, format);
		}

		private string Echo(string value, int count)
		{
			return Enumerable.Range(0, count)
				.Aggregate("", (s, i) => s += value);
		}

		private bool Echo(string value, ref string result)
		{
			result = value + "world";

			return true;
		}

		private string this[int index]
		{
			get { return index.ToString(); }
			set { }
		}

		private int this[string index]
		{
			get { return int.Parse(index); }
		}

		private string this[string value, int index]
		{
			get { return value.Substring(index); }
		}

		private T Get<T>(int id)
		{
			return default(T);
		}

		object ICloneable.Clone()
		{
			return this;
		}

		string IPrivate.Name { get; set; }
	}

	private class ConvertibleObject : IConvertible
	{
		public TypeCode GetTypeCode()
		{
			throw new NotImplementedException();
		}

		public bool ToBoolean(IFormatProvider provider)
		{
			throw new NotImplementedException();
		}

		public byte ToByte(IFormatProvider provider)
		{
			throw new NotImplementedException();
		}

		public char ToChar(IFormatProvider provider)
		{
			throw new NotImplementedException();
		}

		public DateTime ToDateTime(IFormatProvider provider)
		{
			throw new NotImplementedException();
		}

		public decimal ToDecimal(IFormatProvider provider)
		{
			throw new NotImplementedException();
		}

		public double ToDouble(IFormatProvider provider)
		{
			throw new NotImplementedException();
		}

		public short ToInt16(IFormatProvider provider)
		{
			throw new NotImplementedException();
		}

		public int ToInt32(IFormatProvider provider)
		{
			return 25;
		}

		public long ToInt64(IFormatProvider provider)
		{
			throw new NotImplementedException();
		}

		public sbyte ToSByte(IFormatProvider provider)
		{
			throw new NotImplementedException();
		}

		public float ToSingle(IFormatProvider provider)
		{
			throw new NotImplementedException();
		}

		public string ToString(IFormatProvider provider)
		{
			throw new NotImplementedException();
		}

		public object ToType(Type conversionType, IFormatProvider provider)
		{
			if (conversionType == typeof(PrivateObject))
				return new PrivateObject();

			throw new NotSupportedException();
		}

		public ushort ToUInt16(IFormatProvider provider)
		{
			throw new NotImplementedException();
		}

		public uint ToUInt32(IFormatProvider provider)
		{
			throw new NotImplementedException();
		}

		public ulong ToUInt64(IFormatProvider provider)
		{
			throw new NotImplementedException();
		}
	}

	public interface IPrivate
	{
		string Name { get; set; }
	}
}
