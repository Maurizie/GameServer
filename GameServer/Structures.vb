Public Class Structures
    Private BaseMetal As Integer = 10
    Private BaseSilicon As Integer = 10
    Private BaseTritium As Integer = 10
    Private BaseExponent As Single = 1.5

    Sub New(ByVal metal As Integer, ByVal silicon As Integer, ByVal tritium As Integer, Optional ByVal exponent As Single = 1.5)
        BaseMetal = metal
        BaseSilicon = silicon
        BaseTritium = tritium
        BaseExponent = exponent

    End Sub

    Public Sub CalculateCost(level As Integer, ByRef MetalCost As Integer, ByRef SiliconCost As Integer, ByRef TritiumCost As Integer)
        MetalCost = (BaseMetal * (BaseExponent ^ (level - 1)))
        SiliconCost = (BaseSilicon * (BaseExponent ^ (level - 1)))
        TritiumCost = (BaseTritium * (BaseExponent ^ (level - 1)))

    End Sub



End Class

