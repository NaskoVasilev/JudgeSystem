/*
 * string.h
 *
 * ISO-C standard header, with MSVC compatible extensions.
 *
 * $Id: string.h,v 6085af566dd0 2017/12/18 11:45:49 keith $
 *
 * Written by Colin Peters <colin@bird.fu.is.saga-u.ac.jp>
 * Copyright (C) 1997-2000, 2002-2004, 2007, 2009, 2015-2017,
 *  MinGW.org Project.
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
#ifndef _STRING_H
#pragma GCC system_header
#define _STRING_H

/* All MinGW system headers must include this...
 */
#include <_mingw.h>

#ifndef RC_INVOKED
/* ISO-C requires this header to expose definitions for NULL and size_t,
 * retaining compatiblity with their fundamental <stddef.h> definitions.
 */
#define __need_NULL
#define __need_size_t
#ifndef __STRICT_ANSI__
 /* MSVC extends this requirement to include a definition of wchar_t,
  * (which contravenes strict ISO-C standards conformity).
  */
# define __need_wchar_t
#endif
#include <stddef.h>

#if _EMULATE_GLIBC
/* GNU's glibc declares strcasecmp() and strncasecmp() in <string.h>,
 * contravening POSIX.1-2008 which requires them to be declared only in
 * <strings.h>; we may emulate this anomalous glibc behaviour, which is
 * ostensibly to support BSD usage, (in spite of such usage now being
 * obsolete in BSD), by simply including our <strings.h> here.
 */
#include <strings.h>
#endif

_BEGIN_C_DECLS

#define __STRING_H_SOURCED__
/* Prototypes for the ISO-C Standard library string functions.
 */
_CRTIMP __cdecl __MINGW_NOTHROW  void *memchr (const void *, int, size_t) __MINGW_ATTRIB_PURE;
_CRTIMP __cdecl __MINGW_NOTHROW  int memcmp (const void *, const void *, size_t) __MINGW_ATTRIB_PURE;
_CRTIMP __cdecl __MINGW_NOTHROW  void *memcpy (void *, const void *, size_t);
_CRTIMP __cdecl __MINGW_NOTHROW  void *memmove (void *, const void *, size_t);
_CRTIMP __cdecl __MINGW_NOTHROW  void *memset (void *, int, size_t);
_CRTIMP __cdecl __MINGW_NOTHROW  char *strcat (char *, const char *);
_CRTIMP __cdecl __MINGW_NOTHROW  char *strchr (const char *, int) __MINGW_ATTRIB_PURE;
_CRTIMP __cdecl __MINGW_NOTHROW  int strcmp (const char *, const char *) __MINGW_ATTRIB_PURE;
_CRTIMP __cdecl __MINGW_NOTHROW  int strcoll (const char *, const char *); /* Compare using locale */
_CRTIMP __cdecl __MINGW_NOTHROW  char *strcpy (char *, const char *);
_CRTIMP __cdecl __MINGW_NOTHROW  size_t strcspn (const char *, const char *) __MINGW_ATTRIB_PURE;
_CRTIMP __cdecl __MINGW_NOTHROW  char *strerror (int); /* NOTE: NOT an old name wrapper. */

_CRTIMP __cdecl __MINGW_NOTHROW  size_t strlen (const char *) __MINGW_ATTRIB_PURE;
_CRTIMP __cdecl __MINGW_NOTHROW  char *strncat (char *, const char *, size_t);
_CRTIMP __cdecl __MINGW_NOTHROW  int strncmp (const char *, const char *, size_t) __MINGW_ATTRIB_PURE;
_CRTIMP __cdecl __MINGW_NOTHROW  char *strncpy (char *, const char *, size_t);
_CRTIMP __cdecl __MINGW_NOTHROW  char *strpbrk (const char *, const char *) __MINGW_ATTRIB_PURE;
_CRTIMP __cdecl __MINGW_NOTHROW  char *strrchr (const char *, int) __MINGW_ATTRIB_PURE;
_CRTIMP __cdecl __MINGW_NOTHROW  size_t strspn (const char *, const char *) __MINGW_ATTRIB_PURE;
_CRTIMP __cdecl __MINGW_NOTHROW  char *strstr (const char *, const char *) __MINGW_ATTRIB_PURE;
_CRTIMP __cdecl __MINGW_NOTHROW  char *strtok (char *, const char *);
_CRTIMP __cdecl __MINGW_NOTHROW  size_t strxfrm (char *, const char *, size_t);

#ifndef __STRICT_ANSI__
/* Extra non-ANSI functions provided by the CRTDLL library
 */
