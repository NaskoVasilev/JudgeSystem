/*
 * features.h
 *
 * Features configuration for MinGW.org GCC implementation; users may
 * customize this file, to establish their preferred default behaviour.
 * Projects may provide an alternative, package-specific configuration,
 * either by placing their own customized <features.h> in the package
 * -I path, ahead of the system default, or by assignment of their
 * preferred alternative to the _MINGW_FEATURES_HEADER macro.
 *
 *
 * $Id$
 *
 * Template written by Keith Marshall <keith@users.osdn.me>
 * Copyright (C) 2019, MinGW.org Project.
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
#ifndef __MINGW_FEATURES__
#pragma GCC system_header

/* Users are expected to customize this header, but it remains subject to
 * automatic updates by system software.  To ensure that any customisation
 * is not ovewritten, during such updates, it MUST observe the following:
 *
 * This header MUST define __MINGW_FEATURES__; the definition MUST begin
 * with "#define __MINGW_FEATURES__ (__MINGW_FEATURES_BEGIN__) \"; it MUST
 * extend over multiple lines, and terminate with "__MINGW_FEATURES_END__";
 * intervening lines may enumerate any defined features, one per line, and
 * each specified as an argument to either the __MINGW_FEATURE_ENABLE__(),
 * or the __MINGW_FEATURE_IGNORE__() macro, (ensuring that at least one
 * space separates either of these macro names from its parenthesized
 * argument name).
 *
 * CAUTION:
 * If customizing this features configuration, ALWAYS refer to features
 * using their designated symbolic constant names; NEVER usurp the use of
 * these symbolic constants for any other purpose, and NEVER assume that
 * any such constant has a specific value ... their definitions may vary
 * between distinct MinGW Runtime Library software releases!
 */
#define __MINGW_FEATURES__    (__MINGW_FEATURES_BEGIN__)        \
 __MINGW_FEATURE_IGNORE__     (__MINGW_ANSI_STDIO__)            \
 __MINGW_FEATURE_IGNORE__     (__MINGW_LC_MESSAGES__)           \
 __MINGW_FEATURE_IGNORE__     (__MINGW_LC_ENVVARS__)            \
 __MINGW_FEATURES_END__

#endif	/* !__MINGW_FEATURES__: $RCSfile$: end of file */
