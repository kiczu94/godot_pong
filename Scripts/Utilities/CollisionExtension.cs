using System.Collections.Generic;
using Godot;

namespace pong_1.Scripts.Utilities;

public static class CollisionExtensions
{
    public static IEnumerable<StringName> GetColliderGroups(this Node2D node, KinematicCollision2D collision)
    {
        var collider = collision.GetCollider();
        var colliderPath = collider.Call("get_path").ToString();
        var colliderNode = node.GetNode(colliderPath);
        return colliderNode.GetGroups();
    }
}
