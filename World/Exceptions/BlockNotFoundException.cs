using System;

namespace Krystal.World.Exceptions;

public class BlockNotFoundException : Exception
{
    public BlockNotFoundException(int attemptedId) : base($"Block {attemptedId} is not a valid block type") {}
}