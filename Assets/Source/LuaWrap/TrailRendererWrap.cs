﻿using System;
using UnityEngine;
using LuaInterface;
using Object = UnityEngine.Object;

public class TrailRendererWrap
{
	public static LuaMethod[] regs = new LuaMethod[]
	{
		new LuaMethod("New", _CreateTrailRenderer),
		new LuaMethod("GetClassType", GetClassType),
		new LuaMethod("__eq", Lua_Eq),
	};

	static LuaField[] fields = new LuaField[]
	{
		new LuaField("time", get_time, set_time),
		new LuaField("startWidth", get_startWidth, set_startWidth),
		new LuaField("endWidth", get_endWidth, set_endWidth),
		new LuaField("autodestruct", get_autodestruct, set_autodestruct),
	};

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateTrailRenderer(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 0)
		{
			TrailRenderer obj = new TrailRenderer();
			LuaScriptMgr.Push(L, obj);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: TrailRenderer.New");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, typeof(TrailRenderer));
		return 1;
	}

	public static void Register(IntPtr L)
	{
		LuaScriptMgr.RegisterLib(L, "UnityEngine.TrailRenderer", typeof(TrailRenderer), regs, fields, typeof(UnityEngine.Renderer));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_time(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		TrailRenderer obj = (TrailRenderer)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name time");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index time on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.time);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_startWidth(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		TrailRenderer obj = (TrailRenderer)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name startWidth");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index startWidth on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.startWidth);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_endWidth(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		TrailRenderer obj = (TrailRenderer)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name endWidth");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index endWidth on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.endWidth);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_autodestruct(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		TrailRenderer obj = (TrailRenderer)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name autodestruct");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index autodestruct on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.autodestruct);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_time(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		TrailRenderer obj = (TrailRenderer)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name time");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index time on a nil value");
			}
		}

		obj.time = (float)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_startWidth(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		TrailRenderer obj = (TrailRenderer)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name startWidth");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index startWidth on a nil value");
			}
		}

		obj.startWidth = (float)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_endWidth(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		TrailRenderer obj = (TrailRenderer)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name endWidth");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index endWidth on a nil value");
			}
		}

		obj.endWidth = (float)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_autodestruct(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		TrailRenderer obj = (TrailRenderer)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name autodestruct");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index autodestruct on a nil value");
			}
		}

		obj.autodestruct = LuaScriptMgr.GetBoolean(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Lua_Eq(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		Object arg0 = LuaScriptMgr.GetVarObject(L, 1) as Object;
		Object arg1 = LuaScriptMgr.GetVarObject(L, 2) as Object;
		bool o = arg0 == arg1;
		LuaScriptMgr.Push(L, o);
		return 1;
	}
}

