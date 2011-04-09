/*BEGIN_LEGAL 
Intel Open Source License 

Copyright (c) 2002-2011 Intel Corporation. All rights reserved.
 
Redistribution and use in source and binary forms, with or without
modification, are permitted provided that the following conditions are
met:

Redistributions of source code must retain the above copyright notice,
this list of conditions and the following disclaimer.  Redistributions
in binary form must reproduce the above copyright notice, this list of
conditions and the following disclaimer in the documentation and/or
other materials provided with the distribution.  Neither the name of
the Intel Corporation nor the names of its contributors may be used to
endorse or promote products derived from this software without
specific prior written permission.
 
THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
``AS IS'' AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE INTEL OR
ITS CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
END_LEGAL */
// <ORIGINAL-AUTHOR>: Greg Lueck
// <COMPONENT>: fund
// <FILE-TYPE>: component public header

#ifndef FUND_HPP
#define FUND_HPP

#if defined(__GNUC__)
#   include <stdint.h>
#endif
#include "fund/config.h"


/*!
 * The FUND namespace provides a small set of fundamental types that are used
 * by many other components.
 *
 * The build system may define the following macros.  In some installations, these
 * might be pre-defined, in which case it is not necessary for the build system to
 * define them.
 *
 *      FUND_TC_HOSTCPU
 *          Defined to one of FUND_CPU_IA32, FUND_CPU_INTEL64, FUND_CPU_IA64
 *      FUND_TC_HOSTOS
 *          Defined to one of FUND_OS_LINUX, FUND_OS_WINDOWS, FUND_OS_MAC
 *      FUND_TC_TARGETCPU
 *          Defined to one of FUND_CPU_NONE, FUND_CPU_IA32, FUND_CPU_INTEL64, FUND_CPU_IA64
 *      FUND_TC_TARGETOS
 *          Defined to one of FUND_OS_NONE, FUND_OS_LINUX, FUND_OS_WINDOWS, FUND_OS_MAC
 */
namespace FUND {

#if defined(_MSC_VER)

// Microsoft Visual C/C++ compiler

typedef unsigned __int8 UINT8;      ///< 8-bit unsigned integer
typedef unsigned __int16 UINT16;    ///< 16-bit unsigned integer
typedef unsigned __int32 UINT32;    ///< 32-bit unsigned integer
typedef unsigned __int64 UINT64;    ///< 64-bit unsigned integer
typedef __int8 INT8;                ///< 8-bit signed integer
typedef __int16 INT16;              ///< 16-bit signed integer
typedef __int32 INT32;              ///< 32-bit signed integer
typedef __int64 INT64;              ///< 64-bit signed integer

/*!
 * Use this when defining a symbol that should be exported from a DLL.
 */
#define FUND_EXPORT __declspec(dllexport)

/*!
 * Minimum alignment required by a type (t).
 */
#define FUND_ALIGNMENT_OF(t) __alignof(t)

#elif defined(__GNUC__)

// GNU C/C++ compiler

typedef uint8_t  UINT8;             ///< 8-bit unsigned integer
typedef uint16_t UINT16;            ///< 16-bit unsigned integer
typedef uint32_t UINT32;            ///< 32-bit unsigned integer
typedef uint64_t UINT64;            ///< 64-bit unsigned integer
typedef int8_t  INT8;               ///< 8-bit signed integer
typedef int16_t INT16;              ///< 16-bit signed integer
typedef int32_t INT32;              ///< 32-bit signed integer
typedef int64_t INT64;              ///< 64-bit signed integer

/*!
 * Use this when defining a symbol that should be exported from a DLL.
 */
#define FUND_EXPORT

/*!
 * Minimum alignment required by a type (t).
 */
#define FUND_ALIGNMENT_OF(t) __alignof__(t)

#endif


#if defined(FUND_HOST_IA32)

/*!
 * Unsigned integer of the same size as a pointer on the host system
 * (the system where the program runs).  Conversions to/from PTRINT and
 * pointers do not lose data.
 */
typedef UINT32 PTRINT;
#define FUND_PTRINT_SIZE 32

#elif defined(FUND_HOST_INTEL64) || defined(FUND_HOST_IA64)

/*!
 * Unsigned integer of the same size as a pointer on the host system
 * (the system where the program runs).  Conversions to/from PTRINT and
 * pointers do not lose data.
 */
typedef UINT64 PTRINT;
#define FUND_PTRINT_SIZE 64

#endif


#if defined(FUND_TARGET_IA32)

/*!
 * Unsigned integer of the same size as a pointer on the target system.
 * The concept of "target" system makes sense only for programs that analyze or
 * generate code.  Consider a cross-compiler as an example that runs on system
 * X and generates code for system Y.  In this example, PTRINT represents a pointer
 * on system X (the host system) and ADDRINT represents a pointer on system Y (the
 * target system).
 */
typedef UINT32  ADDRINT;
#define FUND_ADDRINT_SIZE 32

#elif defined(FUND_TARGET_INTEL64) || defined(FUND_TARGET_IA64)

/*!
 * Unsigned integer of the same size as a pointer on the target system.
 * The concept of "target" system makes sense only for programs that analyze or
 * generate code.  Consider a cross-compiler as an example that runs on system
 * X and generates code for system Y.  In this example, PTRINT represents a pointer
 * on system X (the host system) and ADDRINT represents a pointer on system Y (the
 * target system).
 */
typedef UINT64  ADDRINT;
#define FUND_ADDRINT_SIZE 64

#endif

} // namespace
#endif // file guard
