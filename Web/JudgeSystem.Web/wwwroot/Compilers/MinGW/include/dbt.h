/*
 * dbt.h
 *
 * Device management API manifest constants and type definitions.
 *
 * $Id: dbt.h,v 0bd67cc9bc86 2017/03/20 20:01:38 keithmarshall $
 *
 * Written by Anders Norlander <anorland@hem2.passagen.se>
 * Copyright (C) 1998, 1999, 2002-2004, 2016, MinGW.org Project
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
#ifndef _DBT_H
#pragma GCC system_header

#ifndef __WINUSER_H_SOURCED__
/* Part of <dbt.h> is made available for selective inclusion by <winuser.h>;
 * define the <dbt.h> multiple inclusion guard only when NOT included as an
 * adjunct to <winuser.h>
 */
#define _DBT_H

/* When including <dbt.h> in its own right, ensure that the standard set of
 * W32API utility macros and data types is defined.
 */
#include <windef.h>

_BEGIN_C_DECLS

#define DBT_NO_DISK_SPACE				 0x47
#define DBT_CONFIGMGPRIVATE			       0x7FFF
#define DBT_DEVICEARRIVAL			       0x8000
#define DBT_DEVICEQUERYREMOVE			       0x8001
#define DBT_DEVICEQUERYREMOVEFAILED		       0x8002
#define DBT_DEVICEREMOVEPENDING 		       0x8003
#define DBT_DEVICEREMOVECOMPLETE		       0x8004
#define DBT_DEVICETYPESPECIFIC			       0x8005
#define DBT_DEVTYP_OEM					    0
#define DBT_DEVTYP_DEVNODE				    1
#define DBT_DEVTYP_VOLUME				    2
#define DBT_DEVTYP_PORT 				    3
#define DBT_DEVTYP_NET					    4

#if _WIN32_WINDOWS >= _WIN32_WINDOWS_98 || _WIN32_WINNT >= _WIN32_WINNT_WIN2K
#define DBT_DEVTYP_DEVICEINTERFACE			    5
#define DBT_DEVTYP_HANDLE				    6
#endif	/* >= _WIN32_WINDOWS_98 || >= _WIN32_WINNT_WIN2K */

#define DBT_APPYBEGIN				 	    0
#define DBT_APPYEND				 	    1
#define DBT_DEVNODES_CHANGED				    7
#define DBT_QUERYCHANGECONFIG				 0x17
#define DBT_CONFIGCHANGED				 0x18
#define DBT_CONFIGCHANGECANCELED			 0x19
#define DBT_MONITORCHANGE				 0x1B
#define DBT_SHELLLOGGEDON				   32
#define DBT_CONFIGMGAPI32				   34
#define DBT_VXDINITCOMPLETE				   35
#define DBT_VOLLOCKQUERYLOCK			       0x8041
#define DBT_VOLLOCKLOCKTAKEN			       0x8042
#define DBT_VOLLOCKLOCKFAILED			       0x8043
#define DBT_VOLLOCKQUERYUNLOCK			       0x8044
#define DBT_VOLLOCKLOCKRELEASED 		       0x8045
#define DBT_VOLLOCKUNLOCKFAILED 		       0x8046
#define DBT_USERDEFINED 			       0xFFFF
#define DBTF_MEDIA					    1
#define DBTF_NET					    2

#endif	/* !__WINUSER_H_SOURCED__ */
/* The following definitions are shared with <winuser.h>; thus, we
 * ALWAYS define them, whether within the __WINUSER_H_SOURCED__ scope,
 * or regular inclusion of <dbt.h>
 */
#define BSM_ALLCOMPONENTS				    0
#define BSM_APPLICATIONS				    8
#define BSM_ALLDESKTOPS 				   16
#define BSM_INSTALLABLEDRIVERS				    4
#define BSM_NETDRIVER					    2
#define BSM_VXDS					    1
#define BSF_FLUSHDISK				   0x00000004
#define BSF_FORCEIFHUNG 			   0x00000020
#define BSF_IGNORECURRENTTASK			   0x00000002
#define BSF_NOHANG				   0x00000008
#define BSF_NOTIMEOUTIFNOTHUNG			   0x00000040
#define BSF_POSTMESSAGE 			   0x00000010
#define BSF_QUERY				   0x00000001

#if _WIN32_WINNT >= _WIN32_WINNT_WIN2K
#define BSF_ALLOWSFW				   0x00000080
#define BSF_SENDNOTIFYMESSAGE			   0x00000100

#if _WIN32_WINNT >= _WIN32_WINNT_WINXP
#define BSF_LUID				   0x00000400
#define BSF_RETURNHDESK 			   0x00000200

