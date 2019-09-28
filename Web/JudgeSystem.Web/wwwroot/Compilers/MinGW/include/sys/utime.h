/*
 * sys/utime.h
 *
 * Microsoft counterpart to the (now deprecated) POSIX <utime.h> header;
 * unlike POSIX, Microsoft provide this as <sys/utime.h>
 *
 *
 * $Id: utime.h,v b582ba152894 2018/02/22 20:00:40 keith $
 *
 * Written by Colin Peters <colin@bird.fu.is.saga-u.ac.jp>
 * Copyright (C) 1997-2001, 2003, 2004, 2007, 2018, MinGW.org Project
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
#ifndef _SYS_UTIME_H
#pragma GCC system_header
#define _SYS_UTIME_H

#include <_mingw.h>
#define __need_time_t  1
#include <sys/types.h>
#undef __need_time_t

#ifndef RC_INVOKED

#define __need_wchar_t
#define __need_size_t
#include <stddef.h>

_BEGIN_C_DECLS

struct _utimbuf
{ /* Microsoft's definition for their equivalent of the POSIX utimbuf
   * structure, as used in their _utime() function.
   */
  time_t	actime;		/* last file access time */
  time_t	modtime;	/* last modification time */
};

#ifndef _NO_OLDNAMES
/* Microsoft used to also support the formal POSIX structure definition,
 * but have discontinued this practice; for portability, NEVER disable
 * Microsoft's old names, (i.e. NEVER define _NO_OLDNAMES).
 */
struct utimbuf
{ /* This is the POSIX compatible definition; it must EXACTLY mimic the
   * Microsoft definition (as specified above).
   */
  time_t	actime;
  time_t	modtime;
};
#endif	/* !_NO_OLDNAMES */

struct __utimbuf32
{ /* A 32-bit-explicit equivalent for struct _utimbuf; technically,
   * Microsoft don't support this, before the release of Vista, other
   * than on non-free runtimes from MSVCR80.DLL onwards, but since it
   * is fundamentally identical to struct _utimbuf on those platforms
   * which do not explicitly support it, and WE provide emulation for
   * the associated 32-bit-explicit functions on those platforms, it
   * is convenient for us to define it unconditionally.
   */
  __time32_t	actime;
  __time32_t	modtime;
};

#if defined _WIN64 /* FIXME: is this true, for ALL Win64 versions? */  \
 || _WIN32_WINNT >= _WIN32_WINNT_VISTA || __MSVCRT_VERSION__ >= __MSVCR80_DLL
 /* The associated 32-bit-explicit functions are implemented in, and exported
  * from MSVCRT.DLL, from Vista onwards, and versions of non-free MSVCRxx.DLL
  * from MSVCR80.DLL onwards; (for any runtime version which does not satisfy
  * either one of these criteria, we provide emulations below).
  */
_CRTIMP __cdecl __MINGW_NOTHROW int _utime32( const char *, struct __utimbuf32 * );
_CRTIMP __cdecl __MINGW_NOTHROW int _wutime32( const wchar_t *, struct __utimbuf32 * );
_CRTIMP __cdecl __MINGW_NOTHROW int _futime32( int, struct __utimbuf32 * );
#endif	/* Win-Vista and later, or MSVCR80.DLL and later */

#if defined _WIN64 /* ALL Win64 versions should have this! */  \
 || _WIN32_WINNT >= _WIN32_WINNT_WIN2K || __MSVCRT_VERSION__ >= __MSVCR61_DLL
 /* Conversely, the 64-bit-explicit alternative functions have been supported
  * by MSVCRT.DLL, from Win2K onwards, and by non-free MSVCR61.DLL and later;
  * we declare them, for such platforms, but we make no provision to emulate
  * them on any earlier platform.
  */
struct __utimbuf64
{ /* The 64-bit-explicit equivalent of struct _utimbuf; it is necessary
   * to declare it, only when the functions which use it are declared...
   */
  __time64_t	actime;
  __time64_t	modtime;
};

/* ...as here.
 */
_CRTIMP __cdecl __MINGW_NOTHROW int _utime64( const char *, struct __utimbuf64 * );
_CRTIMP __cdecl __MINGW_NOTHROW int _wutime64( const wchar_t *, struct __utimbuf64 * );
_CRTIMP __cdecl __MINGW_NOTHROW int _futime64( int, struct __utimbuf64 * );
#endif	/* Win2K and later, or MSVCR61.DLL and later */

#if __MSVCRT_VERSION__ >= __MSVCR80_DLL
/* Non-free MSVCRxx.DLL variants, from MSVCR80.DLL onwards, do not export
 * utime(), _utime(), _wutime(), or _futime(); instead, they require in-line
 * emulation, under control of Microsoft's ill-conceived _USE_32BIT_TIME_T
 * feature test macro...
 */