_CRTIMP __cdecl __MINGW_NOTHROW  char *_strerror (const char *);
_CRTIMP __cdecl __MINGW_NOTHROW  void *_memccpy (void *, const void *, int, size_t);
_CRTIMP __cdecl __MINGW_NOTHROW  int _memicmp (const void *, const void *, size_t);
_CRTIMP __cdecl __MINGW_NOTHROW  char *_strdup (const char *) __MINGW_ATTRIB_MALLOC;
_CRTIMP __cdecl __MINGW_NOTHROW  int _strcmpi (const char *, const char *);
_CRTIMP __cdecl __MINGW_NOTHROW  int _stricoll (const char *, const char *);
_CRTIMP __cdecl __MINGW_NOTHROW  char *_strlwr (char *);
_CRTIMP __cdecl __MINGW_NOTHROW  char *_strnset (char *, int, size_t);
_CRTIMP __cdecl __MINGW_NOTHROW  char *_strrev (char *);
_CRTIMP __cdecl __MINGW_NOTHROW  char *_strset (char *, int);
_CRTIMP __cdecl __MINGW_NOTHROW  char *_strupr (char *);
_CRTIMP __cdecl __MINGW_NOTHROW  void _swab (const char *, char *, size_t);

#if !_EMULATE_GLIBC
/* MSVC's non-ANSI _stricmp() and _strnicmp() functions must also be
 * prototyped here, but we need to share them with <strings.h>, where
 * we declare their POSIX strcasecmp() and strncasecmp() equivalents;
 * get the requisite prototypes by selective <strings.h> inclusion,
 * (noting that we've already done so, if emulating glibc, but if not,
 * we use quoted inclusion now, to ensure that we get our "strings.h",
 * which is equipped to support __STRING_H_SOURCED__ filtering).
 */
#include "strings.h"
#endif

# ifdef __MSVCRT__
 /* These were not present in the CRTDLL prior to the first release of
  * MSVCRT.DLL, but are available in all versions of that library.
  */
_CRTIMP __cdecl __MINGW_NOTHROW  int _strncoll(const char *, const char *, size_t);
_CRTIMP __cdecl __MINGW_NOTHROW  int _strnicoll(const char *, const char *, size_t);
# endif

# ifndef _NO_OLDNAMES
 /* Non-underscore decorated versions of non-ANSI functions. They live in the
  * OLDNAMES libraries, whence they provide a little extra portability.
  */
_CRTIMP __cdecl __MINGW_NOTHROW  void *memccpy (void *, const void *, int, size_t);
_CRTIMP __cdecl __MINGW_NOTHROW  int memicmp (const void *, const void *, size_t);
_CRTIMP __cdecl __MINGW_NOTHROW  char *strdup (const char *) __MINGW_ATTRIB_MALLOC;
_CRTIMP __cdecl __MINGW_NOTHROW  int strcmpi (const char *, const char *);
_CRTIMP __cdecl __MINGW_NOTHROW  int stricmp (const char *, const char *);
_CRTIMP __cdecl __MINGW_NOTHROW  int stricoll (const char *, const char *);
_CRTIMP __cdecl __MINGW_NOTHROW  char *strlwr (char *);
_CRTIMP __cdecl __MINGW_NOTHROW  int strnicmp (const char *, const char *, size_t);
_CRTIMP __cdecl __MINGW_NOTHROW  char *strnset (char *, int, size_t);
_CRTIMP __cdecl __MINGW_NOTHROW  char *strrev (char *);
_CRTIMP __cdecl __MINGW_NOTHROW  char *strset (char *, int);
_CRTIMP __cdecl __MINGW_NOTHROW  char *strupr (char *);

#  ifndef _UWIN
  /* FIXME: Do we really care that UWin doesn't support this?  We are
   * under no obligation to support UWin.
   */
_CRTIMP __cdecl __MINGW_NOTHROW  void swab (const char *, char *, size_t);

#  endif /* ! _UWIN */
# endif /* ! _NO_OLDNAMES */

/* MSVC also expects <string.h> to declare duplicates of the wchar_t
 * string functions which are nominally declared in <wchar.h>, (which
 * is where ISO-C specifies that they should be declared).  For the
 * convenience of applications which rely on this Microsoft anomaly,
 * inclusion of <wchar.h>, within the current __STRING_H_SOURCED__
 * scope, will selectively expose the required function prototypes;
 * however, strictly ISO-C conforming applications should include
 * <wchar.h> directly; they should not rely on this MSVC specific
 * anomalous behaviour.  (We use the quoted form of inclusion here,
 * to ensure that we get our own "wchar.h", and not any predecessor
 * which may have been insinuated into the system include path, and
 * so could interfere with our mechanism for partial inclusion of
 * shared header content).
 */
