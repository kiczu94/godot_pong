using System.Linq;
using Godot;

namespace pong_1.Scripts.Utilities;

public static class KinematicCollision2DExtension
{
    public static StringName[] GetColliderGroup(this KinematicCollision2D collision2D)
    {
        var collidedNode = collision2D.GetCollider();
        var colliderPath = collidedNode.Call("get_path").ToString();
        var mainLoop = Godot.Engine.GetMainLoop();
        var sceneTree = mainLoop as SceneTree;
        return new Node().GetNode(colliderPath).GetGroups().ToArray();
    }
}
