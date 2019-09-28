/*
 * winreg.h
 *
 * Windows Registry Management API Declarations
 *
 *
 * $Id: winreg.h,v 25e378a66e0b 2017/12/20 23:26:30 keith $
 *
 * Written by Anders Norlander <anorland@hem2.passagen.se>
 * Copyright (C) 2002, 2006, 2008, 2012, 2013, 2016, 2017, MinGW.org Project
 *
 *
 * Permission is hereby granted, free of charge, to any person obtaining a
 * copy of this software and associated documentation files (the "Software"),
 * to deal in the Software without restriction, including without limitation
 * the rights to use, copy, modify, merge, publish, distribute, sublicense,
 * and/or sell copies of the Software, and to permit persons to whom the
 * Software is furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice (including the next
 * paragraph) shall be included in all copies or substantial portions of the
 * Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
 * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
 * DEALINGS IN THE SOFTWARE.
 *
 */
#ifndef _WINREG_H
#pragma GCC system_header
#define _WINREG_H

#include <winbase.h>
#include <reason.h>

#ifndef WINADVAPI
#define WINADVAPI
#endif

_BEGIN_C_DECLS

#define HKEY_CLASSES_ROOT		((HKEY)(0x80000000))
#define HKEY_CURRENT_USER		((HKEY)(0x80000001))
#define HKEY_LOCAL_MACHINE		((HKEY)(0x80000002))
#define HKEY_USERS			((HKEY)(0x80000003))
#define HKEY_PERFORMANCE_DATA		((HKEY)(0x80000004))
#define HKEY_CURRENT_CONFIG		((HKEY)(0x80000005))
#define HKEY_DYN_DATA			((HKEY)(0x80000006))

#define REG_OPTION_VOLATILE 				 1
#define REG_OPTION_NON_VOLATILE 			 0

/* https://msdn.microsoft.com/en-us/library/aa965882%28v=vs.85%29.aspx */
#define REG_OPTION_BACKUP_RESTORE			 4

#define REG_CREATED_NEW_KEY				 1
#define REG_OPENED_EXISTING_KEY 			 2

#define REG_NONE					 0
#define REG_SZ						 1
#define REG_EXPAND_SZ					 2
#define REG_BINARY					 3
#define REG_DWORD_LITTLE_ENDIAN 			 4
#define REG_DWORD					 4
#define REG_DWORD_BIG_ENDIAN				 5
#define REG_LINK					 6
#define REG_MULTI_SZ					 7
#define REG_RESOURCE_LIST				 8
#define REG_FULL_RESOURCE_DESCRIPTOR			 9
#define REG_RESOURCE_REQUIREMENTS_LIST			10
#define REG_QWORD_LITTLE_ENDIAN 			11
#define REG_QWORD					11

#define REG_NOTIFY_CHANGE_NAME				 1
#define REG_NOTIFY_CHANGE_ATTRIBUTES			 2
#define REG_NOTIFY_CHANGE_LAST_SET			 4
#define REG_NOTIFY_CHANGE_SECURITY			 8

/* https://msdn.microsoft.com/en-us/library/ms724868%28v=vs.85%29.aspx */
#define RRF_RT_REG_NONE 			0x00000001
#define RRF_RT_REG_SZ				0x00000002
#define RRF_RT_REG_EXPAND_SZ			0x00000004
#define RRF_RT_REG_BINARY			0x00000008
#define RRF_RT_REG_DWORD			0x00000010
#define RRF_RT_DWORD				0x00000018
#define RRF_RT_REG_MULTI_SZ			0x00000020
#define RRF_RT_REG_QWORD			0x00000040
#define RRF_RT_QWORD				0x00000048
#define RRF_RT_ANY				0x0000FFFF
#define RRF_SUBKEY_WOW6464KEY			0x00010000
#define RRF_SUBKEY_WOW6432KEY			0x00020000
#define RRF_NOEXPAND				0x10000000
#define RRF_ZEROONFAILURE			0x20000000

/* https://msdn.microsoft.com/en-us/library/ms724919%28v=vs.85%29.aspx */
#define REG_STANDARD_FORMAT				 1
#define REG_LATEST_FORMAT				 2

#define REG_NO_COMPRESSION				 4

#ifndef RC_INVOKED

typedef ACCESS_MASK REGSAM;

