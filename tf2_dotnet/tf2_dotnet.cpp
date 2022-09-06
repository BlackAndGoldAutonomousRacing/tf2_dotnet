#include <assert.h>
#include <stdlib.h>
#include <iostream>

#include <rcl/error_handling.h>
#include <rcl/node.h>
#include <rcl/rcl.h>
#include <rmw/rmw.h>

#include "tf2_dotnet.h"

extern "C" {
void native_tf2_init()
{
  tf2_buffer_core = new tf2::BufferCore();
}

void native_tf2_add_transform(int32_t sec, uint32_t nanosec,
  const char * frame_id, const char * child_frame_id,
  double trans_x, double trans_y, double trans_z,
  double rot_x, double rot_y, double rot_z, double rot_w, int32_t is_static)
  {
    geometry_msgs::msg::TransformStamped transform;

    transform.header.stamp.sec = sec;
    transform.header.stamp.nanosec = nanosec;
    transform.header.frame_id = std::string(frame_id);
    transform.child_frame_id = std::string(child_frame_id);
    transform.transform.translation.x = trans_x;
    transform.transform.translation.y = trans_y;
    transform.transform.translation.z = trans_z;
    transform.transform.rotation.x = rot_x;
    transform.transform.rotation.y = rot_y;
    transform.transform.rotation.z = rot_z;
    transform.transform.rotation.w = rot_w;

    tf2_buffer_core->setTransform(transform, "tf2_dotnet", is_static == 1);
  }

Tf2DotnetTransformStamped
tf2_lookup_transform(
    const char * target_frame, const char * source_frame,
    const tf2::TimePoint& time)
{
  Tf2DotnetTransformStamped ret;

  try {
    geometry_msgs::msg::TransformStamped transform =
      tf2_buffer_core->lookupTransform(std::string(target_frame), std::string(source_frame), time);

    ret.sec = transform.header.stamp.sec;
    ret.nanosec = transform.header.stamp.nanosec;
    ret.translation_x = transform.transform.translation.x;
    ret.translation_y = transform.transform.translation.y;
    ret.translation_z = transform.transform.translation.z;
    ret.rotation_x = transform.transform.rotation.x;
    ret.rotation_y = transform.transform.rotation.y;
    ret.rotation_z = transform.transform.rotation.z;
    ret.rotation_w = transform.transform.rotation.w;
    ret.valid = 1;
  }catch(const tf2::LookupException& e) {
    std::cerr<<"tf2::LookupException "<<e.what()<<std::endl;
    ret.valid = 0;
  }catch(const tf2::ConnectivityException& e) {
    std::cerr<<"tf2::ConnectivityException "<<e.what()<<std::endl;
    ret.valid = 0;
  }catch(const tf2::ExtrapolationException& e) {
    std::cerr<<"tf2::ExtrapolationException "<<e.what()<<std::endl;
    ret.valid = 0;
  }catch(const tf2::InvalidArgumentException& e) {
    std::cerr<<"tf2::InvalidArgumentException "<<e.what()<<std::endl;
    ret.valid = 0;
  }

  return ret;
}

Tf2DotnetTransformStamped
native_tf2_lookup_transform(
    const char * target_frame, const char * source_frame,
    int32_t sec, uint32_t nanosec)
{
  return tf2_lookup_transform(target_frame, source_frame,
    tf2::TimePoint(std::chrono::seconds(sec) + std::chrono::nanoseconds(nanosec)));
}

Tf2DotnetTransformStamped
native_tf2_lookup_last_transform(
    const char * target_frame, const char * source_frame)
{
  return tf2_lookup_transform(target_frame, source_frame, tf2::TimePointZero);
}



}
