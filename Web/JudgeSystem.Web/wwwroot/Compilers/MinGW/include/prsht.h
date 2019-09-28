/*
 * prsht.h
 *
 * Windows Property Sheet API definitions.
 *
 *
 * $Id: prsht.h,v bc2e65f722d0 2018/11/16 20:03:22 keith $
 *
 * Written by Anders Norlander  <anorland@hem2.passagen.se>
 * Copyright (C) 1998, 1999, 2001-2004, 2010, 2018, MinGW.org Project.
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
#ifndef _PRSHT_H
#pragma GCC system_header
#define _PRSHT_H

#include <winuser.h>

#ifdef __cplusplus
# define POSTMSG  ::PostMessage
# define SNDMSG   ::SendMessage
#else	/* !__cplusplus */
# define POSTMSG    PostMessage
# define SNDMSG     SendMessage
#endif	/* !__cplusplus */

_BEGIN_C_DECLS

#define MAXPROPPAGES			       100

#define PSP_DEFAULT				 0
#define PSP_DLGINDIRECT 			 1
#define PSP_USEHICON				 2
#define PSP_USEICONID				 4
#define PSP_USETITLE				 8
#define PSP_RTLREADING				16
#define PSP_HASHELP				32
#define PSP_USEREFPARENT			64
#define PSP_USECALLBACK 		       128
#define PSP_PREMATURE			      1024

#if _WIN32_IE >= _WIN32_IE_IE40

#define PSP_HIDEHEADER			      2048
#define PSP_USEHEADERTITLE		      4096
#define PSP_USEHEADERSUBTITLE		      8192

#endif	/* _WIN32_IE >= _WIN32_IE_IE40 */

#define PSPCB_RELEASE				 1
#define PSPCB_CREATE				 2

#define PSH_DEFAULT				 0
#define PSH_PROPTITLE				 1
#define PSH_USEHICON				 2
#define PSH_USEICONID				 4
#define PSH_PROPSHEETPAGE			 8
#define PSH_WIZARDHASFINISH			16
#define PSH_WIZARD				32
#define PSH_USEPSTARTPAGE			64
#define PSH_NOAPPLYNOW			       128
#define PSH_USECALLBACK 		       256
#define PSH_HASHELP			       512
#define PSH_MODELESS			      1024
#define PSH_RTLREADING			      2048
#define PSH_WIZARDCONTEXTHELP		      4096

#if _WIN32_IE >= _WIN32_IE_IE40

#define PSH_WATERMARK			     32768
#define PSH_USEHBMWATERMARK		     65536
#define PSH_USEHPLWATERMARK		    131072
#define PSH_STRETCHWATERMARK		    262144
#define PSH_HEADER			    524288
#define PSH_USEHBMHEADER		   1048576
#define PSH_USEPAGELANG 		   2097152

#if _WIN32_IE < _WIN32_IE_IE50
/* First introduced with IE-4.0, this definition prevailed
 * until the subsequent release of IE-5.0 ...
 */
#define PSH_WIZARD97			0x00002000

#else	/* _WIN32_IE >= _WIN32_IE_IE50 */
/* ... when the applicable definition was changed, becoming
 * the following for all IE versions from IE-5.0 onwards.
 */
#define PSH_WIZARD97			0x01000000

/* IE-5.0 also introduced the following related definitions.
 */
#define PSH_WIZARD_LITE 		  0x400000
#define PSH_NOCONTEXTHELP		 0x2000000

#endif	/* _WIN32_IE >= _WIN32_IE_IE50 */
#endif	/* _WIN32_IE >= _WIN32_IE_IE40 */

#define PSCB_INITIALIZED			 1
#define PSCB_PRECREATE				 2

#define PSM_GETTABCONTROL		      1140
#define PSM_GETCURRENTPAGEHWND		      1142
#define PSM_ISDIALOGMESSAGE		      1141
#define PSM_PRESSBUTTON 		      1137
#define PSM_SETCURSELID 		      1138

#define PSM_SETFINISHTEXT  __AW_SUFFIXED__( PSM_SETFINISHTEXT )

#define PSM_SETFINISHTEXTW		      1145
#define PSM_SETFINISHTEXTA		      1139

