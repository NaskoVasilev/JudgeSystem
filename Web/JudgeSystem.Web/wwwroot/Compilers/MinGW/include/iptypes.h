/*
 * iptypes.h
 *
 * Internet Protocol Type Definitions and Manifest Constants
 *
 *
 * $Id: iptypes.h,v 0e0d248ed898 2018/02/24 15:41:13 keith $
 *
 * Written by Corinna Vinschen <corinna@vinschen.de>
 * Copyright (C) 2000-2002, 2006, 2018, MinGW.org Project
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
#ifndef _IPTYPES_H
#pragma GCC system_header
#define _IPTYPES_H

#include <windef.h>
#define __need_time_t  1
#include <sys/types.h>
#undef __need_time_t

_BEGIN_C_DECLS

#define DEFAULT_MINIMUM_ENTITIES			32

#define MAX_ADAPTER_ADDRESS_LENGTH			 8
#define MAX_ADAPTER_DESCRIPTION_LENGTH		       128
#define MAX_ADAPTER_NAME_LENGTH 		       256
#define MAX_DOMAIN_NAME_LEN			       128
#define MAX_HOSTNAME_LEN			       128
#define MAX_SCOPE_ID_LEN			       256

#define BROADCAST_NODETYPE				 1
#define PEER_TO_PEER_NODETYPE				 2
#define MIXED_NODETYPE					 4
#define HYBRID_NODETYPE 				 8
#define IF_OTHER_ADAPTERTYPE				 0
#define IF_ETHERNET_ADAPTERTYPE 			 1
#define IF_TOKEN_RING_ADAPTERTYPE			 2
#define IF_FDDI_ADAPTERTYPE				 3
#define IF_PPP_ADAPTERTYPE				 4
#define IF_LOOPBACK_ADAPTERTYPE 			 5

#if _WIN32_WINNT >= _WIN32_WINNT_WINXP
#define IP_ADAPTER_DDNS_ENABLED 		0x00000001
#define IP_ADAPTER_REGISTER_ADAPTER_SUFFIX	0x00000002
#define IP_ADAPTER_DHCP_ENABLED 		0x00000004
#define IP_ADAPTER_RECEIVE_ONLY 		0x00000008
#define IP_ADAPTER_NO_MULTICAST 		0x00000010
#define IP_ADAPTER_IPV6_OTHER_STATEFUL_CONFIG	0x00000020
#define IP_ADAPTER_ADDRESS_DNS_ELIGIBLE 	0x00000001
#define IP_ADAPTER_ADDRESS_TRANSIENT		0x00000002
#endif

typedef struct { char String[16]; }
IP_ADDRESS_STRING, *PIP_ADDRESS_STRING, IP_MASK_STRING, *PIP_MASK_STRING;

typedef struct _IP_ADDR_STRING
{ struct _IP_ADDR_STRING		*Next;
  IP_ADDRESS_STRING			 IpAddress;
  IP_MASK_STRING			 IpMask;
  DWORD 				 Context;
} IP_ADDR_STRING, *PIP_ADDR_STRING;

typedef struct _IP_ADAPTER_INFO
/* This structure includes a pair of fields, which are declared, according to
 * https://msdn.microsoft.com/en-us/library/windows/desktop/aa366062(v=vs.85).aspx
 * as being of type time_t, yet they MUST be equivalent to __time32_t on 32-bit
 * versions or MS-Windows, and to __time64_t on 64-bit versions.  Since VS-2005,
 * Microsoft have made the definition of time_t ambiguous, on 32-bit Windows,
 * and have placed the onus on users to specify _USE_32_BIT_TIME_T in any
 * translation unit which uses this structure, to resolve the ambiguity;
 * WE can do better...
 */
#undef __dhcp_time_t
#ifdef _WIN64
/* ...by introducing a local type replacement macro, to ensure that the
 * field definitions become unequivocally 64-bit on Win64...
 */
# define __dhcp_time_t  __time64_t
#else
/* ...and 32-bit, on Win32.
 */
