﻿using Microsoft.EntityFrameworkCore;

namespace Coordinator.Models.Contexts
{
    public class TwoPhaseCommitContext:DbContext
    {
        public DbSet<Node> Nodes { get; set; }
        public DbSet<NodeState> NodeStates { get; set; }

    }
}
