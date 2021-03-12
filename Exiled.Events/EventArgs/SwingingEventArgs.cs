// -----------------------------------------------------------------------
// <copyright file="SwingingEventArgs.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Exiled.Events.EventArgs
{
    using System;

    using Exiled.API.Features;

    using UnityEngine;

    using Scp096 = PlayableScps.Scp096;

    /// <summary>
    /// Contains all informations before SCP-096 swings at a target.
    /// </summary>
    public class SwingingEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SwingingEventArgs"/> class.
        /// </summary>
        /// <param name="scp096"><inheritdoc cref="Scp096"/></param>
        /// <param name="player"><inheritdoc cref="Player"/></param>
        /// <param name="target"><inheritdoc cref="Target"/></param>
        /// <param name="damage"><inheritdoc cref="Damage"/></param>
        /// <param name="isAllowed"><inheritdoc cref="IsAllowed"/></param>
        public SwingingEventArgs(Scp096 scp096, Player player, GameObject target, float damage, bool isAllowed = true)
        {
            Scp096 = scp096;
            Player = player;
            Target = target;
            Damage = damage;
            IsAllowed = isAllowed;
        }

        /// <summary>
        /// Gets the SCP-096 instance.
        /// </summary>
        public Scp096 Scp096 { get; }

        /// <summary>
        /// Gets the player who is controlling SCP-096.
        /// </summary>
        public Player Player { get; }

        /// <summary>
        /// Gets the target the user is swinging at.
        /// </summary>
        public GameObject Target { get; }

        /// <summary>
        /// Gets or sets the amount of damage SCP-096 will deal to the target.
        /// </summary>
        public float Damage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether SCP-096 can swing.
        /// </summary>
        public bool IsAllowed { get; set; }
    }
}