typedef struct value_entA
{ LPSTR 	ve_valuename;
  DWORD 	ve_valuelen;
  DWORD 	ve_valueptr;
  DWORD 	ve_type;
} VALENTA, *PVALENTA;

typedef struct value_entW
{ LPWSTR	ve_valuename;
  DWORD 	ve_valuelen;
  DWORD 	ve_valueptr;
  DWORD 	ve_type;
} VALENTW, *PVALENTW;

typedef __AW_ALIAS__(VALENT), *PVALENT;

#define AbortSystemShutdown __AW_SUFFIXED__(AbortSystemShutdown)
WINADVAPI BOOL WINAPI AbortSystemShutdownA (LPCSTR);
WINADVAPI BOOL WINAPI AbortSystemShutdownW (LPCWSTR);

#define InitiateSystemShutdown __AW_SUFFIXED__(InitiateSystemShutdown)
WINADVAPI BOOL WINAPI InitiateSystemShutdownA (LPSTR, LPSTR, DWORD, BOOL, BOOL);
WINADVAPI BOOL WINAPI InitiateSystemShutdownW (LPWSTR, LPWSTR, DWORD, BOOL, BOOL);

WINADVAPI LONG WINAPI RegCloseKey (HKEY);

#define RegConnectRegistry __AW_SUFFIXED__(RegConnectRegistry)
WINADVAPI LONG WINAPI RegConnectRegistryA (LPCSTR, HKEY, PHKEY);
WINADVAPI LONG WINAPI RegConnectRegistryW (LPCWSTR, HKEY, PHKEY);

#define RegCreateKey __AW_SUFFIXED__(RegCreateKey)
WINADVAPI LONG WINAPI RegCreateKeyA (HKEY, LPCSTR, PHKEY);
WINADVAPI LONG WINAPI RegCreateKeyW (HKEY, LPCWSTR, PHKEY);

#define RegCreateKeyEx __AW_SUFFIXED__(RegCreateKeyEx)
WINADVAPI LONG WINAPI RegCreateKeyExA
  ( HKEY, LPCSTR, DWORD, LPSTR, DWORD, REGSAM, LPSECURITY_ATTRIBUTES,
    PHKEY, PDWORD
  );
WINADVAPI LONG WINAPI RegCreateKeyExW
  ( HKEY, LPCWSTR, DWORD, LPWSTR, DWORD, REGSAM, LPSECURITY_ATTRIBUTES,
    PHKEY, PDWORD
  );
#define RegDeleteKey __AW_SUFFIXED__(RegDeleteKey)
WINADVAPI LONG WINAPI RegDeleteKeyA (HKEY, LPCSTR);
WINADVAPI LONG WINAPI RegDeleteKeyW (HKEY, LPCWSTR);

#define RegDeleteValue __AW_SUFFIXED__(RegDeleteValue)
WINADVAPI LONG WINAPI RegDeleteValueA (HKEY, LPCSTR);
WINADVAPI LONG WINAPI RegDeleteValueW (HKEY, LPCWSTR);

/* https://msdn.microsoft.com/en-us/library/ms724854%28v=vs.85%29.aspx */
WINADVAPI LONG WINAPI RegDisablePredefinedCache (void);

#define RegEnumKey __AW_SUFFIXED__(RegEnumKey)
WINADVAPI LONG WINAPI RegEnumKeyA (HKEY, DWORD, LPSTR, DWORD);
WINADVAPI LONG WINAPI RegEnumKeyW (HKEY, DWORD, LPWSTR, DWORD);

#define RegEnumKeyEx __AW_SUFFIXED__(RegEnumKeyEx)
WINADVAPI LONG WINAPI RegEnumKeyExA
  ( HKEY, DWORD, LPSTR, PDWORD, PDWORD, LPSTR, PDWORD, PFILETIME
  );
WINADVAPI LONG WINAPI RegEnumKeyExW
  ( HKEY, DWORD, LPWSTR, PDWORD, PDWORD, LPWSTR, PDWORD, PFILETIME
  );
#define RegEnumValue __AW_SUFFIXED__(RegEnumValue)
WINADVAPI LONG WINAPI RegEnumValueA
  ( HKEY, DWORD, LPSTR, PDWORD, PDWORD, PDWORD, LPBYTE, PDWORD
  );
WINADVAPI LONG WINAPI RegEnumValueW
  ( HKEY, DWORD, LPWSTR, PDWORD, PDWORD, PDWORD, LPBYTE, PDWORD
  );
