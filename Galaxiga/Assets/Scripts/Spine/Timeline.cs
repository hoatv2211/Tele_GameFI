using System;

namespace Spine
{
	public interface Timeline
	{
		void Apply(Skeleton skeleton, float lastTime, float time, ExposedList<Event> events, float alpha, MixPose pose, MixDirection direction);

		int PropertyId { get; }
	}
}