# define __dhcp_time_t  __time32_t
#endif
{ struct _IP_ADAPTER_INFO	*Next;
  DWORD 			 ComboIndex;
  char				 AdapterName[MAX_ADAPTER_NAME_LENGTH + 4];
  char				 Description[MAX_ADAPTER_DESCRIPTION_LENGTH + 4];
  UINT				 AddressLength;
  BYTE				 Address[MAX_ADAPTER_ADDRESS_LENGTH];
  DWORD 			 Index;
  UINT				 Type;
  UINT				 DhcpEnabled;
  PIP_ADDR_STRING		 CurrentIpAddress;
  IP_ADDR_STRING		 IpAddressList;
  IP_ADDR_STRING		 GatewayList;
  IP_ADDR_STRING		 DhcpServer;
  BOOL				 HaveWins;
  IP_ADDR_STRING		 PrimaryWinsServer;
  IP_ADDR_STRING		 SecondaryWinsServer;
  __dhcp_time_t 		 LeaseObtained;
  __dhcp_time_t 		 LeaseExpires;
} IP_ADAPTER_INFO, *PIP_ADAPTER_INFO;
#undef __dhcp_time_t

typedef struct _IP_PER_ADAPTER_INFO
{ UINT				 AutoconfigEnabled;
  UINT				 AutoconfigActive;
  PIP_ADDR_STRING		 CurrentDnsServer;
  IP_ADDR_STRING		 DnsServerList;
} IP_PER_ADAPTER_INFO, *PIP_PER_ADAPTER_INFO;

typedef struct _FIXED_INFO
{ char				 HostName[MAX_HOSTNAME_LEN + 4] ;
  char				 DomainName[MAX_DOMAIN_NAME_LEN + 4];
  PIP_ADDR_STRING		 CurrentDnsServer;
  IP_ADDR_STRING		 DnsServerList;
  UINT				 NodeType;
  char				 ScopeId[MAX_SCOPE_ID_LEN + 4];
  UINT				 EnableRouting;
  UINT				 EnableProxy;
  UINT				 EnableDns;
} FIXED_INFO, *PFIXED_INFO;

#if (_WIN32_WINNT >= _WIN32_WINNT_WINXP) && defined _WINSOCK2_H

typedef enum
{ IfOperStatusUp		=	 1,
  IfOperStatusDown,
  IfOperStatusTesting,
  IfOperStatusUnknown,
  IfOperStatusDormant,
  IfOperStatusNotPresent,
  IfOperStatusLowerLayerDown
} IF_OPER_STATUS;

typedef enum
{ IpDadStateInvalid		=	 0,
  IpDadStateTentative,
  IpDadStateDuplicate,
  IpDadStateDeprecated,
  IpDadStatePreferred
} IP_DAD_STATE;

typedef enum
{ IpPrefixOriginOther		=	 0,
  IpPrefixOriginManual,
  IpPrefixOriginWellKnown,
  IpPrefixOriginDhcp,
  IpPrefixOriginRouterAdvertisement
} IP_PREFIX_ORIGIN;

typedef enum
{ IpSuffixOriginOther		=	 0,
  IpSuffixOriginManual,
  IpSuffixOriginWellKnown,
  IpSuffixOriginDhcp,
  IpSuffixOriginLinkLayerAddress,
  IpSuffixOriginRandom
} IP_SUFFIX_ORIGIN;

typedef enum
{ ScopeLevelInterface		=	 1,
  ScopeLevelLink		=	 2,
  ScopeLevelSubnet		=	 3,
  ScopeLevelAdmin		=	 4,
  ScopeLevelSite		=	 5,
  ScopeLevelOrganization	=	 8,
  ScopeLevelGlobal		=	14
} SCOPE_LEVEL;

typedef struct
{ ULONG 				 Index;
  ULONG 				 MediaType;
  UCHAR 				 ConnectionType;
  UCHAR 				 AccessType;
  GUID					 DeviceGuid;
  GUID					 InterfaceGuid;
} IP_INTERFACE_NAME_INFO, *PIP_INTERFACE_NAME_INFO;

typedef struct _IP_ADAPTER_ANYCAST_ADDRESS
{ _ANONYMOUS_UNION union
  { ULONGLONG				   Alignment;
    _ANONYMOUS_STRUCT struct
    { ULONG				     Length;
      DWORD				     Flags;
    }				 	   DUMMYSTRUCTNAME;
  }					 DUMMYUNIONNAME;
  struct _IP_ADAPTER_ANYCAST_ADDRESS	*Next;
  SOCKET_ADDRESS			 Address;
} IP_ADAPTER_ANYCAST_ADDRESS, *PIP_ADAPTER_ANYCAST_ADDRESS;