WINADVAPI LONG WINAPI RegFlushKey (HKEY);
WINADVAPI LONG WINAPI RegGetKeySecurity
  ( HKEY, SECURITY_INFORMATION, PSECURITY_DESCRIPTOR, PDWORD
  );
#define RegLoadKey __AW_SUFFIXED__(RegLoadKey)
WINADVAPI LONG WINAPI RegLoadKeyA (HKEY, LPCSTR, LPCSTR);
WINADVAPI LONG WINAPI RegLoadKeyW (HKEY, LPCWSTR, LPCWSTR);

WINADVAPI LONG WINAPI RegNotifyChangeKeyValue (HKEY, BOOL, DWORD, HANDLE, BOOL);

/* https://msdn.microsoft.com/en-us/library/ms724894%28v=vs.85%29.aspx */
WINADVAPI LONG WINAPI RegOpenCurrentUser (REGSAM, PHKEY);

#define RegOpenKey __AW_SUFFIXED__(RegOpenKey)
WINADVAPI LONG WINAPI RegOpenKeyA (HKEY, LPCSTR, PHKEY);
WINADVAPI LONG WINAPI RegOpenKeyW (HKEY, LPCWSTR, PHKEY);

#define RegOpenKeyEx __AW_SUFFIXED__(RegOpenKeyEx)
WINADVAPI LONG WINAPI RegOpenKeyExA (HKEY, LPCSTR, DWORD, REGSAM, PHKEY);
WINADVAPI LONG WINAPI RegOpenKeyExW (HKEY, LPCWSTR, DWORD, REGSAM, PHKEY);

/* https://msdn.microsoft.com/en-us/library/ms724899%28v=vs.85%29.aspx */
WINADVAPI LONG WINAPI RegOpenUserClassesRoot (HANDLE, DWORD, REGSAM, PHKEY);

/* https://msdn.microsoft.com/en-us/library/ms724901%28v=vs.85%29.aspx */
WINADVAPI LONG WINAPI RegOverridePredefKey (HKEY, HKEY);

#define RegQueryInfoKey __AW_SUFFIXED__(RegQueryInfoKey)
WINADVAPI LONG WINAPI RegQueryInfoKeyA
  ( HKEY, LPSTR, PDWORD, PDWORD, PDWORD, PDWORD, PDWORD, PDWORD, PDWORD,
    PDWORD, PDWORD, PFILETIME
  );
WINADVAPI LONG WINAPI RegQueryInfoKeyW
  ( HKEY, LPWSTR, PDWORD, PDWORD, PDWORD, PDWORD, PDWORD, PDWORD, PDWORD,
    PDWORD, PDWORD, PFILETIME
  );
#define RegQueryMultipleValues __AW_SUFFIXED__(RegQueryMultipleValues)
WINADVAPI LONG WINAPI RegQueryMultipleValuesA
  ( HKEY, PVALENTA, DWORD, LPSTR, LPDWORD
  );
WINADVAPI LONG WINAPI RegQueryMultipleValuesW
  ( HKEY, PVALENTW, DWORD, LPWSTR, LPDWORD
  );
#define RegQueryValue __AW_SUFFIXED__(RegQueryValue)
WINADVAPI LONG WINAPI RegQueryValueA (HKEY, LPCSTR, LPSTR, PLONG);
WINADVAPI LONG WINAPI RegQueryValueW (HKEY, LPCWSTR, LPWSTR, PLONG);

#define RegQueryValueEx __AW_SUFFIXED__(RegQueryValueEx)
WINADVAPI LONG WINAPI RegQueryValueExA
  ( HKEY, LPCSTR, LPDWORD, LPDWORD, LPBYTE, LPDWORD
  );
WINADVAPI LONG WINAPI RegQueryValueExW
  ( HKEY, LPCWSTR, LPDWORD, LPDWORD, LPBYTE, LPDWORD
  );
#define RegReplaceKey __AW_SUFFIXED__(RegReplaceKey)
WINADVAPI LONG WINAPI RegReplaceKeyA (HKEY, LPCSTR, LPCSTR, LPCSTR);
WINADVAPI LONG WINAPI RegReplaceKeyW (HKEY, LPCWSTR, LPCWSTR, LPCWSTR);

#define RegRestoreKey __AW_SUFFIXED__(RegRestoreKey)
WINADVAPI LONG WINAPI RegRestoreKeyA (HKEY, LPCSTR, DWORD);
WINADVAPI LONG WINAPI RegRestoreKeyW (HKEY, LPCWSTR, DWORD);

