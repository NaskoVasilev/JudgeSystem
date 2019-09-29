/*
 * malloc.h
 *
 * Declarations for non-standard heap management, and memory allocation
 * functions.  These augment the standard functions, which are declared
 * in <stdlib.h>
 *
 * $Id: malloc.h,v 82f89a7a28d8 2018/12/20 19:30:25 keith $
 *
 * Written by Colin Peters <colin@bird.fu.is.saga-u.ac.jp>
 * Copyright (C) 1997-1999, 2001-2005, 2007, 2018, MinGW.org Project.
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
 * disclaimer, shall be included in all copies or substantial portions of
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
#ifndef _MALLOC_H
#pragma GCC system_header
#define _MALLOC_H

/* All MinGW headers assume that <_mingw.h> is included; including
 * <stdlib.h>, which we also need here, is sufficient to make it so.
 */
#include <stdlib.h>

#ifndef RC_INVOKED

/* Microsoft stipulate that the alloca() API should be defined in this
 * header, whereas GNU specify it in its own dedicated header file; to
 * comply with both, we adopt the GNU stratagem, and then include the
 * GNU style dedicated header file here.
 */
#include "alloca.h"

typedef
struct _heapinfo
{ /* The structure used to control operation, and return information,
   * when walking the heap using the _heapwalk() function.
   */
  int		*_pentry;
  size_t	 _size;
  int		 _useflag;
} _HEAPINFO;

/* Status codes returned by _heapwalk()
 */
#define _HEAPEMPTY		(-1)
#define _HEAPOK 		(-2)
#define _HEAPBADBEGIN		(-3)
#define _HEAPBADNODE		(-4)
#define _HEAPEND		(-5)
#define _HEAPBADPTR		(-6)

/* Values returned by _heapwalk(), in the _HEAPINFO.useflag
 */
#define _FREEENTRY		 (0)
#define _USEDENTRY		 (1)

/* Maximum size permitted for a heap memory allocation request
 */
#define _HEAP_MAXREQ	(0xFFFFFFE0)

_BEGIN_C_DECLS

/* The _heap memory allocation functions are supported on WinNT, but not on
 * Win9X, (on which they always simply set errno to ENOSYS).
 */
_CRTIMP __cdecl __MINGW_NOTHROW  int    _heapwalk (_HEAPINFO *);

_CRTIMP __cdecl __MINGW_NOTHROW  int    _heapchk (void);
_CRTIMP __cdecl __MINGW_NOTHROW  int    _heapmin (void);

_CRTIMP __cdecl __MINGW_NOTHROW  int    _heapset (unsigned int);

_CRTIMP __cdecl __MINGW_NOTHROW  size_t _msize (void *);
_CRTIMP __cdecl __MINGW_NOTHROW  size_t _get_sbh_threshold (void);
_CRTIMP __cdecl __MINGW_NOTHROW  int    _set_sbh_threshold (size_t);
_CRTIMP __cdecl __MINGW_NOTHROW  void  *_expand (void *, size_t);

#ifndef _NO_OLDNAMES
/* Legacy versions of Microsoft runtimes may have supported this alternative
 * name for the _heapwalk() API.
 */
_CRTIMP __cdecl __MINGW_NOTHROW  int     heapwalk (_HEAPINFO *);
#endif	/* !_NO_OLDNAMES */

#if __MSVCRT_VERSION__ >= __MSVCR70_DLL
/* First introduced in non-free MSVCR70.DLL, the following were subsequently
 * made available from MSVCRT.DLL, from the release of WinXP onwards; however,
 * we choose to declare them only for the non-free case, preferring to emulate
 * them, in terms of libmingwex.a replacement implementations, for consistent
 * behaviour across ALL MSVCRT.DLL versions.
 */
_CRTIMP __cdecl __MINGW_NOTHROW __MINGW_ATTRIB_MALLOC
void *_aligned_malloc (size_t, size_t);

_CRTIMP __cdecl __MINGW_NOTHROW __MINGW_ATTRIB_MALLOC
void *_aligned_offset_malloc (size_t, size_t, size_t);

_CRTIMP __cdecl __MINGW_NOTHROW
void *_aligned_realloc (void *, size_t, size_t);

_CRTIMP __cdecl __MINGW_NOTHROW
void *_aligned_offset_realloc (void *, size_t, size_t, size_t);

_CRTIMP __cdecl __MINGW_NOTHROW
void  _aligned_free (void *);

/* Curiously, there are no "calloc()" alike variants of the following pair of
 * "recalloc()" alike functions; furthermore, neither of these is provided by
 * any version of pseudo-free MSVCRT.DLL
 */
_CRTIMP __cdecl __MINGW_NOTHROW
void *_aligned_recalloc (void *, size_t, size_t, size_t);