typedef struct _IP_ADAPTER_MULTICAST_ADDRESS
{ _ANONYMOUS_UNION union
  { ULONGLONG				   Alignment;
    _ANONYMOUS_STRUCT struct
    { ULONG				     Length;
      DWORD				     Flags;
    }					   DUMMYSTRUCTNAME;
  }					 DUMMYUNIONNAME;
  struct _IP_ADAPTER_MULTICAST_ADDRESS	*Next;
  SOCKET_ADDRESS			 Address;
} IP_ADAPTER_MULTICAST_ADDRESS, *PIP_ADAPTER_MULTICAST_ADDRESS;

typedef struct _IP_ADAPTER_UNICAST_ADDRESS
{ _ANONYMOUS_UNION union
  { ULONGLONG				   Alignment;
    _ANONYMOUS_STRUCT struct
    { ULONG				     Length;
      DWORD				     Flags;
    }					   DUMMYSTRUCTNAME;
  }					 DUMMYUNIONNAME;
  struct _IP_ADAPTER_UNICAST_ADDRESS	*Next;
  SOCKET_ADDRESS			 Address;
  IP_PREFIX_ORIGIN			 PrefixOrigin;
  IP_SUFFIX_ORIGIN			 SuffixOrigin;
  IP_DAD_STATE				 DadState;
  ULONG 				 ValidLifetime;
  ULONG 				 PreferredLifetime;
  ULONG 				 LeaseLifetime;
} IP_ADAPTER_UNICAST_ADDRESS, *PIP_ADAPTER_UNICAST_ADDRESS;

typedef struct _IP_ADAPTER_DNS_SERVER_ADDRESS
{ _ANONYMOUS_UNION union
  { ULONGLONG				   Alignment;
    _ANONYMOUS_STRUCT struct
    { ULONG				     Length;
      DWORD				     Reserved;
    }					   DUMMYSTRUCTNAME;
  }					 DUMMYUNIONNAME;
  struct _IP_ADAPTER_DNS_SERVER_ADDRESS *Next;
  SOCKET_ADDRESS			 Address;
} IP_ADAPTER_DNS_SERVER_ADDRESS, *PIP_ADAPTER_DNS_SERVER_ADDRESS;

typedef struct _IP_ADAPTER_PREFIX
{ _ANONYMOUS_UNION union
  { ULONGLONG				   Alignment;
    _ANONYMOUS_STRUCT struct
    { ULONG				     Length;
      DWORD				     Flags;
    }					   DUMMYSTRUCTNAME;
  }					 DUMMYUNIONNAME;
  struct _IP_ADAPTER_PREFIX		*Next;
  SOCKET_ADDRESS			 Address;
  ULONG 				 PrefixLength;
} IP_ADAPTER_PREFIX, *PIP_ADAPTER_PREFIX;

typedef struct _IP_ADAPTER_ADDRESSES
{ _ANONYMOUS_UNION union
  { ULONGLONG				   Alignment;
    _ANONYMOUS_STRUCT struct
    { ULONG				     Length;
      DWORD				     IfIndex;
    }					   DUMMYSTRUCTNAME;
  }					 DUMMYUNIONNAME;
  struct _IP_ADAPTER_ADDRESSES		*Next;
  PCHAR 				 AdapterName;
  PIP_ADAPTER_UNICAST_ADDRESS		 FirstUnicastAddress;
  PIP_ADAPTER_ANYCAST_ADDRESS		 FirstAnycastAddress;
  PIP_ADAPTER_MULTICAST_ADDRESS 	 FirstMulticastAddress;
  PIP_ADAPTER_DNS_SERVER_ADDRESS	 FirstDnsServerAddress;
  PWCHAR				 DnsSuffix;
  PWCHAR				 Description;
  PWCHAR				 FriendlyName;
  BYTE					 PhysicalAddress[MAX_ADAPTER_ADDRESS_LENGTH];
  DWORD 				 PhysicalAddressLength;
  DWORD 				 Flags;
  DWORD 				 Mtu;
  DWORD 				 IfType;
  IF_OPER_STATUS			 OperStatus;
  DWORD 				 Ipv6IfIndex;
  DWORD 				 ZoneIndices[16];
  PIP_ADAPTER_PREFIX			 FirstPrefix;
} IP_ADAPTER_ADDRESSES, *PIP_ADAPTER_ADDRESSES;

#endif	/* WinXP && _WINSOCK2_H */

_END_C_DECLS

#endif	/* !_IPTYPES_H: $RCSfile: iptypes.h,v $: end of file */