#include "wchar.h"

#endif /* ! __STRICT_ANSI__ */

#if _POSIX_C_SOURCE >= 200809L
#if __MSVCRT_VERSION__ >= __MSVCR80_DLL
/* MSVCR80.DLL adds a (mostly) POSIX.1-2008 conforming strnlen(); (it's
 * also available in MSVCRT.DLL from _WIN32_WINNT_VISTA onwards, but we
 * pretend otherwise, since recent GCC will try to use the function when
 * it can be found in libmsvcrt.a, so breaking it for use on WinXP and
 * earlier).
 */
_CRTIMP __cdecl __MINGW_NOTHROW  char *strnlen (const char *, size_t);

#else	/* MSVCRT.DLL || pre-MSVCR80.DLL */
/* Emulation, to support recent POSIX.1; we prefer this for ALL versions
 * of MSVCRT.DLL, (even those which already provide strnlen()); to avoid
 * the GCC breakage noted above.  (Note that we implement strnlen() with
 * the alternative external name, __mingw_strnlen() in libmingwex.a, to
 * avoid possible link time collision with MSVCR80.DLL's implementation,
 * then map this to strnlen() via a __CRT_ALIAS, with stubs designated
 * for linking from within the appropriate oldname libraries.
 */
extern size_t __mingw_strnlen (const char *, size_t);

__JMPSTUB__(( LIB=coldname; FUNCTION=strnlen ))
__CRT_ALIAS size_t strnlen (const char *__text, size_t __maxlen)
{ return __mingw_strnlen (__text, __maxlen); }

#endif	/* MSVCRT.DLL || pre-MSVCR80.DLL */
#endif	/* _POSIX_C_SOURCE >= 200809L */

#if _POSIX_C_SOURCE >= 199506L  /* SUSv2 */
/* SUSv2 added a re-entrant variant of strtok(), which maintains state
 * using a user supplied reference pointer, rather than the internal
 * reference used by strtok() itself, thus making it both thread-safe,
 * and suitable for interleaved use on multiple strings, even within a
 * single thread context, (which isn't possible with strtok() itself,
 * even with Microsoft's intrinsically thread-safe implementation).
 */
extern char *strtok_r
(char *__restrict__, const char *__restrict__, char **__restrict__);

#if _POSIX_C_SOURCE >= 200112L
/* POSIX.1-2001 added a re-entrant variant of strerror(), which stores
 * the message text in a user supplied buffer, rather than in (possibly
 * volatile) system supplied storage.  Although inherently thread-safe,
 * Microsoft's strerror() also uses a potentially volatile buffer, (in
 * the sense that it is overwritten by successive calls within a single
 * thread); thus, we provide our own implementation of POSIX.1-2001's
 * strerror_r() function, to facilitate the return of non-volatile
 * copies of strerror()'s message text.
 */
extern int strerror_r (int, char *, size_t);

#endif	/* POSIX.1-2001 */
#endif	/* SUSv2 */

#if __MSVCRT_VERSION__>=__MSVCR80_DLL || _WIN32_WINNT >= _WIN32_WINNT_VISTA
/* MSVCR80.DLL introduced a safer, (erroneously so called "more secure"),
 * alternative to strerror(), named strerror_s(); it was later retrofitted
 * to MSVCRT.DLL, from the release of Windows-Vista onwards.
 */
_CRTIMP __cdecl __MINGW_NOTHROW  int strerror_s (char *, size_t, int);

/* Also introduced in MSVCR80.DLL, and retrofitted to MSVCRT.DLL from the
 * release of Windows-Vista, strtok_s() is a direct analogue for POSIX.1's
 * strtok_r() function; (contrary to Microsoft's description, it is neither
 * a "more secure", nor even a "safer" version of strtok() itself).
 */
_CRTIMP __cdecl __MINGW_NOTHROW  char *strtok_s (char *, const char *, char **);

#elif _POSIX_C_SOURCE >= 200112L
/* For the benefit of pre-Vista MSVCRT.DLL users, we provide an approximate
 * emulation of strerror_s(), in terms of inline referral to POSIX.1-2001's
 * strerror_r() function.
 */
__CRT_ALIAS int strerror_s (char *__buf, size_t __len, int __err)
{ return strerror_r (__err, __buf, __len); }
#endif

#undef __STRING_H_SOURCED__

_END_C_DECLS

#endif	/* ! RC_INVOKED */
#endif	/* !_STRING_H: $RCSfile: string.h,v $: end of file */
