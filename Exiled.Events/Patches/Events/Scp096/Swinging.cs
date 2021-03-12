// -----------------------------------------------------------------------
// <copyright file="Swinging.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Exiled.Events.Patches.Events.Scp096
{
#pragma warning disable SA1118
    using System.Collections.Generic;
    using System.Reflection.Emit;

    using Exiled.API.Features;
    using Exiled.Events.EventArgs;

    using HarmonyLib;

    using NorthwoodLib.Pools;

    using UnityEngine;

    using static HarmonyLib.AccessTools;

    /// <summary>
    /// Patches the <see cref="PlayableScps.Scp096.Attack"/> method.
    /// Adds the <see cref="Handlers.Scp096.Swinging"/> event.
    /// </summary>
    internal static class Swinging
    {
        [HarmonyPatch(typeof(PlayableScps.Scp096), nameof(PlayableScps.Scp096.AttackTriggerPhysics))]
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            Log.Info("Test");
            var newInstructions = ListPool<CodeInstruction>.Shared.Rent(instructions);

            // The index offsets.
            const int doorVariantEventOffset = 10;
            const int breakableWindowEventOffset = 0;
            const int referenceHubEventOffset = 30;

            // The already defined local indices.
            const int doorVariantLocal = 7;
            const int breakableWindowLocal = 9;
            const int referenceHubLocal = 10;

            // Find the event indicies.
            var doorVariantEventIndex = newInstructions.FindLastIndex(instruction => instruction.opcode == OpCodes.Ldloc_S && (int)instruction.operand == doorVariantLocal) + doorVariantEventOffset;
            var breakableWindowEventIndex = newInstructions.FindLastIndex(instruction => instruction.opcode == OpCodes.Ldloc_S && (int)instruction.operand == breakableWindowLocal) + breakableWindowEventOffset;
            var referenceHubEventIndex = newInstructions.FindIndex(instruction => instruction.opcode == OpCodes.Stloc_S && (int)instruction.operand == referenceHubLocal) + referenceHubEventOffset;

            // Get the needed labels.
            var doorVariantLabel = newInstructions[doorVariantEventIndex - 1].operand;
            var breakableWindowLabel = newInstructions[breakableWindowEventIndex - 1].operand;
            var referenceHubLabel = newInstructions[referenceHubEventIndex - 1].operand;

            // Get the count to find the previous index.
            var oldCount = newInstructions.Count;

            // Get the instructions.
            var doorVariantInstructions = GetInstructions(doorVariantLocal, 250f, doorVariantLabel);
            var breakableWindowInstructions = GetInstructions(breakableWindowLocal, 500f, breakableWindowLabel);
            var referenceHubInstructions = GetInstructions(referenceHubLocal, 9696f, referenceHubLabel);

            // Add the instructions.
            newInstructions.InsertRange(doorVariantEventIndex, doorVariantInstructions);
            newInstructions.InsertRange(breakableWindowEventIndex + doorVariantInstructions.Length, breakableWindowInstructions);
            newInstructions.InsertRange(referenceHubEventIndex + doorVariantInstructions.Length + breakableWindowInstructions.Length, referenceHubInstructions);

            // Move all labels.
            newInstructions[doorVariantEventIndex].MoveLabelsFrom(newInstructions[newInstructions.Count - oldCount + doorVariantEventIndex]);
            newInstructions[breakableWindowEventIndex].MoveLabelsFrom(newInstructions[newInstructions.Count - oldCount + breakableWindowEventIndex]);
            newInstructions[referenceHubEventIndex].MoveLabelsFrom(newInstructions[newInstructions.Count - oldCount + referenceHubEventIndex]);

            for (int z = 0; z < newInstructions.Count; z++)
            {
                Log.Info(newInstructions[z].opcode + ") " + newInstructions[z].operand);
                yield return newInstructions[z];
            }

            ListPool<CodeInstruction>.Shared.Return(newInstructions);
        }

        private static CodeInstruction[] GetInstructions(int localIndex, float damage, object label)
        {
            var instructions = new[]
            {
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Ldfld, Field(typeof(PlayableScps.PlayableScp), nameof(PlayableScps.PlayableScp.Hub))),
                new CodeInstruction(OpCodes.Call, Method(typeof(Player), nameof(Player.Get), new[] { typeof(ReferenceHub) })),
                new CodeInstruction(OpCodes.Ldloc_S, localIndex),
                new CodeInstruction(OpCodes.Call, PropertyGetter(typeof(Component), nameof(Component.gameObject))),
                new CodeInstruction(OpCodes.Ldc_R4, damage),
                new CodeInstruction(OpCodes.Ldc_I4_1),
                new CodeInstruction(OpCodes.Newobj, GetDeclaredConstructors(typeof(SwingingEventArgs))[0]),
                new CodeInstruction(OpCodes.Dup),
                new CodeInstruction(OpCodes.Call, Method(typeof(Handlers.Scp096), nameof(Handlers.Scp096.OnSwinging))),
                new CodeInstruction(OpCodes.Call, PropertyGetter(typeof(SwingingEventArgs), nameof(SwingingEventArgs.IsAllowed))),
                new CodeInstruction(OpCodes.Brfalse_S, label),
            };



            return instructions;
        }
    }
}
