using Ibit.Core.Util;
using NUnit.Framework;
using UnityEngine;

public class FlowMathTest
{
    [Test]
    public void TestEquationsPitaco()
    {
        var diffpress = 1100f;
        Debug.Log("Volumetric Flow Rate: " + PitacoFlowMath.ToLitresPerMinute(diffpress) + " L/min");
    }

    [Test]
    public void TestEquationsMano()
    {
        var diffpress = 1100f;
        Debug.Log("Pression Rate: " + ManoFlowMath.ToCentimetersofWater(diffpress) + " CmH2O");
    }

    [Test]
    public void TestEquationsCinta()
    {
        var diffpress = 1100f;
        Debug.Log("Volumetric Flow Rate: " + CintaFlowMath.ToLitresPerMinute(diffpress) + " L/min");
    }
}