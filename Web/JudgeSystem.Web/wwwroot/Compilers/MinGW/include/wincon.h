/*
 * wincon.h
 *
 * Windows console I/O declarations, and API function prototypes.
 *
 * $Id: wincon.h,v 9510284773f9 2018/10/29 23:36:08 keith $
 *
 * Written by Anders Norlander  <anorland@hem2.passagen.se>
 * Copyright (C) 1998, 1999, 2002-2006, 2009, 2011, 2016, 2018,
 *   MinGW.org Project.
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
#ifndef _WINCON_H
#pragma GCC system_header
#define _WINCON_H

#if _WIN32_WINNT < _WIN32_WINNT_VISTA
/* For effective self-containment, <wincon.h> is dependent on the core set
 * of Windows' custom type definitions; prior to Vista, it suffices to...
 */
# include <windef.h>
#else
/* ...but Vista and later introduce additional dependencies on <wingdi.h>;
 * this will indirectly include <windef.h> anyway, so it suffices to...
 */
# include <wingdi.h>
#endif

_BEGIN_C_DECLS

#define FOREGROUND_BLUE 			0x0001
#define FOREGROUND_GREEN			0x0002
#define FOREGROUND_RED				0x0004
#define FOREGROUND_INTENSITY			0x0008
#define BACKGROUND_BLUE 			0x0010
#define BACKGROUND_GREEN			0x0020
#define BACKGROUND_RED				0x0040
#define BACKGROUND_INTENSITY			0x0080

#define COMMON_LVB_LEADING_BYTE 		0x0100
#define COMMON_LVB_TRAILING_BYTE		0x0200
#define COMMON_LVB_GRID_HORIZONTAL		0x0400
#define COMMON_LVB_GRID_LVERTICAL		0x0800
#define COMMON_LVB_GRID_RVERTICAL		0x1000
#define COMMON_LVB_REVERSE_VIDEO		0x4000
#define COMMON_LVB_UNDERSCORE			0x8000

#define CTRL_C_EVENT				0x0000
#define CTRL_BREAK_EVENT			0x0001
#define CTRL_CLOSE_EVENT			0x0002
#define CTRL_LOGOFF_EVENT			0x0005
#define CTRL_SHUTDOWN_EVENT			0x0006

#define ENABLE_LINE_INPUT			0x0002
#define ENABLE_ECHO_INPUT			0x0004
#define ENABLE_PROCESSED_INPUT			0x0001
#define ENABLE_WINDOW_INPUT			0x0008
#define ENABLE_MOUSE_INPUT			0x0010
#define ENABLE_INSERT_MODE			0x0020
#define ENABLE_QUICK_EDIT_MODE			0x0040
#define ENABLE_EXTENDED_FLAGS			0x0080
#define ENABLE_AUTO_POSITION			0x0100
#define ENABLE_VIRTUAL_TERMINAL_INPUT		0x0200
#define ENABLE_PROCESSED_OUTPUT 		0x0001
#define ENABLE_WRAP_AT_EOL_OUTPUT		0x0002
#define ENABLE_VIRTUAL_TERMINAL_PROCESSING	0x0004
#define DISABLE_NEWLINE_AUTO_RETURN		0x0008
#define ENABLE_LVB_GRID_WORLDWIDE		0x0010

#define KEY_EVENT				0x0001
#define MOUSE_EVENT				0x0002
#define WINDOW_BUFFER_SIZE_EVENT		0x0004
#define MENU_EVENT				0x0008
#define FOCUS_EVENT				0x0010
#define CAPSLOCK_ON				0x0080
#define ENHANCED_KEY				0x0100

#define RIGHT_ALT_PRESSED			0x0001
#define LEFT_ALT_PRESSED			0x0002
#define RIGHT_CTRL_PRESSED			0x0004
#define LEFT_CTRL_PRESSED			0x0008
#define SHIFT_PRESSED				0x0010
#define NUMLOCK_ON				0x0020
#define SCROLLLOCK_ON				0x0040

#define FROM_LEFT_1ST_BUTTON_PRESSED		0x0001
#define RIGHTMOST_BUTTON_PRESSED		0x0002
#define FROM_LEFT_2ND_BUTTON_PRESSED		0x0004
#define FROM_LEFT_3RD_BUTTON_PRESSED		0x0008
#define FROM_LEFT_4TH_BUTTON_PRESSED		0x0010
#define MOUSE_MOVED				0x0001
#define DOUBLE_CLICK				0x0002
#define MOUSE_WHEELED				0x0004
#define MOUSE_HWHEELED				0x0008