_CRTIMP __cdecl __MINGW_NOTHROW
void *_aligned_offset_recalloc (void *, size_t, size_t, size_t, size_t);

#endif	/* Non-free MSVCR70.DLL, or later */

/* The following emulations are provided in libmingwex.a; they are suitable
 * for use on any Windows version, irrespective of the limited availability
 * of the preceding Microsoft implementations.
 */
__cdecl __MINGW_NOTHROW __MINGW_ATTRIB_MALLOC
void *__mingw_aligned_malloc (size_t, size_t);

__cdecl __MINGW_NOTHROW __MINGW_ATTRIB_MALLOC
void *__mingw_aligned_offset_malloc (size_t, size_t, size_t);

__cdecl __MINGW_NOTHROW
void *__mingw_aligned_offset_realloc (void *, size_t, size_t, size_t);

__cdecl __MINGW_NOTHROW
void *__mingw_aligned_realloc (void *, size_t, size_t);

__cdecl __MINGW_NOTHROW
void __mingw_aligned_free (void *);

#if __MSVCRT_VERSION__ < __MSVCR70_DLL
/* Although the Microsoft aligned heap allocation functions are present in
 * MSVCRT.DLL, from WinXP onwards, we choose to retain our legacy supporting
 * emulations across all MSVCRT.DLL versions; thus, we enable the following
 * in-line emulations in all cases where the user has not specified use of
 * non-free MSVCR70.DLL or later.
 *
 * Note that because these emulations are deployed as in-line replacements
 * of their emulated function calls, GCC will not normally provide any means
 * of obtaining externally accessible entry-point addresses for them; if it
 * becomes necessary to dereference such an address, the requirement may be
 * satisfied by linking with the auxiliary "-lmemalign" library.
 */
__cdecl __MINGW_NOTHROW __MINGW_ATTRIB_MALLOC
__CRT_ALIAS __LIBIMPL__((LIB = memalign, FUNCTION = aligned_malloc))
void *_aligned_malloc (size_t __wanted, size_t __aligned )
{ return __mingw_aligned_offset_malloc (__wanted, __aligned, (size_t)(0)); }

__cdecl __MINGW_NOTHROW __MINGW_ATTRIB_MALLOC
__CRT_ALIAS __LIBIMPL__((LIB = memalign, FUNCTION = aligned_offset_malloc))
void *_aligned_offset_malloc (size_t __wanted, size_t __aligned, size_t __offset )
{ return __mingw_aligned_offset_malloc (__wanted, __aligned, __offset); }

__cdecl __MINGW_NOTHROW
__CRT_ALIAS __LIBIMPL__((LIB = memalign, FUNCTION = aligned_realloc))
void *_aligned_realloc (void *__ptr, size_t __wanted, size_t __aligned )
{ return __mingw_aligned_offset_realloc (__ptr, __wanted, __aligned, (size_t)(0)); }

__cdecl __MINGW_NOTHROW
__CRT_ALIAS __LIBIMPL__((LIB = memalign, FUNCTION = aligned_offset_realloc))
void *_aligned_offset_realloc (void *__ptr, size_t __wanted, size_t __aligned, size_t __offset )
{ return __mingw_aligned_offset_realloc (__ptr, __wanted, __aligned, __offset); }

__cdecl __MINGW_NOTHROW
__CRT_ALIAS __LIBIMPL__((LIB = memalign, FUNCTION = aligned_free))
void _aligned_free (void *__ptr) { __mingw_free (__ptr); }

#endif	/* __MSVCRT_VERSION__ < __MSVCR70_DLL */

/* Regardless of availability of their Microsoft alternatives, the
 * __mingw_aligned_malloc(), and __mingw_aligned_realloc() functions
 * may always be implemented in terms of their "offset" siblings, by
 * simply specifying an offset of zero.
 */
__cdecl __MINGW_NOTHROW __MINGW_ATTRIB_MALLOC
__CRT_ALIAS __LIBIMPL__(( FUNCTION = mingw_aligned_malloc ))
void *__mingw_aligned_malloc( size_t __want, size_t __aligned )
{ return __mingw_aligned_offset_malloc( __want, __aligned, (size_t)(0) ); }

__cdecl __MINGW_NOTHROW
__CRT_ALIAS __LIBIMPL__(( FUNCTION = mingw_aligned_realloc ))
void *__mingw_aligned_realloc( void *__ptr, size_t __want, size_t __aligned )
{ return __mingw_aligned_offset_realloc( __ptr, __want, __aligned, (size_t)(0) ); }

_END_C_DECLS

#endif	/* ! RC_INVOKED */
#endif	/* !_MALLOC_H: $RCSfile: malloc.h,v $: end of file */
