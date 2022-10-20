/* Copyright 2022 Stefan Hoffmann <stefan.hoffmann@schiller.de>
 * Copyright 2019 Francisco Martin <fmrico@gmail.com>
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

using geometry_msgs.msg;
using ROS2;
using tf2_msgs.msg;

namespace Ros2.Tf2DotNet
{
    public sealed class TransformListener
    {
        private readonly TransformBuffer _buffer;

        // TODO: Implement IDisposable once rcldotnet supports unsubscribing.
        private readonly Subscription<TFMessage> _tfSubscription;
        private readonly Subscription<TFMessage> _tfStaticSubscription;

        /// <summary>
        /// The TF2 dynamic listener qos profile.
        /// </summary>
        public static QosProfile DynamicListenerQosProfile { get; } = QosProfile.KeepLast(100);

        /// <summary>
        /// The TF2 static listener qos profile.
        /// </summary>
        public static QosProfile StaticListenerQosProfile { get; } = QosProfile.KeepLast(100).WithTransientLocal();

        public TransformListener(TransformBuffer buffer, Node node)
        {
            _buffer = buffer;

            _tfSubscription = node.CreateSubscription<TFMessage>("/tf", (TFMessage message) =>
            {
                SubscriptionCallback(message, isStatic: false);
            }, DynamicListenerQosProfile);

            _tfStaticSubscription = node.CreateSubscription<TFMessage>("/tf_static", (TFMessage message) =>
            {
                SubscriptionCallback(message, isStatic: true);
            }, StaticListenerQosProfile);

            // Suppress field not used exceptions.
            _ = _tfSubscription;
            _ = _tfStaticSubscription;
        }

        private void SubscriptionCallback(TFMessage message, bool isStatic)
        {
            foreach (TransformStamped transform in message.Transforms)
            {
                _buffer.SetTransform(transform, "ros2_dotnet", isStatic);
            }
        }
    }
}