typedef
struct _CHAR_INFO
{ union
  { WCHAR			  UnicodeChar;
    CHAR			  AsciiChar;
  }				Char;
  WORD				Attributes;
} CHAR_INFO, *PCHAR_INFO;

typedef
struct _SMALL_RECT
{ SHORT 			Left;
  SHORT 			Top;
  SHORT 			Right;
  SHORT 			Bottom;
} SMALL_RECT, *PSMALL_RECT;

typedef
struct _CONSOLE_CURSOR_INFO
{ DWORD 			dwSize;
  BOOL				bVisible;
} CONSOLE_CURSOR_INFO, *PCONSOLE_CURSOR_INFO;

typedef
struct _COORD
{ SHORT 			X;
  SHORT 			Y;
} COORD, *PCOORD;

typedef
struct _CONSOLE_SCREEN_BUFFER_INFO
{ COORD 			dwSize;
  COORD 			dwCursorPosition;
  WORD				wAttributes;
  SMALL_RECT			srWindow;
  COORD 			dwMaximumWindowSize;
} CONSOLE_SCREEN_BUFFER_INFO, *PCONSOLE_SCREEN_BUFFER_INFO;

typedef BOOL (CALLBACK *PHANDLER_ROUTINE)(DWORD);

typedef
struct _KEY_EVENT_RECORD
#ifdef __GNUC__  /* gcc's alignment is not what Win32 expects here! */
# define __MINGW_ATTRIBUTE_PACKED__ __attribute__((packed))
#else
# define __MINGW_ATTRIBUTE_PACKED__
#endif
{ BOOL				bKeyDown;
  WORD				wRepeatCount;
  WORD				wVirtualKeyCode;
  WORD				wVirtualScanCode;
  union
  { WCHAR			  UnicodeChar;
    CHAR			  AsciiChar;
  }				uChar;
  DWORD 			dwControlKeyState;
} __MINGW_ATTRIBUTE_PACKED__ KEY_EVENT_RECORD;

typedef
struct _MOUSE_EVENT_RECORD
{ COORD 			dwMousePosition;
  DWORD 			dwButtonState;
  DWORD 			dwControlKeyState;
  DWORD 			dwEventFlags;
} MOUSE_EVENT_RECORD;

typedef
struct _WINDOW_BUFFER_SIZE_RECORD
{ COORD 			dwSize;
} WINDOW_BUFFER_SIZE_RECORD;

typedef
struct _MENU_EVENT_RECORD
{ UINT				dwCommandId;
} MENU_EVENT_RECORD, *PMENU_EVENT_RECORD;

typedef
struct _FOCUS_EVENT_RECORD
{ BOOL				bSetFocus;
} FOCUS_EVENT_RECORD;

typedef
struct _INPUT_RECORD
{ WORD				EventType;
  union
  { KEY_EVENT_RECORD		  KeyEvent;
    MOUSE_EVENT_RECORD		  MouseEvent;
    WINDOW_BUFFER_SIZE_RECORD	  WindowBufferSizeEvent;
    MENU_EVENT_RECORD		  MenuEvent;
    FOCUS_EVENT_RECORD		  FocusEvent;
  }				Event;
} INPUT_RECORD, *PINPUT_RECORD;

WINAPI BOOL AllocConsole (void);
WINAPI HANDLE CreateConsoleScreenBuffer (DWORD, DWORD, CONST SECURITY_ATTRIBUTES *, DWORD, LPVOID);
WINAPI BOOL FillConsoleOutputAttribute (HANDLE, WORD, DWORD, COORD, PDWORD);

#define FillConsoleOutputCharacter __AW_SUFFIXED__(FillConsoleOutputCharacter)
WINAPI BOOL FillConsoleOutputCharacterA (HANDLE, CHAR, DWORD, COORD, PDWORD);
WINAPI BOOL FillConsoleOutputCharacterW (HANDLE, WCHAR, DWORD, COORD, PDWORD);

