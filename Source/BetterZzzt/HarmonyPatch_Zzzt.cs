using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Verse;
using OpCodes = System.Reflection.Emit.OpCodes;

namespace BetterZzzt
{
    [HarmonyPatch(typeof(ShortCircuitUtility), "DrainBatteriesAndCauseExplosion")]
    public static class BetterZzztHarmonyPatch
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> instructionList = instructions.ToList();

            for (int i = 0; i < instructionList.Count; i++)
            {
                CodeInstruction instruction = instructionList[i];

                if (instruction.opcode == OpCodes.Callvirt &&
                   instruction.OperandIs(AccessTools.Method(type: typeof(CompPowerBattery), name: nameof(CompPowerBattery.DrawPower))))
                {
                    yield return new CodeInstruction(opcode: OpCodes.Ldc_R4, operand: 1.33f);
                    yield return new CodeInstruction(opcode: OpCodes.Div);
                }

                yield return instruction;
            }
        }
    }

    [StaticConstructorOnStartup]
    public static class StartUp
    {
        static StartUp()
        {
            var harmony = new Harmony("BetterZzzt");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}