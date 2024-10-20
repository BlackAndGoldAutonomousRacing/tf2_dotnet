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

using System.Diagnostics;
using Microsoft.Win32.SafeHandles;

namespace ROS2.Tf2DotNet
{
    /// <summary>
    /// Safe handle representing a tf2::BufferCore.
    /// </summary>
    internal sealed class SafeBufferCoreHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        public SafeBufferCoreHandle()
            : base(ownsHandle: true)
        {
        }

        protected override bool ReleaseHandle()
        {
            Tf2ExceptionHelper.ResetMessage();
            Interop.tf2_dotnet_native_buffer_core_destroy(handle, out Tf2ExceptionType exceptionType, Tf2ExceptionHelper.MessageBuffer);

            bool successfullyFreed = exceptionType == Tf2ExceptionType.NoException;
            Debug.Assert(successfullyFreed);

            return successfullyFreed;
        }
    }
}