WINAPI BOOL FlushConsoleInputBuffer (HANDLE);
WINAPI BOOL FreeConsole (void);
WINAPI BOOL GenerateConsoleCtrlEvent (DWORD, DWORD);

#define GetConsoleAlias __AW_SUFFIXED__(GetConsoleAlias)
WINAPI DWORD GetConsoleAliasA (LPSTR, LPSTR, DWORD, LPSTR);
WINAPI DWORD GetConsoleAliasW (LPWSTR, LPWSTR, DWORD, LPWSTR);

WINAPI UINT GetConsoleCP (void);
WINAPI BOOL GetConsoleCursorInfo (HANDLE, PCONSOLE_CURSOR_INFO);
WINAPI BOOL GetConsoleMode (HANDLE, PDWORD);
WINAPI UINT GetConsoleOutputCP (void);
WINAPI BOOL GetConsoleScreenBufferInfo (HANDLE, PCONSOLE_SCREEN_BUFFER_INFO);

#define GetConsoleTitle __AW_SUFFIXED__(GetConsoleTitle)
WINAPI DWORD GetConsoleTitleA (LPSTR, DWORD);
WINAPI DWORD GetConsoleTitleW (LPWSTR, DWORD);

WINAPI COORD GetLargestConsoleWindowSize (HANDLE);
WINAPI BOOL GetNumberOfConsoleInputEvents (HANDLE, PDWORD);
WINAPI BOOL GetNumberOfConsoleMouseButtons (PDWORD);
WINAPI BOOL HandlerRoutine (DWORD);

#define PeekConsoleInput __AW_SUFFIXED__(PeekConsoleInput)
WINAPI BOOL PeekConsoleInputA (HANDLE, PINPUT_RECORD, DWORD, PDWORD);
WINAPI BOOL PeekConsoleInputW (HANDLE, PINPUT_RECORD, DWORD, PDWORD);

#define ReadConsole __AW_SUFFIXED__(ReadConsole)
WINAPI BOOL ReadConsoleA (HANDLE, PVOID, DWORD, PDWORD, PVOID);
WINAPI BOOL ReadConsoleW (HANDLE, PVOID, DWORD, PDWORD, PVOID);

#define ReadConsoleInput __AW_SUFFIXED__(ReadConsoleInput)
WINAPI BOOL ReadConsoleInputA (HANDLE, PINPUT_RECORD, DWORD, PDWORD);
WINAPI BOOL ReadConsoleInputW (HANDLE, PINPUT_RECORD, DWORD, PDWORD);

WINAPI BOOL ReadConsoleOutputAttribute (HANDLE, LPWORD, DWORD, COORD, LPDWORD);

#define ReadConsoleOutputCharacter __AW_SUFFIXED__(ReadConsoleOutputCharacter)
WINAPI BOOL ReadConsoleOutputCharacterA (HANDLE, LPSTR, DWORD, COORD, PDWORD);
WINAPI BOOL ReadConsoleOutputCharacterW (HANDLE, LPWSTR, DWORD, COORD, PDWORD);

#define ReadConsoleOutput __AW_SUFFIXED__(ReadConsoleOutput)
WINAPI BOOL ReadConsoleOutputA (HANDLE, PCHAR_INFO, COORD, COORD, PSMALL_RECT);
WINAPI BOOL ReadConsoleOutputW (HANDLE, PCHAR_INFO, COORD, COORD, PSMALL_RECT);

#define ScrollConsoleScreenBuffer __AW_SUFFIXED__(ScrollConsoleScreenBuffer)
WINAPI BOOL ScrollConsoleScreenBufferA
(HANDLE, const SMALL_RECT *, const SMALL_RECT *, COORD, const CHAR_INFO *);
WINAPI BOOL ScrollConsoleScreenBufferW
(HANDLE, const SMALL_RECT *, const SMALL_RECT *, COORD, const CHAR_INFO *);

WINAPI BOOL SetConsoleActiveScreenBuffer (HANDLE);
WINAPI BOOL SetConsoleCP (UINT);
WINAPI BOOL SetConsoleCtrlHandler (PHANDLER_ROUTINE, BOOL);
WINAPI BOOL SetConsoleCursorInfo (HANDLE, const CONSOLE_CURSOR_INFO *);
WINAPI BOOL SetConsoleCursorPosition (HANDLE, COORD);
WINAPI BOOL SetConsoleMode (HANDLE, DWORD);
WINAPI BOOL SetConsoleOutputCP (UINT);
WINAPI BOOL SetConsoleScreenBufferSize (HANDLE, COORD);
WINAPI BOOL SetConsoleTextAttribute (HANDLE, WORD);

