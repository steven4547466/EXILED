// -----------------------------------------------------------------------
// <copyright file="JumpedEventArgs.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Exiled.Events.EventArgs
{
    using System;

    using Exiled.API.Features;

    /// <summary>
    /// Contains all informations after a player starts or stops jumping.
    /// </summary>
    public class JumpedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JumpedEventArgs"/> class.
        /// </summary>
        /// <param name="player"><inheritdoc cref="Player"/></param>
        /// <param name="jumping"><inheritdoc cref="bool"/></param>
        public JumpedEventArgs(Player player, bool jumping)
        {
            Player = player;
            Jumping = jumping;
        }

        /// <summary>
        /// Gets the player who is jumping.
        /// </summary>
        public Player Player { get; }

        /// <summary>
        /// Gets a value indicating whether or not the <see cref="Player"/> is jumping or not.
        /// </summary>
        public bool Jumping { get; }
    }
}