#define RegSaveKey __AW_SUFFIXED__(RegSaveKey)
WINADVAPI LONG WINAPI RegSaveKeyA (HKEY, LPCSTR, LPSECURITY_ATTRIBUTES);
WINADVAPI LONG WINAPI RegSaveKeyW (HKEY, LPCWSTR, LPSECURITY_ATTRIBUTES);

WINADVAPI LONG WINAPI RegSetKeySecurity
  ( HKEY, SECURITY_INFORMATION, PSECURITY_DESCRIPTOR
  );
#define RegSetValue __AW_SUFFIXED__(RegSetValue)
WINADVAPI LONG WINAPI RegSetValueA (HKEY, LPCSTR, DWORD, LPCSTR, DWORD);
WINADVAPI LONG WINAPI RegSetValueW (HKEY, LPCWSTR, DWORD, LPCWSTR, DWORD);

#define RegSetValueEx __AW_SUFFIXED__(RegSetValueEx)
WINADVAPI LONG WINAPI RegSetValueExA
  ( HKEY, LPCSTR, DWORD, DWORD, const BYTE *, DWORD
  );
WINADVAPI LONG WINAPI RegSetValueExW
  ( HKEY, LPCWSTR, DWORD, DWORD, const BYTE *, DWORD
  );
#define RegUnLoadKey __AW_SUFFIXED__(RegUnLoadKey)
WINADVAPI LONG WINAPI RegUnLoadKeyA (HKEY, LPCSTR);
WINADVAPI LONG WINAPI RegUnLoadKeyW (HKEY, LPCWSTR);

#if _WIN32_WINNT >= _WIN32_WINNT_WINXP
#define RegSaveKeyEx __AW_SUFFIXED__(RegSaveKeyEx)
/* https://msdn.microsoft.com/en-us/library/ms724919%28v=vs.85%29.aspx */
WINADVAPI LONG WINAPI RegSaveKeyExA (HKEY, LPCSTR, LPSECURITY_ATTRIBUTES, DWORD);
WINADVAPI LONG WINAPI RegSaveKeyExW (HKEY, LPCWSTR, LPSECURITY_ATTRIBUTES, DWORD);

#if _WIN32_WINNT >= _WIN32_WINNT_WS03
#define RegDeleteKeyEx __AW_SUFFIXED__(RegDeleteKeyEx)
WINADVAPI LONG WINAPI RegDeleteKeyExA (HKEY, LPCSTR, REGSAM, DWORD);
WINADVAPI LONG WINAPI RegDeleteKeyExW (HKEY, LPCWSTR, REGSAM, DWORD);

/* https://msdn.microsoft.com/en-us/library/ms724858%28v=vs.85%29.aspx */
WINADVAPI LONG WINAPI RegDisableReflectionKey (HKEY);

/* https://msdn.microsoft.com/en-us/library/ms724859%28v=vs.85%29.aspx */
WINADVAPI LONG WINAPI RegEnableReflectionKey (HKEY);

/* https://msdn.microsoft.com/en-us/library/ms724907%28v=vs.85%29.aspx */
WINADVAPI LONG WINAPI RegQueryReflectionKey (HKEY, BOOL *);

#if _WIN32_WINNT >= _WIN32_WINNT_VISTA
#define RegCopyTree __AW_SUFFIXED__(RegCopyTree)
/* https://msdn.microsoft.com/en-us/library/aa379768%28v=vs.85%29.aspx */
WINADVAPI LONG WINAPI RegCopyTreeA (HKEY, LPCSTR, HKEY);
WINADVAPI LONG WINAPI RegCopyTreeW (HKEY, LPCWSTR, HKEY);

#define RegCreateKeyTransacted __AW_SUFFIXED__(RegCreateKeyTransacted)
/* https://msdn.microsoft.com/en-us/library/aa965882%28v=vs.85%29.aspx */
WINADVAPI LONG WINAPI RegCreateKeyTransactedA
  ( HKEY, LPCSTR, DWORD, LPSTR, DWORD, REGSAM, const LPSECURITY_ATTRIBUTES,
    PHKEY, LPDWORD, HANDLE, PVOID
  );
WINADVAPI LONG WINAPI RegCreateKeyTransactedW
  ( HKEY, LPCWSTR, DWORD, LPWSTR, DWORD, REGSAM, const LPSECURITY_ATTRIBUTES,
    PHKEY, LPDWORD, HANDLE, PVOID
  );