#define SetConsoleTitle __AW_SUFFIXED__(SetConsoleTitle)
WINAPI BOOL SetConsoleTitleA (LPCSTR);
WINAPI BOOL SetConsoleTitleW (LPCWSTR);

WINAPI BOOL SetConsoleWindowInfo (HANDLE, BOOL, const SMALL_RECT *);

#define WriteConsole __AW_SUFFIXED__(WriteConsole)
WINAPI BOOL WriteConsoleA (HANDLE, PCVOID, DWORD, PDWORD, PVOID);
WINAPI BOOL WriteConsoleW (HANDLE, PCVOID, DWORD, PDWORD, PVOID);

#define WriteConsoleInput __AW_SUFFIXED__(WriteConsoleInput)
WINAPI BOOL WriteConsoleInputA (HANDLE, const INPUT_RECORD *, DWORD, PDWORD);
WINAPI BOOL WriteConsoleInputW (HANDLE, const INPUT_RECORD *, DWORD, PDWORD);

#define WriteConsoleOutput __AW_SUFFIXED__(WriteConsoleOutput)
WINAPI BOOL WriteConsoleOutputA (HANDLE, const CHAR_INFO *, COORD, COORD, PSMALL_RECT);
WINAPI BOOL WriteConsoleOutputW (HANDLE, const CHAR_INFO *, COORD, COORD, PSMALL_RECT);

WINAPI BOOL WriteConsoleOutputAttribute (HANDLE, const WORD *, DWORD, COORD, PDWORD);

#define WriteConsoleOutputCharacter __AW_SUFFIXED__(WriteConsoleOutputCharacter)
WINAPI BOOL WriteConsoleOutputCharacterA (HANDLE, LPCSTR, DWORD, COORD, PDWORD);
WINAPI BOOL WriteConsoleOutputCharacterW (HANDLE, LPCWSTR, DWORD, COORD, PDWORD);

#if _WIN32_WINNT >= _WIN32_WINNT_WIN2K

#define CONSOLE_FULLSCREEN			0x0001
#define CONSOLE_FULLSCREEN_HARDWARE		0x0002

WINAPI BOOL GetConsoleDisplayMode (LPDWORD);
WINAPI COORD GetConsoleFontSize (HANDLE, DWORD);
WINAPI HWND GetConsoleWindow (void);

#if _WIN32_WINNT >= _WIN32_WINNT_WINXP

#define CONSOLE_FULLSCREEN_MODE 		0x0001
#define CONSOLE_WINDOWED_MODE			0x0002
#define CONSOLE_NO_SELECTION			0x0000
#define CONSOLE_SELECTION_IN_PROGRESS		0x0001
#define CONSOLE_SELECTION_NOT_EMPTY		0x0002
#define CONSOLE_MOUSE_SELECTION 		0x0004
#define CONSOLE_MOUSE_DOWN			0x0008

typedef
struct _CONSOLE_FONT_INFO
{ DWORD 			nFont;
  COORD 			dwFontSize;
} CONSOLE_FONT_INFO, *PCONSOLE_FONT_INFO;

typedef
struct _CONSOLE_SELECTION_INFO
{ DWORD 			dwFlags;
  COORD 			dwSelectionAnchor;
  SMALL_RECT			srSelection;
} CONSOLE_SELECTION_INFO, *PCONSOLE_SELECTION_INFO;

#define AddConsoleAlias __AW_SUFFIXED__(AddConsoleAlias)
WINAPI BOOL AddConsoleAliasA (LPCSTR, LPCSTR, LPCSTR);
WINAPI BOOL AddConsoleAliasW (LPCWSTR, LPCWSTR, LPCWSTR);

#define ATTACH_PARENT_PROCESS  ((DWORD)(-1))

WINAPI BOOL AttachConsole (DWORD);

#define GetConsoleAliases __AW_SUFFIXED__(GetConsoleAliases)
WINAPI DWORD GetConsoleAliasesA (LPSTR, DWORD, LPSTR);
WINAPI DWORD GetConsoleAliasesW (LPWSTR, DWORD, LPWSTR);