#define PSN_FIRST			     (-200)
#define PSN_LAST			     (-299)
#define PSN_APPLY			     (-202)
#define PSN_HELP			     (-205)
#define PSN_KILLACTIVE			     (-201)
#define PSN_QUERYCANCEL 		     (-209)
#define PSN_RESET			     (-203)
#define PSN_SETACTIVE			     (-200)
#define PSN_WIZBACK			     (-206)
#define PSN_WIZFINISH			     (-208)
#define PSN_WIZNEXT			     (-207)

#define PSNRET_NOERROR				 0
#define PSNRET_INVALID				 1
#define PSNRET_INVALID_NOCHANGEPAGE		 2

#define ID_PSRESTARTWINDOWS			 2
#define ID_PSREBOOTSYSTEM			 3

#define WIZ_CXDLG			       276
#define WIZ_CYDLG			       140
#define WIZ_CXBMP				80
#define WIZ_BODYX				92
#define WIZ_BODYCX			       184

#define PROP_SM_CXDLG			       212
#define PROP_SM_CYDLG			       188
#define PROP_MED_CXDLG			       227
#define PROP_MED_CYDLG			       215
#define PROP_LG_CXDLG			       252
#define PROP_LG_CYDLG			       218

#define PSBTN_MAX				 6
#define PSBTN_BACK				 0
#define PSBTN_NEXT				 1
#define PSBTN_FINISH				 2
#define PSBTN_OK				 3
#define PSBTN_APPLYNOW				 4
#define PSBTN_CANCEL				 5
#define PSBTN_HELP				 6

#define PSWIZB_BACK				 1
#define PSWIZB_NEXT				 2
#define PSWIZB_FINISH				 4
#define PSWIZB_DISABLEDFINISH			 8

#define PSM_SETWIZBUTTONS	    (WM_USER + 112)
#define PSM_APPLY		    (WM_USER + 110)
#define PSM_UNCHANGED		    (WM_USER + 109)
#define PSM_QUERYSIBLINGS	    (WM_USER + 108)
#define PSM_CANCELTOCLOSE	    (WM_USER + 107)
#define PSM_REBOOTSYSTEM	    (WM_USER + 106)
#define PSM_RESTARTWINDOWS	    (WM_USER + 105)
#define PSM_CHANGED		    (WM_USER + 104)
#define PSM_ADDPAGE		    (WM_USER + 103)
#define PSM_REMOVEPAGE		    (WM_USER + 102)
#define PSM_SETCURSEL		    (WM_USER + 101)

#define PSM_SETTITLE     __AW_SUFFIXED__( PSM_SETTITLE )

#define PSM_SETTITLEA		    (WM_USER + 111)
#define PSM_SETTITLEW		    (WM_USER + 120)

#ifndef RC_INVOKED

#pragma pack (push, 8)

#define PROPSHEETPAGE    __AW_SUFFIXED__( PROPSHEETPAGE )
#define LPPROPSHEETPAGE  __AW_SUFFIXED__( LPPROPSHEETPAGE )

typedef
struct _PROPSHEETPAGEA
{ DWORD 		 dwSize;
  DWORD 		 dwFlags;
  HINSTANCE		 hInstance;

  _ANONYMOUS_UNION union
  { LPCSTR		   pszTemplate;
    LPCDLGTEMPLATE	   pResource;
  }			 DUMMYUNIONNAME;

  _ANONYMOUS_UNION union
  { HICON		   hIcon;
    LPCSTR		   pszIcon;
  }			 DUMMYUNIONNAME2;

  LPCSTR		 pszTitle;
  DLGPROC		 pfnDlgProc;
  LPARAM		 lParam;
  UINT	(CALLBACK	*pfnCallback) (HWND, UINT, struct _PROPSHEETPAGEA *);
  UINT			*pcRefParent;

# if _WIN32_IE >= _WIN32_IE_IE40

  LPCSTR		 pszHeaderTitle;
  LPCSTR		 pszHeaderSubTitle;

# endif	/* _WIN32_IE >= _WIN32_IE_IE40 */

} PROPSHEETPAGEA, *LPPROPSHEETPAGEA;

#define LPCPROPSHEETPAGE  __AW_SUFFIXED__( LPCPROPSHEETPAGE )

typedef const PROPSHEETPAGEA *LPCPROPSHEETPAGEA;

typedef
struct _PROPSHEETPAGEW
{ DWORD 		 dwSize;
  DWORD 		 dwFlags;
  HINSTANCE		 hInstance;

  _ANONYMOUS_UNION union
  { LPCWSTR		   pszTemplate;
    LPCDLGTEMPLATE	   pResource;
  }			 DUMMYUNIONNAME;

