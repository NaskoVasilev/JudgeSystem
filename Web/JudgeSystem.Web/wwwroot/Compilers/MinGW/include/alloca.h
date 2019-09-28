/*
 * alloca.h
 *
 * Declarations for the alloca() function API, conforming to both GNU and
 * Microsoft's implementation conventions.
 *
 *
 * $Id: alloca.h,v bc7b73f76386 2019/07/01 20:48:01 keith $
 *
 * Written by Keith Marshall <keith@users.osdn.me>
 * Copyright (C) 2018, 2019, MinGW.org Project.
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
#ifndef _ALLOCA_H
#define _ALLOCA_H
/* Microsoft requires the alloca() API to be declared in <malloc.h>;
 * GNU declares it in <alloca.h>, with default inclusion by <stdlib.h>
 * when !__STRICT_ANSI__.  To achieve compatibility with both, we will
 * define it in the GNU manner, conditionally including this file when
 * reading <stdlib.h>, and UNCONDITIONALLY including it in <malloc.h>
 */
#ifdef __GNUC__
#pragma GCC system_header
/* This implementation is unsupported, for any compiler other than GCC,
 * (which is the standard MinGW compiler, in any case); all MinGW source
 * may assume that <_mingw.h> has been included, so ensure that it is.
 */
#include <_mingw.h>

/* We must also ensure that the "size_t" type definition is in scope;
 * we may guarantee this, by selective inclusion from <stddef.h>
 */
#define __need_size_t
#include <stddef.h>

_BEGIN_C_DECLS

/* Regardless of whether a GNU compatible alloca() implementation, or
 * a MSVC compatible _alloca() implementation is required, it is always
 * appropriate to delegate the call to GCC's __builtin_alloca(); we use
 * a preprocessor macro, rather than an in-line function implementation,
 * to delegate the call, because:
 *
 *  - older GCC versions do not permit in-lining of __builtin_alloca();
 *
 *  - more recent GCC versions generate marginally better code, for the
 *    macro expansion, than they do for an in-line function expansion,
 *    when compiling at optimization level -O0; (both implementation
 *    choices result in identical code, at -O1 and higher);
 *
 *  - the usual argument for C++ namespace qualification, in the case of
 *    an in-line function implementation, is unwarranted for alloca().
 *
 */
#if defined _GNU_SOURCE || ! defined _NO_OLDNAMES
/* This is the GNU standard API; it is also compatible with Microsoft's
 * original, but now deprecated, OLDNAMES naming convention.
 */
#undef alloca
void *alloca( size_t );
#define alloca( __request )  __builtin_alloca( __request )
#endif	/* _GNU_SOURCE || !_NO_OLDNAMES */

/* This represents the same API, but conforms to Microsoft's currently
 * preferred naming convention.
 */
#undef _alloca
void *_alloca( size_t );
#define _alloca( __request )  __builtin_alloca( __request )

_END_C_DECLS

#endif	/* __GNUC__ */
#endif	/* !_ALLOCA_H: $RCSfile: alloca.h,v $: end of file */
