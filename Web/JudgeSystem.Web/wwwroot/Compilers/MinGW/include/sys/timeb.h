/*
 * sys/timeb.h
 *
 * Support for the UNIX System V ftime() system call, and its various
 * Microsoft counterparts.
 *
 *
 * $Id: timeb.h,v b0fbd2c8eb16 2018/02/23 19:51:52 keith $
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
#ifndef _SYS_TIMEB_H
#pragma GCC system_header
#define _SYS_TIMEB_H

#include <_mingw.h>
#define __need_time_t  1
#include <sys/types.h>
#undef __need_time_t

#ifndef RC_INVOKED

/* Tests conducted on 32-bit Win7 indicate that all timeb structs,
 * on Win32 hosts, are packed with 2-byte alignment.
 *
 * FIXME: Is this also true for Win64 hosts?
 */
#pragma pack( push, 2 )

#ifndef _NO_OLDNAMES
/* This is the standard POSIX/Unix System V API declaration; this
 * API was declared as deprecated in POSIX.1-2001, and subsequently
 * deleted from POSIX.1-2008 onwards.
 */
struct __POSIX_2001_DEPRECATED timeb
{ time_t	time;
  short 	millitm;
  short 	timezone;
  short 	dstflag;
};
#endif
/* Microsoft's equivalent of the above uses their typically ugly
 * underscore decorated name; in common with the POSIX variant, it
 * is susceptible to bugs resulting from time_t ambiguity, which
 * is endemic within non-free MSVCR80.DLL and later.
 */
struct _timeb
{ time_t	time;
  short 	millitm;
  short 	timezone;
  short 	dstflag;
};

/* To avoid time_t ambiguity, the following is equivalent to struct
 * _timeb, with explicitly 32-bit time_t; strictly, Microsoft didn't
 * support this, on 32-bit Windows, until non-free MSVCR80.DLL, or
 * universally from Win-Vista onwards; we make it unconditionally
 * available, and back-port its associated _ftime32() function
 * for use on legacy Win32 hosts.
 */
struct __timeb32
{ __time32_t	time;
  short 	millitm;
  short 	timezone;
  short 	dstflag;
};

#pragma pack( pop )

_BEGIN_C_DECLS

#if defined _WIN64	/* Always supported on Win64; otherwise... */  \
 || __MSVCRT_VERSION__ >= __MSVCR61_DLL || _WIN32_WINNT >= _WIN32_WINNT_WIN2K
/* The _ftime64() function, and its associated struct __timeb64 data type, were
 * first introduced in the non-free MSVCR61.DLL runtime, and subsequently, they
 * were retrofitted to MSVCRT.DLL from Win2K onwards.
 */
#pragma pack( push, 2 )

struct __timeb64
{ __time64_t	time;
  short 	millitm;
  short 	timezone;
  short 	dstflag;
};

#pragma pack( pop )

_CRTIMP __cdecl __MINGW_NOTHROW  void _ftime64 (struct __timeb64 *);

#endif	/* _WIN64 || __MSVCR61_DLL || _WIN32_WINNT_WIN2K and later */

#if __MSVCRT_VERSION__ < __MSVCR80_DLL
/* Non-free MSVCR80.DLL, and later, don't povide _ftime() directly;
 * (it must be emulated); when NOT using any of these non-free DLLs,
 * we may import it from MSVCRT.DLL
 */
_CRTIMP __cdecl __MINGW_NOTHROW  void _ftime (struct _timeb *);
#endif	/* !__MSVCR80_DLL or later */

#if defined _WIN64	/* Also always supported on Win64, but... */  \
 || __MSVCRT_VERSION__ >= __MSVCR80_DLL || _WIN32_WINNT >= _WIN32_WINNT_VISTA
/* Conversely, the complementary _ftime32() function, and its associated struct
 * __timeb32 data type, were not introduced until non-free MSVCR80.DLL runtime,
 * nor retrofitted to MSVCRT.DLL until Win-Vista onwards.
 */
_CRTIMP __cdecl __MINGW_NOTHROW	 void _ftime32 (struct __timeb32 *);

#else	/* !(_WIN64 || __MSVCR80_DLL || _WIN32_WINNT_VISTA and later) */
/* This is a legacy _WIN32 system, pre-Win-Vista, and NOT using MSVCR80.DLL,
 * or later; it is convenient for us to emulate _ftime32(), by the expedient
 * of mapping it to the legacy 32-bit _ftime() implementation.
 */
__CRT_ALIAS __cdecl __MINGW_NOTHROW
void _ftime32 (struct __timeb32 *__t) {_ftime ((struct _timeb *)(__t));}

#endif	/* !(_WIN64 || __MSVCR80_DLL || _WIN32_WINNT_VISTA and later) */

#if __MSVCRT_VERSION__ >= __MSVCR80_DLL
/* When linking with any non-free runtime, from MSVCR80.DLL onwards,
 * we must emulate _ftime(), by redirection to either _ftime32(), or
 * _ftime64(), depending on (error prone) user feature selection...
 */
# ifdef _USE_32BIT_TIME_T
  /* ...of an API with an explicitly 32-bit time_t...
   */
__CRT_ALIAS __cdecl __MINGW_NOTHROW
void _ftime (struct _timeb * __t) {_ftime32 ((struct __timeb32*)__t);}

# else
  /* ...or the Microsoft default, (incompatible with 32-bit MSVCRT.DLL,
   * and not consistently implemented within all 32-bit WinAPI modules),
   * 64-bit time_t
   */
__CRT_ALIAS __cdecl __MINGW_NOTHROW
void _ftime (struct _timeb * __t) {_ftime64 ((struct __timeb64*)__t);}

# endif /* !_USE_32BIT_TIME_T */
#endif	/* __MSVCR80_DLL and later */

#ifndef _NO_OLDNAMES
/* Regardless of how the _ftime() function may be implemented, we provide
 * the deprecated POSIX.1 equivalent API, by in-line mapping of the ftime()
 * function to that _ftime() implementation.
 */
__CRT_ALIAS __cdecl __MINGW_NOTHROW __POSIX_2001_DEPRECATED
void ftime (struct timeb *__t) {_ftime ((struct _timeb *)(__t)); }
#endif	/* !_NO_OLDNAMES */

_END_C_DECLS

#endif	/* ! RC_INVOKED */
#endif	/* !_SYS_TIMEB_H: $RCSfile: timeb.h,v $: end of file */
