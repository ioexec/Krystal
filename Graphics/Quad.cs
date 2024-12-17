using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

namespace Krystal.Graphics;


/// <summary>
/// Represents a Quad
/// </summary>
public struct Quad
{
    public Vector3[] _vertices;
    public Vector2[] _uvs;
    public Vector3[] _normals;

    public Vector3 A
    {
        get => _vertices[0];
        set => _vertices[0] = value;
    }

    public Quad()
    {
        _vertices = new Vector3[4];
        _uvs = new Vector2[4];
        _normals = new Vector3[4];
    }
    
    public Vector3 B
    {
        get => _vertices[1];
        set => _vertices[1] = value;
    }

    public Vector3 C
    {
        get => _vertices[2];
        set => _vertices[2] = value;
    }

    public Vector3 D
    {
        get => _vertices[3];
        set => _vertices[3] = value;
    }

    public Vector2 UV_A
    {
        get => _uvs[0];
        set => _uvs[0] = value;
    }

    public Vector2 UV_B
    {
        get => _uvs[1];
        set => _uvs[1] = value;
    }

    public Vector2 UV_C
    {
        get => _uvs[2];
        set => _uvs[2] = value;
    }

    public Vector2 UV_D
    {
        get => _uvs[3];
        set => _uvs[3] = value;
    }

    public Vector3 NORMAL_A
    {
        get => _normals[1];
        set => _normals[1] = value;
    }

    public Vector3 NORMAL_B
    {
        get => _normals[2];
        set => _normals[2] = value;
    }

    public Vector3 NORMAL_C
    {
        get => _normals[3];
        set => _normals[3] = value;
    }

    public Vector3 NORMAL_D
    {
        get => _normals[4];
        set => _normals[4] = value;
    }

    public static Quad operator *(Quad a, Vector3 vec)
    {
        Quad b = new Quad();

        b.A = a.A * vec;
        b.B = a.B * vec;
        b.C = a.C * vec;
        b.D = a.D * vec;

        return b;
    }
    
    /// <summary>
    /// Merges a set of quads into a larger quad.
    /// Assumes all vertices have one axis in common, and that all normals are the same.
    /// </summary>
    /// <param name="quads"></param>
    /// <returns></returns>
    public static Quad Merge(ICollection<Quad> quads)
    {
        // Simple Merge :)
        
        Vector3 UP = new Vector3(0.0f, 1.0f, 0.0f);
        Vector3 DOWN = new Vector3(0.0f, -1.0f, 0.0f);
        Vector3 LEFT = new Vector3(-1.0f, 0.0f, 0.0f);
        Vector3 RIGHT = new Vector3(1.0f, 0.0f, 0.0f);
        Vector3 FORWARDS = new Vector3(0.0f, 0.0f, 1.0f);
        Vector3 BACKWARDS = new Vector3(0.0f, 1.0f, -1.0f);
        
        Quad q = new Quad();
        
        float mostX = 0.0f;
        float mostY = 0.0f;
        float leastX = 0.0f;
        float leastY = 0.0f;
        float mostZ = 0.0f;
        float leastZ = 0.0f;
        
        
        if (quads.ElementAt(0).NORMAL_A == UP || quads.ElementAt(0).NORMAL_A == DOWN)
        {
            foreach (Quad quad in quads)
            {
                foreach (var vertex in quad._vertices)
                {
                    if (vertex.X >= mostX)
                        mostX = vertex.X;
                    else if (vertex.X <= leastX)
                        leastX = vertex.X;
                
                    if (vertex.Z >= mostZ)
                        mostZ = vertex.Z;
                    else if (vertex.Z <= leastZ)
                        leastZ = vertex.Z;
                }
            }
            
            q.A = new Vector3(mostX,  quads.ElementAt(0).A.Y, leastZ);
            q.B = new Vector3(mostX,  quads.ElementAt(0).B.Y, mostZ);
            q.C = new Vector3(leastX, quads.ElementAt(0).C.Y, leastZ);
            q.D = new Vector3(leastX, quads.ElementAt(0).D.Y, mostZ);
            
        }
        else if (quads.ElementAt(0).NORMAL_A == LEFT || quads.ElementAt(0).NORMAL_A == RIGHT)
        {
            foreach (Quad quad in quads)
            {
                foreach (var vertex in quad._vertices)
                {
                    if (vertex.Z >= mostZ)
                        mostZ = vertex.Z;
                    else if (vertex.Z <= leastZ)
                        leastZ = vertex.Z;
                
                    if (vertex.Y >= mostY)
                        mostY = vertex.Y;
                    else if (vertex.Y <= leastY)
                        leastY = vertex.Y;
                }
            }
            
            q.A = new Vector3(quads.ElementAt(0).A.X, mostY, leastZ);
            q.B = new Vector3(quads.ElementAt(0).B.X, mostY, mostZ);
            q.C = new Vector3(quads.ElementAt(0).C.X, leastY,leastZ);
            q.D = new Vector3(quads.ElementAt(0).D.X, leastY, mostZ);
        }
        else if (quads.ElementAt(0).NORMAL_A == FORWARDS || quads.ElementAt(0).NORMAL_A == BACKWARDS)
        {
            foreach (Quad quad in quads)
            {
                foreach (var vertex in quad._vertices)
                {
                    if (vertex.X >= mostX)
                        mostX = vertex.X;
                    else if (vertex.X <= leastX)
                        leastX = vertex.X;
                
                    if (vertex.Y >= mostY)
                        mostY = vertex.Y;
                    else if (vertex.Y <= leastY)
                        leastY = vertex.Y;
                }
            }
            
            q.A = new Vector3(quads.ElementAt(0).A.X, mostY, leastZ);
            q.B = new Vector3(quads.ElementAt(0).B.X, mostY, mostZ);
            q.C = new Vector3(quads.ElementAt(0).C.X, leastY,leastZ);
            q.D = new Vector3(quads.ElementAt(0).D.X, leastY, mostZ);
        }
        
        return q;
    }
    
}