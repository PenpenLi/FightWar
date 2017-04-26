﻿using System;
using UnityEngine;
using LuaInterface;
using Object = UnityEngine.Object;

public class AnimatorOverrideControllerWrap
{
	public static LuaMethod[] regs = new LuaMethod[]
	{
		new LuaMethod("get_Item", get_Item),
		new LuaMethod("set_Item", set_Item),
		new LuaMethod("New", _CreateAnimatorOverrideController),
		new LuaMethod("GetClassType", GetClassType),
		new LuaMethod("__eq", Lua_Eq),
	};

	static LuaField[] fields = new LuaField[]
	{
		new LuaField("runtimeAnimatorController", get_runtimeAnimatorController, set_runtimeAnimatorController),
		new LuaField("clips", get_clips, set_clips),
	};

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateAnimatorOverrideController(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 0)
		{
			AnimatorOverrideController obj = new AnimatorOverrideController();
			LuaScriptMgr.Push(L, obj);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: AnimatorOverrideController.New");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, typeof(AnimatorOverrideController));
		return 1;
	}

	public static void Register(IntPtr L)
	{
		LuaScriptMgr.RegisterLib(L, "UnityEngine.AnimatorOverrideController", typeof(AnimatorOverrideController), regs, fields, typeof(UnityEngine.RuntimeAnimatorController));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_runtimeAnimatorController(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		AnimatorOverrideController obj = (AnimatorOverrideController)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name runtimeAnimatorController");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index runtimeAnimatorController on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.runtimeAnimatorController);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_clips(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		AnimatorOverrideController obj = (AnimatorOverrideController)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name clips");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index clips on a nil value");
			}
		}

		LuaScriptMgr.PushArray(L, obj.clips);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_runtimeAnimatorController(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		AnimatorOverrideController obj = (AnimatorOverrideController)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name runtimeAnimatorController");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index runtimeAnimatorController on a nil value");
			}
		}

		obj.runtimeAnimatorController = LuaScriptMgr.GetUnityObject<RuntimeAnimatorController>(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_clips(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		AnimatorOverrideController obj = (AnimatorOverrideController)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name clips");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index clips on a nil value");
			}
		}

		obj.clips = LuaScriptMgr.GetNetObject<AnimationClipPair[]>(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Item(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 2 && LuaScriptMgr.CheckTypes(L, 1, typeof(AnimatorOverrideController), typeof(AnimationClip)))
		{
			AnimatorOverrideController obj = LuaScriptMgr.GetUnityObject<AnimatorOverrideController>(L, 1);
			AnimationClip arg0 = LuaScriptMgr.GetUnityObject<AnimationClip>(L, 2);
			AnimationClip o = obj[arg0];
			LuaScriptMgr.Push(L, o);
			return 1;
		}
		else if (count == 2 && LuaScriptMgr.CheckTypes(L, 1, typeof(AnimatorOverrideController), typeof(string)))
		{
			AnimatorOverrideController obj = LuaScriptMgr.GetUnityObject<AnimatorOverrideController>(L, 1);
			string arg0 = LuaScriptMgr.GetString(L, 2);
			AnimationClip o = obj[arg0];
			LuaScriptMgr.Push(L, o);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: AnimatorOverrideController.get_Item");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_Item(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 3 && LuaScriptMgr.CheckTypes(L, 1, typeof(AnimatorOverrideController), typeof(AnimationClip), typeof(AnimationClip)))
		{
			AnimatorOverrideController obj = LuaScriptMgr.GetUnityObject<AnimatorOverrideController>(L, 1);
			AnimationClip arg0 = LuaScriptMgr.GetUnityObject<AnimationClip>(L, 2);
			AnimationClip arg1 = LuaScriptMgr.GetUnityObject<AnimationClip>(L, 3);
			obj[arg0] = arg1;
			return 0;
		}
		else if (count == 3 && LuaScriptMgr.CheckTypes(L, 1, typeof(AnimatorOverrideController), typeof(string), typeof(AnimationClip)))
		{
			AnimatorOverrideController obj = LuaScriptMgr.GetUnityObject<AnimatorOverrideController>(L, 1);
			string arg0 = LuaScriptMgr.GetString(L, 2);
			AnimationClip arg1 = LuaScriptMgr.GetUnityObject<AnimationClip>(L, 3);
			obj[arg0] = arg1;
			return 0;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: AnimatorOverrideController.set_Item");
		}

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

