/*
 * conio.h
 *
 * Low level console I/O functions.  Pretty please try to use the ANSI
 * standard ones if you are writing new code.
 *
 * $Id: conio.h,v d1ea52cf7548 2018/10/21 15:39:35 keith $
 *
 * Written by Colin Peters <colin@bird.fu.is.saga-u.ac.jp>
 * Copyright (C) 1997, 1999-2001, 2003, 2004, 2007, 2018, MinGW.org Project.
 *
 *
 * Permission is hereby granted, free of charge, to any person obtaining a
 * copy of this software and associated documentation files (the "Software"),
 * to deal in the Software without restriction, including without limitation
 * the rights to use, copy, modify, merge, publish, distribute, sublicense,
 * and/or sell copies of the Software, and to permit persons to whom the
 * Software is furnished to do so, subject to the following conditions:
 *
 * The above copyright notice, this permission notice, and the following
 * disclaimer shall be included in all copies or substantial portions of
 * the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
 * OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.  IN NO EVENT SHALL
 * THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
 * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OF OR OTHER
 * DEALINGS IN THE SOFTWARE.
 *
 */
#ifndef _CONIO_H
#pragma GCC system_header

/* When including <wchar.h>, some of the definitions and declarations
 * which are nominally provided in <conio.h> must be duplicated.  Rather
 * than require duplicated maintenance effort, we provide for partial
 * inclusion of <conio.h> by <wchar.h>; only when not included in
 * this partial fashion...
 */
#ifndef __WCHAR_H_SOURCED__
 /* ...which is exclusive to <wchar.h>, do we assert the multiple
  * inclusion guard for <conio.h> itself.
  */
#define _CONIO_H

/* All MinGW.org headers are expected to include <_mingw.h>; when
 * selectively included by <wchar.h>, that responsibility has already
 * been addressed, but for free-standing inclusion we do so now.
 */
#include <_mingw.h>
#endif	/* !__WCHAR_H_SOURCED__ */

#ifndef RC_INVOKED
/* There is nothing here which is useful to the resource compiler;
 * for any other form of compilation, and regardless of the scope in
 * which <conio.h> is included, we need definitions for wchar_t, and
 * wint_t; get them by selective inclusion of <stddef.h>.
 */
#define __need_wint_t
#define __need_wchar_t
#include <stddef.h>

_BEGIN_C_DECLS

#ifdef _CONIO_H
/* The following declarations are to be exposed only on free-standing
 * inclusion of <conio.h>
 */
_CRTIMP __cdecl __MINGW_NOTHROW  char *_cgets (char*);
_CRTIMP __cdecl __MINGW_NOTHROW  int   _cprintf (const char*, ...);
_CRTIMP __cdecl __MINGW_NOTHROW  int   _cputs (const char*);
_CRTIMP __cdecl __MINGW_NOTHROW  int   _cscanf (char*, ...);

_CRTIMP __cdecl __MINGW_NOTHROW  int   _getch (void);
_CRTIMP __cdecl __MINGW_NOTHROW  int   _getche (void);
_CRTIMP __cdecl __MINGW_NOTHROW  int   _kbhit (void);
_CRTIMP __cdecl __MINGW_NOTHROW  int   _putch (int);
_CRTIMP __cdecl __MINGW_NOTHROW  int   _ungetch (int);

#if _WIN32_WINNT >= _WIN32_WINNT_WINXP || __MSVCRT_VERSION__ >= __MSVCR70_DLL
/* Wide character variants of the console I/O functions were first
 * introduced in non-free MSVCR70.DLL, and subsequently supported by
 * MSVCRT.DLL from WinXP onwards.  Some are declared in <wchar.t> in
 * addition to <conio.h>; the following are exclusive to <conio.h>
 */
_CRTIMP __cdecl __MINGW_NOTHROW  wint_t  _putwch (wchar_t);

#if __MSVCRT_VERSION__ >= __MSVCR80_DLL
/* Variants which do not perform thread locking require non-free
 * MSVCR80.DLL, or later; they are not supported by MSVCRT.DLL
 */
_CRTIMP __cdecl __MINGW_NOTHROW  int     _getch_nolock (void);
_CRTIMP __cdecl __MINGW_NOTHROW  int     _getche_nolock (void);
_CRTIMP __cdecl __MINGW_NOTHROW  int     _putch_nolock (int);
_CRTIMP __cdecl __MINGW_NOTHROW  wint_t  _putwch_nolock (wchar_t);
_CRTIMP __cdecl __MINGW_NOTHROW  int     _ungetch_nolock (int);

#endif	/* MSVCR80.DLL or later */
#endif	/* WinXP, MSVCR70.DLL, or later */

#ifndef _NO_OLDNAMES
/* Early versions of the Microsoft runtime library provided a subset
 * of the above functions, named without the ugly initial underscore;
 * these remain supported, and should be used when coding to support
 * legacy Windows platforms.
 */
_CRTIMP __cdecl __MINGW_NOTHROW  int  getch (void);
_CRTIMP __cdecl __MINGW_NOTHROW  int  getche (void);
_CRTIMP __cdecl __MINGW_NOTHROW  int  kbhit (void);
_CRTIMP __cdecl __MINGW_NOTHROW  int  putch (int);
_CRTIMP __cdecl __MINGW_NOTHROW  int  ungetch (int);

#endif	/* !_NO_OLDNAMES */
#endif	/* _CONIO_H */

#if ! (defined _CONIO_H && defined _WCHAR_H)
/* The following are to be exposed either on free-standing inclusion
 * of <conio.h>, or on selective inclusion by <wchar.h>, but if both
 * guards are defined, then this is free-standing inclusion, and we
 * have already declared these by selective inclusion; there is no
 * need to declare them a second time.
 */
#if _WIN32_WINNT >= _WIN32_WINNT_WINXP || __MSVCRT_VERSION__ >= __MSVCR70_DLL
/* Wide character variants of the console I/O functions, in this group,
 * were first introduced in non-free Microsoft runtimes, from MSVCR70.DLL
 * onwards; they were not supported by MSVCRT.DLL prior to WinXP.
 */
_CRTIMP __cdecl __MINGW_NOTHROW  wint_t  _getwch (void);
_CRTIMP __cdecl __MINGW_NOTHROW  wint_t  _getwche (void);
_CRTIMP __cdecl __MINGW_NOTHROW  wint_t  _ungetwch (wint_t);

#if __MSVCRT_VERSION__ >= __MSVCR80_DLL
/* Variants which do not perform thread locking require non-free
 * MSVCR80.DLL, or later; they are not supported by MSVCRT.DLL
 */
_CRTIMP __cdecl __MINGW_NOTHROW  wint_t  _getwch_nolock (void);
_CRTIMP __cdecl __MINGW_NOTHROW  wint_t  _getwche_nolock (void);
_CRTIMP __cdecl __MINGW_NOTHROW  wint_t  _ungetwch_nolock (wint_t);

#endif	/* MSVCR80.DLL or later */
#endif	/* WinXP, MSVCR70.DLL, or later */
#endif	/* ! (_CONIO_H && _WCHAR_H) */

_END_C_DECLS

#endif	/* ! RC_INVOKED */
#endif	/* !_CONIO_H: $RCSfile: conio.h,v $: end of file */
