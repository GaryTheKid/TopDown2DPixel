/* Last Edition: 07/07/2022
 * Author: Chongyang Wang
 * Collaborators: 
 * reference: Codemonkey
 * 
 * Description: 
 *   The path follow component system. Instruct each entity with a path position buffer to move
 *   step by step.
 * Last Edition:
 *   Set to legacy
 */

using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

public class PathFollowSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        /*float deltaTime = Time.DeltaTime;

        Entities.ForEach((Entity entity, DynamicBuffer<PathPosition> pathPositionBuffer, ref Translation translation, ref PathFollow pathFollow) =>
        {
            if (pathFollow.pathIndex >= 0)
            {
                PathPosition pathPosition = pathPositionBuffer[pathFollow.pathIndex];

                float3 targetPosition = new float3(pathPosition.position.x, pathPosition.position.y, 0);
                float3 moveDir = math.normalizesafe(targetPosition - translation.Value);
                float moveSpeed = 3f;

                translation.Value += moveDir * moveSpeed * deltaTime;

                if (math.distance(translation.Value, targetPosition) < 0.1f)
                {
                    // Next Waypoint
                    pathFollow.pathIndex--;
                }
            }
        });*/
    }

}