#define GetConsoleAliasExes __AW_SUFFIXED__(GetConsoleAliasExes)
WINAPI DWORD GetConsoleAliasExesA (LPSTR, DWORD);
WINAPI DWORD GetConsoleAliasExesW (LPWSTR, DWORD);

#define GetConsoleAliasesLength __AW_SUFFIXED__(GetConsoleAliasesLength)
WINAPI DWORD GetConsoleAliasesLengthA (LPSTR);
WINAPI DWORD GetConsoleAliasesLengthW (LPWSTR);

#define GetConsoleAliasExesLength __AW_SUFFIXED__(GetConsoleAliasExesLength)
WINAPI DWORD GetConsoleAliasExesLengthA (void);
WINAPI DWORD GetConsoleAliasExesLengthW (void);

WINAPI BOOL GetConsoleSelectionInfo (PCONSOLE_SELECTION_INFO);
WINAPI DWORD GetConsoleProcessList (LPDWORD, DWORD);
WINAPI BOOL GetCurrentConsoleFont (HANDLE, BOOL, PCONSOLE_FONT_INFO);
WINAPI BOOL SetConsoleDisplayMode (HANDLE, DWORD, PCOORD);

#if _WIN32_WINNT >= _WIN32_WINNT_VISTA

#define HISTORY_NO_DUP_FLAG			0x0001

typedef
struct _CONSOLE_FONT_INFOEX
{ ULONG 			cbSize;
  DWORD 			nFont;
  COORD 			dwFontSize;
  UINT				FontFamily;
  UINT				FontWeight;
  WCHAR 			FaceName[LF_FACESIZE];
} CONSOLE_FONT_INFOEX, *PCONSOLE_FONT_INFOEX;

typedef
struct
{ UINT				cbSize;
  UINT				HistoryBufferSize;
  UINT				NumberOfHistoryBuffers;
  DWORD 			dwFlags;
} CONSOLE_HISTORY_INFO, *PCONSOLE_HISTORY_INFO;

typedef
struct _CONSOLE_READCONSOLE_CONTROL
{ ULONG 			nLength;
  ULONG 			nInitialChars;
  ULONG 			dwCtrlWakeupMask;
  ULONG 			dwControlKeyState;
} CONSOLE_READCONSOLE_CONTROL, *PCONSOLE_READCONSOLE_CONTROL;

typedef
struct _CONSOLE_SCREEN_BUFFER_INFOEX
{ ULONG 			cbSize;
  COORD 			dwSize;
  COORD 			dwCursorPosition;
  WORD				wAttributes;
  SMALL_RECT			srWindow;
  COORD 			dwMaximumWindowSize;
  WORD				wPopupAttributes;
  BOOL				bFullscreenSupported;
  COLORREF			ColorTable[16];
} CONSOLE_SCREEN_BUFFER_INFOEX, *PCONSOLE_SCREEN_BUFFER_INFOEX;

WINAPI BOOL GetConsoleHistoryInfo (PCONSOLE_HISTORY_INFO);

#define GetConsoleOriginalTitle __AW_SUFFIXED__(GetConsoleOriginalTitle)
WINAPI DWORD GetConsoleOriginalTitleA (LPSTR, DWORD);
WINAPI DWORD GetConsoleOriginalTitleW (LPWSTR, DWORD);

WINAPI BOOL GetConsoleScreenBufferInfoEx (HANDLE, PCONSOLE_SCREEN_BUFFER_INFOEX);
WINAPI BOOL GetCurrentConsoleFontEx (HANDLE, BOOL, PCONSOLE_FONT_INFOEX);
WINAPI BOOL SetConsoleHistoryInfo (PCONSOLE_HISTORY_INFO);
WINAPI BOOL SetConsoleScreenBufferInfoEx (HANDLE, PCONSOLE_SCREEN_BUFFER_INFOEX);
WINAPI BOOL SetCurrentConsoleFontEx (HANDLE, BOOL, PCONSOLE_FONT_INFOEX);

#endif	/* Vista and later */
#endif	/* WinXP and later */
#endif	/* Win2K and later */

_END_C_DECLS

#endif	/* !_WINCON_H: $RCSfile: wincon.h,v $: end of file */
