﻿/* Date: 20.12.2012, Time: 17:02 */
using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using IllidanS4.SharpUtils.Interop;
using IllidanS4.SharpUtils.Metadata;

namespace IllidanS4.SharpUtils.Accessing
{
	/// <summary>
	/// Basic interface for all accessors.
	/// </summary>
	public interface IStorageAccessor
	{
		Type Type{get;}
	}
	
	/// <summary>
	/// The interface for "get" accessors.
	/// </summary>
	[DefaultMember("Item")]
	public interface IReadAccessor : IStorageAccessor
	{
		object Item{get;}
	}
	
	/// <summary>
	/// The interface for "set" accessors.
	/// </summary>
	[DefaultMember("Item")]
	public interface IWriteAccessor : IStorageAccessor
	{
		object Item{set;}
	}
	
	[DefaultMember("Item")]
	public interface IReadWriteAccessor : IReadAccessor, IWriteAccessor, IStrongBox
	{
		new object Item{get;set;}
	}
	
	/// <summary>
	/// The interface for "get" accessors.
	/// </summary>
	[DefaultMember("Item")]
	public interface IReadAccessor<out T> : IReadAccessor
	{
		new T Item{get;}
	}
	
	/// <summary>
	/// The interface for "set" accessors.
	/// </summary>
	[DefaultMember("Item")]
	public interface IWriteAccessor<in T> : IWriteAccessor
	{
		new T Item{set;}
	}
	
	[DefaultMember("Item")]
	public interface IReadWriteAccessor<T> : IReadAccessor<T>, IWriteAccessor<T>, IReadWriteAccessor
	{
		new T Item{get;set;}
	}
	
	public interface ITypedReference : IStorageAccessor
	{
		TRet GetReference<TRet>(Func<SafeReference,TRet> func);
	}
	
	/// <summary>
	/// An accessor that provides an "out" reference.
	/// </summary>
	public interface IOutReference<T> : IWriteAccessor<T>, IStorageAccessor, ITypedReference
	{
		TRet GetReference<TRet>(Reference.OutFunc<T, TRet> func);
	}
	
	/// <summary>
	/// An accessor that provides a "ref" reference.
	/// </summary>
	public interface IRefReference<T> : IReadWriteAccessor<T>, IOutReference<T>
	{
		TRet GetReference<TRet>(Reference.RefFunc<T, TRet> func);
	}
	
	/// <summary>
	/// Basic generic accessor.
	/// </summary>
	public abstract class BasicAccessor<T> : MarshalByRefObject, IStorageAccessor
	{
		Type IStorageAccessor.Type{
			get{
				return typeof(T);
			}
		}
	}
	
	[DefaultMember("Item")]
	public abstract class BasicReadAccessor<T> : BasicAccessor<T>, IReadAccessor<T>
	{
		public abstract T Item{get;}
		object IReadAccessor.Item{
			get{
				return this.Item;
			}
		}
	}
	
	[DefaultMember("Item")]
	public abstract class BasicWriteAccessor<T> : BasicAccessor<T>, IWriteAccessor<T>
	{
		public abstract T Item{set;}
		object IWriteAccessor.Item{
			set{
				Item = (T)value;
			}
		}
	}
	
	[DefaultMember("Item")]
	public abstract class BasicReadWriteAccessor<T> : BasicAccessor<T>, IReadWriteAccessor<T>, IStrongBox
	{
		public abstract T Item{get; set;}
		object IReadAccessor.Item{
			get{
				return this.Item;
			}
		}
		object IWriteAccessor.Item{
			set{
				Item = (T)value;
			}
		}
		object IReadWriteAccessor.Item{
			get{
				return this.Item;
			}
			set{
				Item = (T)value;
			}
		}
		
		object IStrongBox.Value{
			get{
				return this.Item;
			}
			set{
				Item = (T)value;
			}
		}
	}
}