#endif	/* >= _WIN32_WINNT_WINXP */
#endif	/* >= _WIN32_WINNT_WIN2K */

#ifndef __WINUSER_H_SOURCED__
/* The remaining definitions are NOT shared with <winuser.h>, so are
 * NOT defined within the __WINUSER_H_SOURCED__ scope.
 */
#define BSF_MSGSRV32ISOK_BIT				   31
#define BSF_MSGSRV32ISOK			   0x80000000

typedef struct _DEV_BROADCAST_HDR
{ DWORD 			dbch_size;
  DWORD 			dbch_devicetype;
  DWORD 			dbch_reserved;
} DEV_BROADCAST_HDR, *PDEV_BROADCAST_HDR;

typedef struct _DEV_BROADCAST_OEM
{ DWORD 			dbco_size;
  DWORD 			dbco_devicetype;
  DWORD 			dbco_reserved;
  DWORD 			dbco_identifier;
  DWORD 			dbco_suppfunc;
} DEV_BROADCAST_OEM, *PDEV_BROADCAST_OEM;

typedef struct _DEV_BROADCAST_PORT_A
{ DWORD 			dbcp_size;
  DWORD 			dbcp_devicetype;
  DWORD 			dbcp_reserved;
  char				dbcp_name[1];
} DEV_BROADCAST_PORT_A, *PDEV_BROADCAST_PORT_A;

typedef struct _DEV_BROADCAST_PORT_W
{ DWORD 			dbcp_size;
  DWORD 			dbcp_devicetype;
  DWORD 			dbcp_reserved;
  wchar_t			dbcp_name[1];
} DEV_BROADCAST_PORT_W, *PDEV_BROADCAST_PORT_W;

/* Map generic type name references for UNICODE/non-UNICODE usage
 * of the preceding pair of structured data types.
 */
typedef __AW_ALIAS_EX__(DEV_BROADCAST_PORT);
typedef __AW_ALIAS_EX__(PDEV_BROADCAST_PORT);

typedef struct _DEV_BROADCAST_USERDEFINED
{ struct _DEV_BROADCAST_HDR	dbud_dbh;
  char				dbud_szName[1];
} DEV_BROADCAST_USERDEFINED;

typedef struct _DEV_BROADCAST_VOLUME
{ DWORD 			dbcv_size;
  DWORD 			dbcv_devicetype;
  DWORD 			dbcv_reserved;
  DWORD 			dbcv_unitmask;
  WORD				dbcv_flags;
} DEV_BROADCAST_VOLUME, *PDEV_BROADCAST_VOLUME;

#if _WIN32_WINDOWS >= _WIN32_WINDOWS_98 || _WIN32_WINNT >= _WIN32_WINNT_WIN2K

typedef struct _DEV_BROADCAST_DEVICEINTERFACE_A
{ DWORD 			dbcc_size;
  DWORD 			dbcc_devicetype;
  DWORD 			dbcc_reserved;
  GUID				dbcc_classguid;
  char				dbcc_name[1];
} DEV_BROADCAST_DEVICEINTERFACE_A, *PDEV_BROADCAST_DEVICEINTERFACE_A;

typedef struct _DEV_BROADCAST_DEVICEINTERFACE_W
{ DWORD 			dbcc_size;
  DWORD 			dbcc_devicetype;
  DWORD 			dbcc_reserved;
  GUID				dbcc_classguid;
  wchar_t			dbcc_name[1];
} DEV_BROADCAST_DEVICEINTERFACE_W, *PDEV_BROADCAST_DEVICEINTERFACE_W;

/* Map generic type name references for UNICODE/non-UNICODE usage
 * of the preceding pair of structured data types.
 */
typedef __AW_ALIAS_EX__(DEV_BROADCAST_DEVICEINTERFACE);
typedef __AW_ALIAS_EX__(PDEV_BROADCAST_DEVICEINTERFACE);

typedef struct _DEV_BROADCAST_HANDLE
{ DWORD 			dbch_size;
  DWORD 			dbch_devicetype;
  DWORD 			dbch_reserved;
  HANDLE			dbch_handle;
  DWORD 			dbch_hdevnotify;
  GUID				dbch_eventguid;
  LONG				dbch_nameoffset;
  BYTE				dbch_data[1];
} DEV_BROADCAST_HANDLE, *PDEV_BROADCAST_HANDLE;
#endif	/* >= _WIN32_WINDOWS_98 || >= _WIN32_WINNT_WIN2K */

_END_C_DECLS

#endif	/* !__WINUSER_H_SOURCED__ */
#endif	/* !_DBT_H: $RCSfile: dbt.h,v $: end of file */