  _ANONYMOUS_UNION union
  { HICON		   hIcon;
    LPCWSTR		   pszIcon;
  }			 DUMMYUNIONNAME2;

  LPCWSTR		 pszTitle;
  DLGPROC		 pfnDlgProc;
  LPARAM		 lParam;
  UINT	(CALLBACK	*pfnCallback) (HWND, UINT, struct _PROPSHEETPAGEW *);
  UINT			*pcRefParent;

# if _WIN32_IE >= _WIN32_IE_IE40

  LPCWSTR		 pszHeaderTitle;
  LPCWSTR		 pszHeaderSubTitle;

# endif	/* _WIN32_IE >= _WIN32_IE_IE40 */

} PROPSHEETPAGEW, *LPPROPSHEETPAGEW;

typedef const PROPSHEETPAGEW *LPCPROPSHEETPAGEW;

#define LPFNPSPCALLBACK  __AW_SUFFIXED__( LPFNPSPCALLBACK )

typedef UINT (CALLBACK *LPFNPSPCALLBACKA) (HWND, UINT, LPPROPSHEETPAGEA);
typedef UINT (CALLBACK *LPFNPSPCALLBACKW) (HWND, UINT, LPPROPSHEETPAGEW);

typedef int (CALLBACK *PFNPROPSHEETCALLBACK) (HWND, UINT, LPARAM);

DECLARE_HANDLE (HPROPSHEETPAGE);

#define PROPSHEETHEADER    __AW_SUFFIXED__( PROPSHEETHEADER )
#define LPPROPSHEETHEADER  __AW_SUFFIXED__( LPPROPSHEETHEADER )

typedef
struct _PROPSHEETHEADERA
{ DWORD 		 dwSize;
  DWORD 		 dwFlags;
  HWND			 hwndParent;
  HINSTANCE		 hInstance;

  _ANONYMOUS_UNION union
  { HICON		   hIcon;
    LPCSTR		   pszIcon;
  }			 DUMMYUNIONNAME;

  LPCSTR		 pszCaption;
  UINT			 nPages;

  _ANONYMOUS_UNION union
  { UINT		   nStartPage;
    LPCSTR		   pStartPage;
  }			 DUMMYUNIONNAME2;

  _ANONYMOUS_UNION union
  { LPCPROPSHEETPAGEA	   ppsp;
    HPROPSHEETPAGE	  *phpage;
  }			 DUMMYUNIONNAME3;

  PFNPROPSHEETCALLBACK	 pfnCallback;

# if _WIN32_IE >= _WIN32_IE_IE40

  _ANONYMOUS_UNION union
  { HBITMAP		   hbmWatermark;
    LPCSTR		   pszbmWatermark;
  }			 DUMMYUNIONNAME4;

  HPALETTE		 hplWatermark;

  _ANONYMOUS_UNION union
  { HBITMAP		   hbmHeader;
    LPCSTR		   pszbmHeader;
  }			 DUMMYUNIONNAME5;

# endif	/* _WIN32_IE >= _WIN32_IE_IE40 */

} PROPSHEETHEADERA, *LPPROPSHEETHEADERA;

typedef
struct _PROPSHEETHEADERW
{ DWORD 		 dwSize;
  DWORD 		 dwFlags;
  HWND			 hwndParent;
  HINSTANCE		 hInstance;

  _ANONYMOUS_UNION union
  { HICON		   hIcon;
    LPCWSTR		   pszIcon;
  }			 DUMMYUNIONNAME;

  LPCWSTR		 pszCaption;
  UINT			 nPages;

  _ANONYMOUS_UNION union
  { UINT		   nStartPage;
    LPCWSTR		   pStartPage;
  }			 DUMMYUNIONNAME2;

  _ANONYMOUS_UNION union
  { LPCPROPSHEETPAGEW	   ppsp;
    HPROPSHEETPAGE	  *phpage;
  }			 DUMMYUNIONNAME3;

  PFNPROPSHEETCALLBACK	 pfnCallback;

# if _WIN32_IE >= _WIN32_IE_IE40

  _ANONYMOUS_UNION union
  { HBITMAP		   hbmWatermark;
    LPCWSTR		   pszbmWatermark;
  }			 DUMMYUNIONNAME4;

  HPALETTE		 hplWatermark;

  _ANONYMOUS_UNION union
  { HBITMAP		   hbmHeader;
    LPCWSTR		   pszbmHeader;
  }			 DUMMYUNIONNAME5;

# endif	/* _WIN32_IE >= _WIN32_IE_IE40 */

} PROPSHEETHEADERW, *LPPROPSHEETHEADERW;