#define RegDeleteKeyTransacted __AW_SUFFIXED__(RegDeleteKeyTransacted)
/* https://msdn.microsoft.com/en-us/library/aa965883%28v=vs.85%29.aspx */
WINADVAPI LONG WINAPI RegDeleteKeyTransactedA
  ( HKEY, LPCSTR, REGSAM, DWORD, HANDLE, PVOID
  );
WINADVAPI LONG WINAPI RegDeleteKeyTransactedW
  ( HKEY, LPCWSTR, REGSAM, DWORD, HANDLE, PVOID
  );
#define RegDeleteKeyValue __AW_SUFFIXED__(RegDeleteKeyValue)
/* https://msdn.microsoft.com/en-us/library/ms724848%28v=vs.85%29.aspx */
WINADVAPI LONG WINAPI RegDeleteKeyValueA (HKEY, LPCSTR, LPCSTR);
WINADVAPI LONG WINAPI RegDeleteKeyValueW (HKEY, LPCWSTR, LPCWSTR);

#define RegDeleteTree __AW_SUFFIXED__(RegDeleteTree)
/* https://msdn.microsoft.com/en-us/library/aa379776%28v=vs.85%29.aspx */
WINADVAPI LONG WINAPI RegDeleteTreeA (HKEY, LPCSTR);
WINADVAPI LONG WINAPI RegDeleteTreeW (HKEY, LPCWSTR);

/* https://msdn.microsoft.com/en-us/library/ms724855%28v=vs.85%29.aspx */
WINADVAPI LONG WINAPI RegDisablePredefinedCacheEx (void);

#define RegGetValue __AW_SUFFIXED__(RegGetValue)
/* https://msdn.microsoft.com/en-us/library/ms724868%28v=vs.85%29.aspx */
WINADVAPI LONG WINAPI RegGetValueA
  ( HKEY, LPCSTR, LPCSTR, DWORD, LPDWORD, PVOID, LPDWORD
  );
WINADVAPI LONG WINAPI RegGetValueW
  ( HKEY, LPCWSTR, LPCWSTR, DWORD, LPDWORD, PVOID, LPDWORD
  );
#define RegLoadMUIString __AW_SUFFIXED__(RegLoadMUIString)
/* https://msdn.microsoft.com/en-us/library/ms724890%28v=vs.85%29.aspx */
WINADVAPI LONG WINAPI RegLoadMUIStringA
  ( HKEY, LPCSTR, LPSTR, DWORD, LPDWORD, DWORD, LPCSTR
  );
WINADVAPI LONG WINAPI RegLoadMUIStringW
  ( HKEY, LPCWSTR, LPWSTR, DWORD, LPDWORD, DWORD, LPCWSTR
  );
#define RegOpenKeyTransacted __AW_SUFFIXED__(RegOpenKeyTransacted)
/* https://msdn.microsoft.com/en-us/library/aa965886%28v=vs.85%29.aspx */
WINADVAPI LONG WINAPI RegOpenKeyTransactedA
  ( HKEY, LPCSTR, DWORD, REGSAM, PHKEY, HANDLE, PVOID
  );
WINADVAPI LONG WINAPI RegOpenKeyTransactedW
  ( HKEY, LPCWSTR, DWORD, REGSAM, PHKEY, HANDLE, PVOID
  );
#define RegSetKeyValue __AW_SUFFIXED__(RegSetKeyValue)
/* https://msdn.microsoft.com/en-us/library/ms724921%28v=vs.85%29.aspx */
WINADVAPI LONG WINAPI RegSetKeyValueA
  ( HKEY, LPCSTR, LPCSTR, DWORD, LPCVOID, DWORD
  );
WINADVAPI LONG WINAPI RegSetKeyValueW
  ( HKEY, LPCWSTR, LPCWSTR, DWORD, LPCVOID, DWORD
  );
#endif	/* _WIN32_WINNT >= _WIN32_WINNT_VISTA */
#endif	/* _WIN32_WINNT >= _WIN32_WINNT_WS03 */
#endif	/* _WIN32_WINNT >= _WIN32_WINNT_WINXP */

#endif	/* ! RC_INVOKED */

_END_C_DECLS

#endif	/* !_WINREG_H: $RCSfile: winreg.h,v $: end of file */