# ifdef _USE_32BIT_TIME_T
/* ...which, when defined, causes these generic function names to be mapped
 * to their 32-bit-explicit variants, (which thus retain compatibility with
 * their actual implementations, as exported by MSVCRT.DLL)...
 */
__CRT_ALIAS __cdecl __MINGW_NOTHROW
int _utime( const char * __v1, struct _utimbuf * __v2 )
{ return _utime32( __v1, (struct __utimbuf32 *)(__v2) ); }

__CRT_ALIAS __cdecl __MINGW_NOTHROW
int _wutime( const wchar_t * __v1, struct _utimbuf * __v2)
{ return _wutime32( __v1, (struct __utimbuf32 *)(__v2) ); }

__CRT_ALIAS __cdecl __MINGW_NOTHROW
int _futime( int __v1, struct _utimbuf * __v2 )
{ return _futime32( __v1, (struct __utimbuf32 *)(__v2) ); }

# else
/* ...but, when NOT defined, they are mapped to the 64-bit-explicit
 * variants, (which are incompatible with the corresponding MSVCRT.DLL
 * implementations).
 */
__CRT_ALIAS __cdecl __MINGW_NOTHROW
int _utime( const char * __v1, struct _utimbuf * __v2 )
{ return _utime64( __v1, (struct __utimbuf64 *)(__v2) ); }

__CRT_ALIAS __cdecl __MINGW_NOTHROW
int _wutime( const wchar_t * __v1, struct _utimbuf * __v2 )
{ return _wutime64( __v1, (struct __utimbuf64 *)(__v2) ); }

__CRT_ALIAS __cdecl __MINGW_NOTHROW
int _futime( int __v1, struct _utimbuf * __v2 )
{ return _futime64( __v1, (struct __utimbuf64 *)(__v2) ); }

# endif	/* !_USE_32BIT_TIME_T */

# ifndef _NO_OLDNAMES
/* The POSIX compatible (Microsoft old name) function must also be
 * emulated; it maps to whatever in-line implementation has become
 * applicable for _utime()
 */
__CRT_ALIAS __cdecl __MINGW_NOTHROW
int utime( const char * __v1, struct _utimbuf * __v2 )
{ return _utime( __v1, (struct _utimbuf *)(__v2) ); }
# endif	/* !_NO_OLDNAMES */

#else	/* __MSVCRT_VERSION__ < __MSVCR80_DLL */
/* When using CRTDLL.DLL, MSVCRT.DLL, or non-free MSVCRxx.DLL prior to
 * MSVCR80.DLL, each of the preceding functions is physically exported.
 */
_CRTIMP __cdecl __MINGW_NOTHROW int _utime( const char *, struct _utimbuf * );
_CRTIMP __cdecl __MINGW_NOTHROW int _futime( int, struct _utimbuf * );

# ifdef __MSVCRT__
/* The wide character version is only available when linking with MSVCRT.DLL,
 * (or non-free MSVCRxx.DLL variants); this is not declared, when linking with
 * CRTDLL.DLL
 */
_CRTIMP __cdecl __MINGW_NOTHROW int _wutime( const wchar_t *, struct _utimbuf * );
# endif	/* MSVCRT.DLL only */

# if _WIN32_WINNT < _WIN32_WINNT_VISTA
/* However, there is no physical implementation of any 32-bit-explict
 * function variant, prior to Vista, so emulate them.
 *
 * FIXME: maybe only if _MINGW32_SOURCE_EXTENDED is specified?
 */
__CRT_ALIAS __cdecl __MINGW_NOTHROW
int _utime32( const char *__v1, struct __utimbuf32 *__v2 )
{ return _utime( __v1, (struct _utimbuf *)(__v2) ); }

__CRT_ALIAS __cdecl __MINGW_NOTHROW
int _futime32( int __v1, struct __utimbuf32 *__v2 )
{ return _futime( __v1, (struct _utimbuf *)(__v2) ); }

__CRT_ALIAS __cdecl __MINGW_NOTHROW
int _wutime32( const wchar_t *__v1, struct __utimbuf32 *__v2 )
{ return _wutime( __v1, (struct _utimbuf *)(__v2) ); }

# endif	/* _WIN32_WINNT < _WIN32_WINNT_VISTA */

# ifndef _NO_OLDNAMES
/* Strictly, the POSIX compatible utime() function is exported only by
 * MSVCRT.DLL from Vista onwards, but it APPEARS to be exported, by way
 * of a libmoldnames.a trampoline, for any MSVCRT.DLL which exports a
 * physical _utime() implementation.
 */
_CRTIMP __cdecl __MINGW_NOTHROW int utime( const char *, struct utimbuf * );

# endif	/* !_NO_OLDNAMES */
#endif	/* __MSVCRT_VERSION__ < __MSVCR80_DLL */

_END_C_DECLS

#endif	/* ! RC_INVOKED */
#endif	/* !_SYS_UTIME_H: $RCSfile: utime.h,v $: end of file */