#define LPCPROPSHEETHEADER  __AW_SUFFIXED__( LPCPROPSHEETHEADER )

typedef const PROPSHEETHEADERA *LPCPROPSHEETHEADERA;
typedef const PROPSHEETHEADERW *LPCPROPSHEETHEADERW;

typedef BOOL (CALLBACK *LPFNADDPROPSHEETPAGE) (HPROPSHEETPAGE, LPARAM);

typedef BOOL (CALLBACK *LPFNADDPROPSHEETPAGES)
( LPVOID, LPFNADDPROPSHEETPAGE, LPARAM );

typedef
struct _PSHNOTIFY
{ NMHDR 		 hdr;
  LPARAM		 lParam;
} PSHNOTIFY, *LPPSHNOTIFY;

#pragma pack (pop)

#define PropertySheet __AW_SUFFIXED__( PropertySheet )

WINAPI int PropertySheetA (LPCPROPSHEETHEADERA);
WINAPI int PropertySheetW (LPCPROPSHEETHEADERW);

#define CreatePropertySheetPage __AW_SUFFIXED__( CreatePropertySheetPage )

WINAPI HPROPSHEETPAGE CreatePropertySheetPageA (LPCPROPSHEETPAGEA);
WINAPI HPROPSHEETPAGE CreatePropertySheetPageW (LPCPROPSHEETPAGEW);

WINAPI BOOL DestroyPropertySheetPage (HPROPSHEETPAGE);

#define PropSheet_AddPage( d, p )					\
  SNDMSG ((d), PSM_ADDPAGE, 0, (LPARAM)(p))

#define PropSheet_Apply( d )						\
  SNDMSG ((d), PSM_APPLY, 0, 0)

#define PropSheet_CancelToClose( d )					\
  POSTMSG ((d), PSM_CANCELTOCLOSE, 0, 0)

#define PropSheet_Changed( d, w )					\
  SNDMSG ((d), PSM_CHANGED, (WPARAM)(w), 0)

#define PropSheet_GetCurrentPageHwnd( d )				\
  (HWND)(SNDMSG ((d), PSM_GETCURRENTPAGEHWND, 0, 0))

#define PropSheet_GetTabControl( d )					\
  (HWND)(SNDMSG ((d), PSM_GETTABCONTROL, 0, 0))

#define PropSheet_IsDialogMessage( d, m )				\
  (BOOL)(SNDMSG ((d), PSM_ISDIALOGMESSAGE, 0, (LPARAM)(m)))

#define PropSheet_PressButton( d, i )					\
  POSTMSG ((d), PSM_PRESSBUTTON, (i), 0)

#define PropSheet_QuerySiblings( d, w, l )				\
  SNDMSG ((d), PSM_QUERYSIBLINGS, (w), (l))

#define PropSheet_RebootSystem( d )					\
  SNDMSG ((d), PSM_REBOOTSYSTEM, 0, 0)

#define PropSheet_RemovePage( d, i, p ) 				\
  SNDMSG ((d), PSM_REMOVEPAGE, (i), (LPARAM)(p))

#define PropSheet_RestartWindows( d )					\
  SNDMSG ((d), PSM_RESTARTWINDOWS, 0, 0)

#define PropSheet_SetCurSel( d, p, i )					\
  SNDMSG ((d), PSM_SETCURSEL, (i), (LPARAM)(p))

#define PropSheet_SetCurSelByID( d, i ) 				\
  SNDMSG ((d), PSM_SETCURSELID, 0, (i))

#define PropSheet_SetFinishText( d, s ) 				\
  SNDMSG ((d), PSM_SETFINISHTEXT, 0, (LPARAM)(s))

#define PropSheet_SetTitle( d, w, s )					\
  SNDMSG ((d), PSM_SETTITLE, (w), (LPARAM)(s))

#define PropSheet_SetWizButtons( d, f ) 				\
  POSTMSG ((d), PSM_SETWIZBUTTONS, 0, (LPARAM)(f))

#define PropSheet_UnChanged( d, w )					\
  SNDMSG ((d), PSM_UNCHANGED, (WPARAM)(w), 0)

#endif	/* ! RC_INVOKED */

_END_C_DECLS

#endif	/* !_PRSHT_H: $RCSfile: prsht.h,v $: end of file */
