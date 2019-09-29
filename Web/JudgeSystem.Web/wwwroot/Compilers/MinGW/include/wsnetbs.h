/*
 * wsnetbs.h
 *
 * NetBIOS extensions to the standard WinSock API.
 *
 * $Id: wsnetbs.h,v a14ae09b813e 2019/02/03 15:47:20 keith $
 *
 * Written by Kai Henningsen <kai-henningsen@users.sourceforge.net>
 * Copyright (C) 2002, 2017, 2019, MinGW.org Project
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
#ifndef _WSNETBS_H
#pragma GCC system_header
#define _WSNETBS_H

#define NETBIOS_NAME_LENGTH		16
#define NETBIOS_UNIQUE_NAME		 0
#define NETBIOS_GROUP_NAME		 1
#define NETBIOS_TYPE_QUICK_UNIQUE	 2
#define NETBIOS_TYPE_QUICK_GROUP	 3

#ifndef RC_INVOKED

#define __USE_MINGW_WINSOCK_DEFAULT
#include "winsock.h"

typedef struct sockaddr_nb
{ short 	snb_family;
  u_short	snb_type;
  char		snb_name[NETBIOS_NAME_LENGTH];
} SOCKADDR_NB, *PSOCKADDR_NB, *LPSOCKADDR_NB;

#define SET_NETBIOS_SOCKADDR(_snb,_type,_name,_port)			\
  { register int _i;							\
    register char *_n = (_name);					\
    register PSOCKADDR_NB _s = (_snb);					\
    _s->snb_family = AF_NETBIOS;					\
    _s->snb_type = (_type);						\
    for (_i = 0; _n[_i] != '\0' && _i < NETBIOS_NAME_LENGTH - 1; _i++)	\
      { _s->snb_name[_i] = _n[_i]; }					\
    while (_i < NETBIOS_NAME_LENGTH - 1)				\
      { _s->snb_name[_i++] = ' '; }					\
    _s->snb_name[NETBIOS_NAME_LENGTH - 1] = (_port);			\
  }

#endif	/* RC_INVOKED */
#endif	/* !_WSNETBS_H: $RCSfile: wsnetbs.h,v $: end of file */
