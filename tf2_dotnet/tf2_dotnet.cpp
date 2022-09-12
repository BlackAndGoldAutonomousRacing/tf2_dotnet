#include <assert.h>
#include <stdlib.h>
#include <iostream>

#include <rcl/error_handling.h>
#include <rcl/node.h>
#include <rcl/rcl.h>
#include <rmw/rmw.h>

#include "tf2_dotnet.h"

extern "C" {

void tf2_convert_exception(Tf2DotnetExceptionType * exception_type, char * exception_message_buffer)
{
  // See https://stackoverflow.com/a/48036486, this is called a Lippincott function.
  try
  {
    // This re-throws the exception from the catch block in the calling function.
    // This allows to match again to the different types.
    throw;
  }
  catch (const tf2::LookupException& e)
  {
    *exception_type = TF2_DOTNET_LOOKUP_EXCEPTION;
    strncpy(exception_message_buffer, e.what(), TF2_DOTNET_EXCEPTION_MESSAGE_BUFFER_LENGTH);
  }
  catch (const tf2::ConnectivityException& e)
  {
    *exception_type = TF2_DOTNET_CONNECTIVITY_EXCEPTION;
    strncpy(exception_message_buffer, e.what(), TF2_DOTNET_EXCEPTION_MESSAGE_BUFFER_LENGTH);
  }
  catch (const tf2::ExtrapolationException& e)
  {
    *exception_type = TF2_DOTNET_EXTRAPOLATION_EXCEPTION;
    strncpy(exception_message_buffer, e.what(), TF2_DOTNET_EXCEPTION_MESSAGE_BUFFER_LENGTH);
  }
  catch (const tf2::InvalidArgumentException& e)
  {
    *exception_type = TF2_DOTNET_INVALID_ARGUMENT_EXCEPTION;
    strncpy(exception_message_buffer, e.what(), TF2_DOTNET_EXCEPTION_MESSAGE_BUFFER_LENGTH);
  }
  catch (const tf2::TimeoutException& e)
  {
    *exception_type = TF2_DOTNET_TIMEOUT_EXCEPTION;
    strncpy(exception_message_buffer, e.what(), TF2_DOTNET_EXCEPTION_MESSAGE_BUFFER_LENGTH);
  }
  catch (const tf2::TransformException& e)
  {
    *exception_type = TF2_DOTNET_TRANSFORM_EXCEPTION;
    strncpy(exception_message_buffer, e.what(), TF2_DOTNET_EXCEPTION_MESSAGE_BUFFER_LENGTH);
  }
  catch (const std::runtime_error& e)
  {
    *exception_type = TF2_DOTNET_EXCEPTION;
    strncpy(exception_message_buffer, e.what(), TF2_DOTNET_EXCEPTION_MESSAGE_BUFFER_LENGTH);
  }
  catch (...)
  {
    *exception_type = TF2_DOTNET_UNKNOWN_EXCEPTION;
    memset(exception_message_buffer, 0, TF2_DOTNET_EXCEPTION_MESSAGE_BUFFER_LENGTH);
  }
}

tf2::BufferCore *
tf2_dotnet_native_buffer_core_create(
  Tf2DotnetExceptionType * exception_type,
  char * exception_message_buffer)
{
  try
  {
    return new tf2::BufferCore();
  }
  catch (...)
  {
    tf2_convert_exception(exception_type, exception_message_buffer);
    return nullptr;
  }
}

void
tf2_dotnet_native_buffer_core_destroy(
  tf2::BufferCore * buffer_core,
  Tf2DotnetExceptionType * exception_type,
  char * exception_message_buffer)
{
  try
  {
    delete buffer_core;
  }
  catch (...)
  {
    tf2_convert_exception(exception_type, exception_message_buffer);
  }
}

void native_tf2_init(Tf2DotnetExceptionType * exception_type, char * exception_message_buffer)
{
  try
  {
    tf2_buffer_core = new tf2::BufferCore();
  }
  catch (...)
  {
    tf2_convert_exception(exception_type, exception_message_buffer);
  }
}

void native_tf2_add_transform(int32_t sec, uint32_t nanosec,
  const char * frame_id, const char * child_frame_id,
  double trans_x, double trans_y, double trans_z,
  double rot_x, double rot_y, double rot_z, double rot_w, int32_t is_static,
  Tf2DotnetExceptionType * exception_type, char * exception_message_buffer)
{
  try
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
  catch (...)
  {
    tf2_convert_exception(exception_type, exception_message_buffer);
  }
}

Tf2DotnetTransformStamped
tf2_lookup_transform(
    const char * target_frame, const char * source_frame,
    const tf2::TimePoint& time)
{
  Tf2DotnetTransformStamped ret;

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

  return ret;
}
Tf2DotnetTransformStamped
native_tf2_lookup_transform(
    const char * target_frame, const char * source_frame,
    int32_t sec, uint32_t nanosec,
    Tf2DotnetExceptionType * exception_type, char * exception_message_buffer)
{
  try
  {
    return tf2_lookup_transform(target_frame, source_frame,
      tf2::TimePoint(std::chrono::seconds(sec) + std::chrono::nanoseconds(nanosec)));
  }
  catch (...)
  {
    tf2_convert_exception(exception_type, exception_message_buffer);
    return Tf2DotnetTransformStamped();
  }
}

Tf2DotnetTransformStamped
native_tf2_lookup_last_transform(
    const char * target_frame, const char * source_frame,
    Tf2DotnetExceptionType * exception_type, char * exception_message_buffer)
{
  try
  {
    return tf2_lookup_transform(target_frame, source_frame, tf2::TimePointZero);
  }
  catch (...)
  {
    tf2_convert_exception(exception_type, exception_message_buffer);
    return Tf2DotnetTransformStamped();
  }
}



}
