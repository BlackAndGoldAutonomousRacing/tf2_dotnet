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

namespace ROS2.Tf2DotNet
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
        public static QualityOfServiceProfile DynamicListenerQosProfile { get{
            //= QualityOfServiceProfile.KeepLast(100);
            QualityOfServiceProfile qos = new QualityOfServiceProfile();
            qos.SetHistory(HistoryPolicy.QOS_POLICY_HISTORY_KEEP_LAST,100);
            return qos;
        } }

        /// <summary>
        /// The TF2 static listener qos profile.
        /// </summary>
        public static QualityOfServiceProfile StaticListenerQosProfile { get{
            //= QualityOfServiceProfile.KeepLast(1).WithTransientLocal();
            QualityOfServiceProfile qos = new QualityOfServiceProfile();
            qos.SetHistory(HistoryPolicy.QOS_POLICY_HISTORY_KEEP_LAST,1);
            qos.SetDurability(DurabilityPolicy.QOS_POLICY_DURABILITY_TRANSIENT_LOCAL);
            return qos;
        } }

        public TransformListener(TransformBuffer buffer, INode node)
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
            // foreach (var trans in message.Transforms) {
            //     System.Console.WriteLine($"{(isStatic ? "STATIC" : "")} {trans.Header.Frame_id} -> {trans.Child_frame_id}");
            // }
            foreach (TransformStamped transform in message.Transforms)
            {
                _buffer.SetTransform(transform, "ros2_dotnet", isStatic);
            }
        }
    }
}
