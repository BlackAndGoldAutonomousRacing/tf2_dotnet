/* Copyright 2022 Stefan Hoffmann <stefan.hoffmann@schiller.de>
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Runtime.InteropServices;
using ROS2;

namespace ROS2.Tf2DotNet
{
    internal static class Interop
    {
        internal static readonly DllLoadUtils _dllLoadUtils;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate SafeBufferCoreHandle Tf2DotNetNativeBufferCoreCreateType(out Tf2ExceptionType exceptionType, byte[] exceptionMessageBuffer);
        internal static Tf2DotNetNativeBufferCoreCreateType tf2_dotnet_native_buffer_core_create = null;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void Tf2DotNetNativeBufferCoreDestroyType(IntPtr bufferCore, out Tf2ExceptionType exceptionType, byte[] exceptionMessageBuffer);
        internal static Tf2DotNetNativeBufferCoreDestroyType tf2_dotnet_native_buffer_core_destroy = null;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int Tf2DotNetNativeBufferCoreSetTransformType(
            SafeBufferCoreHandle bufferCore,
            int sec,
            uint nanosec,
            [MarshalAs(UnmanagedType.LPStr)] string frameId,
            [MarshalAs(UnmanagedType.LPStr)] string childFrameId,
            double translationX,
            double translationY,
            double translationZ,
            double rotationX,
            double rotationY,
            double rotationZ,
            double rotationW,
            [MarshalAs(UnmanagedType.LPStr)]
            string authority,
            int isStatic,
            out Tf2ExceptionType exceptionType,
            byte[] exceptionMessageBuffer);

        internal static Tf2DotNetNativeBufferCoreSetTransformType tf2_dotnet_native_buffer_core_set_transform = null;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate Transform Tf2DotNetNativeBufferCoreLookupTransformType(
            SafeBufferCoreHandle bufferCore,
            [MarshalAs(UnmanagedType.LPStr)] string targetFrame,
            [MarshalAs(UnmanagedType.LPStr)] string sourceFrame,
            int sec,
            uint nanosec,
            out Tf2ExceptionType exceptionType,
            byte[] exceptionMessageBuffer);

        internal static Tf2DotNetNativeBufferCoreLookupTransformType tf2_dotnet_native_buffer_core_lookup_transform = null;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate Transform Tf2DotNetNativeBufferCoreLookupTransformFullType(
            SafeBufferCoreHandle bufferCore,
            [MarshalAs(UnmanagedType.LPStr)] string targetFrame,
            int targetSec,
            uint targetNanosec,
            [MarshalAs(UnmanagedType.LPStr)] string sourceFrame,
            int sourceSec,
            uint sourceNanosec,
            [MarshalAs(UnmanagedType.LPStr)] string fixedFrame,
            out Tf2ExceptionType exceptionType,
            byte[] exceptionMessageBuffer);
        
        internal static Tf2DotNetNativeBufferCoreLookupTransformFullType tf2_dotnet_native_buffer_core_lookup_transform_full = null;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int Tf2DotNetNativeBufferCoreCanTransformType(
            SafeBufferCoreHandle bufferCore,
            [MarshalAs(UnmanagedType.LPStr)] string targetFrame,
            [MarshalAs(UnmanagedType.LPStr)] string sourceFrame,
            int sec,
            uint nanosec,
            byte[] errorMessageBuffer,
            out Tf2ExceptionType exceptionType,
            byte[] exceptionMessageBuffer);

        internal static Tf2DotNetNativeBufferCoreCanTransformType tf2_dotnet_native_buffer_core_can_transform = null;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int Tf2DotNetNativeBufferCoreCanTransformFullType(
            SafeBufferCoreHandle bufferCore,
            [MarshalAs(UnmanagedType.LPStr)] string targetFrame,
            int targetSec,
            uint targetNanosec,
            [MarshalAs(UnmanagedType.LPStr)] string sourceFrame,
            int sourceSec,
            uint sourceNanosec,
            [MarshalAs(UnmanagedType.LPStr)] string fixedFrame,
            byte[] errorMessageBuffer,
            out Tf2ExceptionType exceptionType,
            byte[] exceptionMessageBuffer);
        internal static Tf2DotNetNativeBufferCoreCanTransformFullType tf2_dotnet_native_buffer_core_can_transform_full = null;

        static Interop()
        {
            _dllLoadUtils = DllLoadUtilsFactory.GetDllLoadUtils();
            IntPtr nativeLibrary = _dllLoadUtils.LoadLibrary("tf2_dotnet");

            IntPtr tf2_dotnet_native_buffer_core_create_ptr = _dllLoadUtils.GetProcAddress(nativeLibrary, "tf2_dotnet_native_buffer_core_create");
            tf2_dotnet_native_buffer_core_create = (Tf2DotNetNativeBufferCoreCreateType)Marshal.GetDelegateForFunctionPointer(tf2_dotnet_native_buffer_core_create_ptr, typeof(Tf2DotNetNativeBufferCoreCreateType));

            IntPtr tf2_dotnet_native_buffer_core_destroy_ptr = _dllLoadUtils.GetProcAddress(nativeLibrary, "tf2_dotnet_native_buffer_core_destroy");
            tf2_dotnet_native_buffer_core_destroy = (Tf2DotNetNativeBufferCoreDestroyType)Marshal.GetDelegateForFunctionPointer(tf2_dotnet_native_buffer_core_destroy_ptr, typeof(Tf2DotNetNativeBufferCoreDestroyType));

            IntPtr tf2_dotnet_native_buffer_core_set_transform_ptr = _dllLoadUtils.GetProcAddress(nativeLibrary, "tf2_dotnet_native_buffer_core_set_transform");
            tf2_dotnet_native_buffer_core_set_transform = (Tf2DotNetNativeBufferCoreSetTransformType)Marshal.GetDelegateForFunctionPointer(tf2_dotnet_native_buffer_core_set_transform_ptr, typeof(Tf2DotNetNativeBufferCoreSetTransformType));
        
            IntPtr tf2_dotnet_native_buffer_core_lookup_transform_ptr = _dllLoadUtils.GetProcAddress(nativeLibrary, "tf2_dotnet_native_buffer_core_lookup_transform");
            tf2_dotnet_native_buffer_core_lookup_transform = (Tf2DotNetNativeBufferCoreLookupTransformType)Marshal.GetDelegateForFunctionPointer(tf2_dotnet_native_buffer_core_lookup_transform_ptr, typeof(Tf2DotNetNativeBufferCoreLookupTransformType));

            IntPtr tf2_dotnet_native_buffer_core_lookup_transform_full_ptr = _dllLoadUtils.GetProcAddress(nativeLibrary, "tf2_dotnet_native_buffer_core_lookup_transform_full");
            tf2_dotnet_native_buffer_core_lookup_transform_full = (Tf2DotNetNativeBufferCoreLookupTransformFullType)Marshal.GetDelegateForFunctionPointer(tf2_dotnet_native_buffer_core_lookup_transform_full_ptr, typeof(Tf2DotNetNativeBufferCoreLookupTransformFullType));
        
            IntPtr tf2_dotnet_native_buffer_core_can_transform_ptr = _dllLoadUtils.GetProcAddress(nativeLibrary, "tf2_dotnet_native_buffer_core_can_transform");
            tf2_dotnet_native_buffer_core_can_transform = (Tf2DotNetNativeBufferCoreCanTransformType)Marshal.GetDelegateForFunctionPointer(tf2_dotnet_native_buffer_core_can_transform_ptr, typeof(Tf2DotNetNativeBufferCoreCanTransformType));

            IntPtr tf2_dotnet_native_buffer_core_can_transform_full_ptr = _dllLoadUtils.GetProcAddress(nativeLibrary, "tf2_dotnet_native_buffer_core_can_transform_full");
            tf2_dotnet_native_buffer_core_can_transform_full = (Tf2DotNetNativeBufferCoreCanTransformFullType)Marshal.GetDelegateForFunctionPointer(tf2_dotnet_native_buffer_core_can_transform_full_ptr, typeof(Tf2DotNetNativeBufferCoreCanTransformFullType));
        }
    }
}
